namespace Common.Scheduler
{
	using System;
	using System.Linq;

	using Abstractions.Repository;
	using Common.IoC.Autofac;
	using Contracts;
	using Hangfire;
	
	public class ScheduledJobsService
	{
		private static BackgroundJobServer _backgroundJobServer;

		public static void StartJob()
		{
			RecurringJob.AddOrUpdate(() => RemoveOutdatedRecords(), Cron.Hourly);
		}

		public static void RemoveOutdatedRecords()
		{
			using (var repository = AutofacService.Instance.Resolve<IAsyncGenericRepository>())
			{
				var outgatedRecordsIds = repository.Find<ScheduledJobInfo>(x => x.ExecutionDate < DateTime.UtcNow).Select(x => x.Id).ToList();
				foreach (var id in outgatedRecordsIds)
				{
					repository.DeleteByIdAndSave<ScheduledJobInfo, Guid>(id);
				}
			}
		}

		public bool Start()
		{
			RegisterIoC();

			GlobalConfiguration.Configuration.UseSqlServerStorage("ScheduledJobsDB");
			GlobalConfiguration.Configuration.UseAutofacActivator(AutofacService.Instance.Container);

			var serverOptions = new BackgroundJobServerOptions
			{
				ServerName = string.Format("Common.ScheduledJobs")
			};
			_backgroundJobServer = new BackgroundJobServer(serverOptions);
			StartJob();

			return true;
		}

		public bool Stop()
		{
			_backgroundJobServer.Dispose();

			return true;
		}

		public bool Pause()
		{
			_backgroundJobServer.Dispose();

			return true;
		}

		public bool Continue()
		{
			var serverOptions = new BackgroundJobServerOptions
			{
				ServerName = string.Format("Common.ScheduledJobs")
			};
			_backgroundJobServer = new BackgroundJobServer(serverOptions);

			return true;
		}

		private static void RegisterIoC()
		{
			AutofacService.Instance.AddAssemblyWithModules(typeof(ComponentRegistrationModule).Assembly);
			AutofacService.Instance.AddAssemblyWithModules(typeof(ServiceComponentRegistrationModule).Assembly);

			AutofacService.Instance.Initialize();
		}
	}
}
