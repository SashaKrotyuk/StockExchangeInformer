namespace SEI.News.Service.Services
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel.Syndication;
	using System.Threading.Tasks;
	using System.Xml;

	using Common;
	using Common.Exceptions;
	using Common.Extensions;
	using Common.IoC.Autofac;
	using SEI.News.Model;
	using Topshelf.Logging;

	public class RssReader
	{
		private static readonly LogWriter Log = AutofacService.Instance.Resolve<LogWriter>();

		public event Action<List<NewsItem>> NewRssItemsEvent;

		public List<NewsItem> Read(IEnumerable<NewsRssSource> feedSources, DateTime? from, DateTime? to)
		{
			try
			{
				var concurrentCollection = new ConcurrentBag<NewsItem>();

				Parallel.ForEach(
					feedSources,
					async source =>
					{
						var newsFromFeed = await this.ReadRssFeed(source, from, to);
						concurrentCollection.AddRange(newsFromFeed);
					});

				return concurrentCollection.ToList();
			}
			catch
			{
				return Enumerable.Empty<NewsItem>().ToList();
			}
		}

		private async Task<IEnumerable<NewsItem>> ReadRssFeed(NewsRssSource source, DateTime? from, DateTime? to)
		{
			if (source == null)
			{
				throw new ArgumentException("Source must be specified", nameof(source));
			}

			if (string.IsNullOrEmpty(source.Url))
			{
				throw new BusinessException("Source url must be specified");
			}

			try
			{
				Log.InfoFormat("Start fetching {0} RSS feed... (URL: {1})", source.Name, source.Url);

				using (var reader = XmlReader.Create(source.Url))
				{
					var feed = SyndicationFeed.Load(reader);
					if (feed != null)
					{
						var result = feed.Items
							.Where(x => (!from.HasValue || (x.PublishDate.Date >= from.Value.Date && x.PublishDate.Hour >= from.Value.Hour))
										&& (!to.HasValue || (x.PublishDate.Date <= to.Value.Date && x.PublishDate.Hour <= to.Value.Hour)))
							.OrderBy(x => x.PublishDate)
							.ThenBy(x => x.LastUpdatedTime)
							.Select(x => ParseSyndicationItem(x, source.Name))
							.ToList();

						if (result.Any())
						{
							await Task.Run(() =>
							{
								var latestCachedItems = RssCache.GetCachedRssItems(source.Name);
								DateTime latestDate = DateTime.MinValue;

								if (latestCachedItems.Any())
								{
									latestDate = latestCachedItems.Max(e => e.Date);
								}

								var itemsToShare = new List<NewsItem>();
								result = result.Where(item => item.Date > latestDate).ToList();
								foreach (var rssItem in result)
								{
									if (latestCachedItems.All(item => item.UniqueIdentifier != rssItem.UniqueIdentifier))
									{
										itemsToShare.Add(rssItem);
									}
								}

								itemsToShare = itemsToShare.GroupBy(e => e.UniqueIdentifier).Select(e => e.First()).ToList();

								Log.InfoFormat("End fetching {0} RSS feed... (URL: {1}). Count of new items = {2}", source.Name, source.Url, itemsToShare.Count);

								RssCache.UpdateCache(source.Name, itemsToShare);

								if (itemsToShare.Count > 0)
								{
									this.NewRssItemsEvent?.Invoke(itemsToShare);
								}
							});
						}

						return result;
					}
				}
			}
			// TODO: Add logging
			catch (Exception exception)
			{
				Log.ErrorFormat("Error fetching {0} RSS feed... (URL: {1}). Error: {2}", source.Name, source.Url, exception.Message);
				return Enumerable.Empty<NewsItem>();
			}

			return Enumerable.Empty<NewsItem>();
		}

		private static NewsItem ParseSyndicationItem(SyndicationItem syndicationItem, string sourceName)
		{
			string resultingUrl;

			Uri url;
			Uri.TryCreate(syndicationItem.Id, UriKind.Absolute, out url);

			if (url != null)
			{
				resultingUrl = url.ToString();
			}
			else
			{
				resultingUrl = syndicationItem.Links.FirstOrDefault(l => l.RelationshipType == "alternate")?.Uri.ToString();
			}

			return NewsItem.New(
								title: syndicationItem.Title.Text,
								description: syndicationItem.Summary.Text.StripHtml(),
								url: resultingUrl,
								imageUrl: syndicationItem.Links.FirstOrDefault(l => l.MediaType != null && l.MediaType.Contains("image"))?.Uri.AbsoluteUri,
								date: syndicationItem.PublishDate.DateTime,
								timestamp: syndicationItem.PublishDate.UtcDateTime.ToJsTime(),
								sourceName: sourceName);
		}
	}
}