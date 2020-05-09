namespace SEI.News.Service
{
	using System.Collections.Generic;

	using Common.IoC.Autofac;
	using Hangfire;
	using MassTransit;
	using SEI.News.Data.Contracts;
	using SEI.News.Model;
	using SEI.News.Service.Services;
	using Microsoft.AspNet.SignalR.Hubs;
	using Microsoft.AspNet.SignalR;
	using Hubs;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	public class NewsService
	{
		private static BackgroundJobServer _backgroundJobServer;

		public static IBusControl Bus { get; set; }

		public bool Start()
		{
			////Bus = ServiceBusConfig.ConfigureServiceBus();
			Bus?.Start();

			GlobalConfiguration.Configuration.UseSqlServerStorage("NewsDB");

			_backgroundJobServer = new BackgroundJobServer();
			StartJob();

			return true;
		}

		public bool Stop()
		{
			Bus?.Stop();

			_backgroundJobServer.Dispose();

			return true;
		}

		public bool Pause()
		{
			Bus?.Stop();

			_backgroundJobServer.Dispose();

			return true;
		}

		public bool Continue()
		{
			Bus?.Start();

			_backgroundJobServer = new BackgroundJobServer();

			return true;
		}

		public static void StartJob()
		{
			RecurringJob.AddOrUpdate(
					() => ReadRss(),
					Cron.Minutely);
		}

		public static void ReadRss()
		{
			using (var newsRssSourcesRepo = AutofacService.Instance.Resolve<INewsRssSourceRepository>())
			{
				var sources = newsRssSourcesRepo.GetAll();
				var rssReader = new RssReader();
				rssReader.NewRssItemsEvent += RssReaderOnNewRssItemsEvent;
				rssReader.Read(sources, null, null);
			}
		}

		private static void RssReaderOnNewRssItemsEvent(List<NewsItem> rssItems)
		{
			IHubConnectionContext<dynamic> _clients = GlobalHost.ConnectionManager.GetHubContext<NewsHub>().Clients;
			var _camelCaseFormatter = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			_clients.All.onLatestNews(JsonConvert.SerializeObject(rssItems, _camelCaseFormatter));
		}
	}
}