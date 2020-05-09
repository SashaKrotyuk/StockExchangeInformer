namespace SEI.Infrastructure.EventLog.Integration
{
	using Autofac;
	using Common.Abstractions.Repository;
	using Common.DataAccess;
	using Common.DataAccess.DataSources;
	using Common.IoC;
	using Common.IoC.Autofac;
	using MongoDB.Driver;
	using SEI.Infrastructure.EventLog.Contracts.Repositories;
	using SEI.Infrastructure.EventLog.Data;
	using SEI.Infrastructure.EventLog.Data.Repositories;

	public class ComponentRegistrationModule : AutofacModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			this.ConfigureMongoDbDataAccess(builder);
			this.RegisterMongoDbRepositories(builder);
		}

		private void ConfigureMongoDbDataAccess(ContainerBuilder builder)
		{
			builder.Register<IMongoDatabase>(x => EventLogContext.GetDatabase())
				.Keyed<IMongoDatabase>(DataSourceType.InfrastructureMongo)
				.SingleInstance();

			builder.Register((x, y) => new MongoDbDataSource(x.ResolveKeyed<IMongoDatabase>(DataSourceType.InfrastructureMongo)))
				.Keyed<IDataSource>(DataSourceType.InfrastructureMongo);

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.InfrastructureMongo)))
				.Keyed<GenericRepository>(DataSourceType.InfrastructureMongo);

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.InfrastructureMongo)))
				.Keyed<IGenericRepository>(DataSourceType.InfrastructureMongo)
				.As<IGenericRepository>();

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.InfrastructureMongo)))
				.Keyed<IAsyncGenericRepository>(DataSourceType.InfrastructureMongo)
				.As<IAsyncGenericRepository>();
		}

		private void RegisterMongoDbRepositories(ContainerBuilder builder)
		{
			builder.Register((x, y) => new EventInfoRepository(x.ResolveKeyed<IAsyncGenericRepository>(DataSourceType.InfrastructureMongo)))
				.As<IEventInfoRepository>();
		}
	}
}