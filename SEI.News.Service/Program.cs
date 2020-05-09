namespace SEI.News.Service
{
	using System.Configuration;

	using Common.Logging;
	using Infrastructure;
	using Topshelf;
	using TopShelf.Owin;

	public class Program
	{
		static void Main(string[] args)
		{
			Logger.Initialize();
			HostFactory.Run(serviceConfig =>
			{
				serviceConfig.UseLog4Net("newsServiceLog.config");

				serviceConfig.Service<NewsService>(serviceInstance =>
				{
					serviceInstance.ConstructUsing(() => new NewsService());
					serviceInstance.WhenStarted(
						execute => execute.Start());

					serviceInstance.WhenStopped(
						execute => execute.Stop());

					serviceInstance.WhenPaused(
						execute => execute.Pause());

					serviceInstance.WhenContinued(
						execute => execute.Continue());

					serviceInstance.OwinEndpoint(
					app =>
					{
						var scheme = ConfigurationManager.AppSettings["NewsServiceScheme"];
						var domain = ConfigurationManager.AppSettings["NewsServiceDomain"];
						var port = int.Parse(ConfigurationManager.AppSettings["NewsServicePort"]);

						app.Scheme = scheme;
						app.Domain = domain;
						app.Port = port;
						app.ConfigureHttp(AppConfiguration.ConfigureHttp);
						app.ConfigureAppBuilder(AppConfiguration.ConfigureSignalR);
					});
				});

				serviceConfig.EnableServiceRecovery(recoveryOption =>
				{
					recoveryOption.RestartService(1);
				});

				serviceConfig.EnablePauseAndContinue();

				serviceConfig.SetServiceName("SEI.News");
				serviceConfig.SetDisplayName("SEI News RSS Feeder");
				serviceConfig.SetDescription("SEI service aimed to reed RSS feeds every minute");

				serviceConfig.RunAsNetworkService();
				serviceConfig.StartAutomatically();
			});
		}
	}
}