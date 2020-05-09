namespace SEI.News.Data
{
	using System;
	using System.Configuration;
	using System.Data.Entity;
	using System.Linq;

	using Common.Abstractions.Entities;
	using SEI.News.Model;

	public class NewsContext : DbContext
	{
		public NewsContext()
			: base(nameOrConnectionString: ConnectionStringName)
		{
		}

		public static string ConnectionStringName
		{
			get
			{
				if (ConfigurationManager.ConnectionStrings["NewsDB"] != null)
				{
					return ConfigurationManager.ConnectionStrings["NewsDB"].ToString();
				}

				return "DefaultConnection";
			}
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2").HasPrecision(0));

			modelBuilder.Types().Where(t => t.GetInterfaces().Contains(typeof(IHaveTrackingState))).Configure(x => x.Ignore("TrackingState"));

			modelBuilder.Entity<NewsRssSource>()
				.HasKey(x => x.Id);
		}
	}
}