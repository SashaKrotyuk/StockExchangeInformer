namespace SEI.News.Service.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Caching;
	using System.Threading;

	using SEI.News.Model;

	public class RssCache
	{
		public const int DefaultCacheItemsLength = 100;
		private static readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();

		public static bool UpdateCache(string sourceName, List<NewsItem> newItems)
		{
			newItems = newItems.OrderByDescending(item => item.Date).Take(DefaultCacheItemsLength).ToList();

			CacheLock.EnterUpgradeableReadLock();
			try
			{
				CacheLock.EnterWriteLock();
				try
				{
					var cachedItems = MemoryCache.Default.Get(sourceName) as List<NewsItem>;

					if (cachedItems != null)
					{
						if (cachedItems.Count == DefaultCacheItemsLength)
						{
							if (newItems.Count == DefaultCacheItemsLength)
							{
								CacheItemPolicy cip = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration };
								MemoryCache.Default.Set(sourceName, newItems, cip);
							}
							else
							{
								for (int i = 0; i < newItems.Count; i++)
								{
									cachedItems.RemoveAt(i);
								}

								cachedItems.AddRange(newItems);
								cachedItems = cachedItems.Take(DefaultCacheItemsLength).ToList();

								CacheItemPolicy cip = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration };
								MemoryCache.Default.Set(sourceName, cachedItems, cip);
							}
						}
						else
						{
							foreach (NewsItem rssItem in newItems)
							{
								if (cachedItems.Count != DefaultCacheItemsLength)
								{
									cachedItems.Add(rssItem);
								}
								else
								{
									cachedItems.RemoveAt(0);
									cachedItems.Add(rssItem);
								}
							}
						}
					}
					else
					{
						CacheItemPolicy cip = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration };
						MemoryCache.Default.Set(sourceName, newItems.Take(DefaultCacheItemsLength).ToList(), cip);
					}

					return true;
				}
				catch
				{
					return false;
				}
				finally
				{
					CacheLock.ExitWriteLock();
				}
			}
			finally
			{
				CacheLock.ExitUpgradeableReadLock();
			}
		}

		public static List<NewsItem> GetCachedRssItems(string sourceName)
		{
			CacheLock.EnterReadLock();
			try
			{
				var cachedItems = MemoryCache.Default.Get(sourceName) as List<NewsItem>;

				if (cachedItems != null)
				{
					return cachedItems;
				}
				else
				{
					return Enumerable.Empty<NewsItem>().ToList();
				}
			}
			catch
			{
				return Enumerable.Empty<NewsItem>().ToList();
			}
			finally
			{
				CacheLock.ExitReadLock();
			}
		}
	}
}