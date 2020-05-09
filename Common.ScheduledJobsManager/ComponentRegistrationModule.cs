namespace Common.Scheduler
{
	using System.Data.Entity;

	using Autofac;
	using Common.Abstractions.Repository;
	using Common.DataAccess;
	using Common.DataAccess.DataSources;
	using Common.IoC;
	using Common.IoC.Autofac;
	using Common.IoC.Common;
	using Common.Scheduler.Contracts;
	using Common.Scheduler.Data.Migrations;
	
	using Topshelf.Logging;

	public class ComponentRegistrationModule : AutofacModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(AutofacService.Instance)
				.As<IDependencyResolver>()
				.SingleInstance();

			builder.RegisterType<ScheduledJobsContext>()
				.Keyed<ScheduledJobsContext>(DataSourceType.CoreScheduledJobs);

			builder.RegisterType<ScheduledJobsContext>()
				.As<DbContext>()
				.Keyed<DbContext>(DataSourceType.CoreScheduledJobs)
				.InstancePerLifetimeScope();

			builder.Register((x, y) => new EntityFrameworkDataSource(x.ResolveKeyed<ScheduledJobsContext>(DataSourceType.CoreScheduledJobs)))
				.Keyed<IDataSource>(DataSourceType.CoreScheduledJobs);

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.CoreScheduledJobs)))
				.Keyed<GenericRepository>(DataSourceType.CoreScheduledJobs);

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.CoreScheduledJobs)))
				.Keyed<IGenericRepository>(DataSourceType.CoreScheduledJobs)
				.As<IGenericRepository>();

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.CoreScheduledJobs)))
				.Keyed<IAsyncGenericRepository>(DataSourceType.CoreScheduledJobs)
				.As<IAsyncGenericRepository>();

			builder.Register((x, y) => new Scheduler(x.ResolveKeyed<IAsyncGenericRepository>(DataSourceType.CoreScheduledJobs)))
				.As<IScheduler>();

			builder.RegisterInstance(HostLogger.Get<ScheduledJobsService>())
				.As<LogWriter>()
				.SingleInstance();
		}
	}
}