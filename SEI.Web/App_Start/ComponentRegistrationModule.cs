namespace SEI.Web
{
	using Autofac;
	using Autofac.Integration.Mvc;
	using Autofac.Integration.WebApi;
	using Common.IoC.Autofac;
	using Common.IoC.Common;

	public class ComponentRegistrationModule : AutofacModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(AutofacService.Instance)
				.As<IDependencyResolver>()
				.SingleInstance();

			////builder.RegisterControllers(typeof(MvcApplication).Assembly);
			////builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
		}
	}
}