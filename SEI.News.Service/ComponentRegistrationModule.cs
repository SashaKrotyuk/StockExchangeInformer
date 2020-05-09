namespace SEI.News.Service
{
	using Autofac;
	using Common.IoC.Autofac;
	using Common.IoC.Common;
	using Topshelf.Logging;

	public class ComponentRegistrationModule : AutofacModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(AutofacService.Instance)
				.As<IDependencyResolver>()
				.SingleInstance();

			builder.RegisterInstance(HostLogger.Get<NewsService>())
				.As<LogWriter>()
				.SingleInstance();
		}
	}
}