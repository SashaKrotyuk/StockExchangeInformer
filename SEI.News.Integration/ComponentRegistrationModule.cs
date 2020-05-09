namespace SEI.News.Integration
{
	using Autofac;
	using Common.Abstractions.Repository;
	using Common.DataAccess;
	using Common.DataAccess.DataSources;
	using Common.IoC;
	using Common.IoC.Autofac;
	using SEI.News.Data;
	using SEI.News.Data.Contracts;
	using SEI.News.Data.Repositories;

	public class ComponentRegistrationModule : AutofacModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			this.ConfigureEntityFrameworkDataAccess(builder);
			this.RegisterEntityFrameworkRepositories(builder);
		}

		private void ConfigureEntityFrameworkDataAccess(ContainerBuilder builder)
		{
			builder.RegisterType<NewsContext>()
				.Keyed<NewsContext>(DataSourceType.News);

			//// ***** Register EFDS for each source type *****
			builder.Register((x, y) => new EntityFrameworkDataSource(x.ResolveKeyed<NewsContext>(DataSourceType.News)))
				.Keyed<IDataSource>(DataSourceType.News);

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.News)))
				.Keyed<GenericRepository>(DataSourceType.News);

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.News)))
				.Keyed<IGenericRepository>(DataSourceType.News)
				.As<IGenericRepository>();

			builder.Register((x, y) => new GenericRepository(x.ResolveKeyed<IDataSource>(DataSourceType.News)))
				.Keyed<IAsyncGenericRepository>(DataSourceType.News)
				.As<IAsyncGenericRepository>();
		}

		private void RegisterEntityFrameworkRepositories(ContainerBuilder builder)
		{
			builder.Register((x, y) => new NewsRssSourceRepository(x.ResolveKeyed<IAsyncGenericRepository>(DataSourceType.News)))
				.As<INewsRssSourceRepository>();
		}
	}
}