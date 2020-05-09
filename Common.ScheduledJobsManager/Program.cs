namespace Common.Scheduler
{
	using Logging;
	using Topshelf;

	public class Program
	{
		public static void Main(string[] args)
		{
			Logger.Initialize();
			HostFactory.Run(serviceConfig =>
			{
				////serviceConfig.UseLog4Net("log4net.config");

				serviceConfig.Service<ScheduledJobsService>(serviceInstance =>
				{
					serviceInstance.ConstructUsing(() => new ScheduledJobsService());
					serviceInstance.WhenStarted(
						execute => execute.Start());

					serviceInstance.WhenStopped(
						execute => execute.Stop());

					serviceInstance.WhenPaused(
						execute => execute.Pause());

					serviceInstance.WhenContinued(
						execute => execute.Continue());
				});

				serviceConfig.EnableServiceRecovery(recoveryOption =>
				{
					recoveryOption.RestartService(1);
				});

				serviceConfig.EnablePauseAndContinue();

				serviceConfig.SetServiceName("AMP.ScheduledJobsService");
				serviceConfig.SetDisplayName("AMP.ScheduledJobsService");
				serviceConfig.SetDescription("AMP service aimed to invalidate scheduled jobs every hour");

				serviceConfig.StartAutomatically();
			});
		}
	}
}