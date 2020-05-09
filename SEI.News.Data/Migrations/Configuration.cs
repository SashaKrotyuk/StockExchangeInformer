namespace SEI.News.Data.Migrations
{
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using SEI.News.Model;

	internal sealed class Configuration : DbMigrationsConfiguration<SEI.News.Data.NewsContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(NewsContext context)
		{
			var defaultRssSources = new List<NewsRssSource>
			{
				NewsRssSource.Create("Nasdaq: Business News", "http://articlefeeds.nasdaq.com/nasdaq/categories?category=Business", true),
				NewsRssSource.Create("Nasdaq: Forex and currencies", "http://articlefeeds.nasdaq.com/nasdaq/categories?category=Forex+and+Currencies", true),
				NewsRssSource.Create("DailyFX: Forex market news and analysis", "https://rss.dailyfx.com/feeds/all", true)
			};

			context.Set<NewsRssSource>().AddOrUpdate(x => x.Url, defaultRssSources.ToArray());

			context.SaveChanges();
		}
	}
}