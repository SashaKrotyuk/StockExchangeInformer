namespace SEI.News.Service.Hubs
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.AspNet.SignalR.Hubs;
	using SEI.News.Data.Contracts;
	using SEI.News.Model;
	using SEI.News.Service.Services;
	
	[HubName("newshub")]
	public class NewsHub : BaseHub
	{
		private INewsRssSourceRepository NewsRssSourceRepository
		{
			get
			{
				return this.DependencyResolver.Resolve<INewsRssSourceRepository>();
			}
		}

		public void GetLatestNews()
		{
			using (var rssSourceRepo = this.NewsRssSourceRepository)
			{
				List<NewsRssSource> sources = rssSourceRepo.GetAll().ToList();

				var result = new List<NewsItem>();
				foreach (var source in sources)
				{
					List<NewsItem> latestMessages = RssCache.GetCachedRssItems(source.Name);
					result.AddRange(latestMessages);
				}

				Clients.All.onLatestNews(result);
			}
		}
	}
}