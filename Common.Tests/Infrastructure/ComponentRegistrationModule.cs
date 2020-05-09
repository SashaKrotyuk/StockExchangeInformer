namespace Common.Tests.Infrastructure
{
	using Autofac;
	using Autofac.Core;

	using Common.IoC.Autofac;
	using Common.Threading;

	public class ComponentRegistrationModule : AutofacModule
	{
		protected override void Load(Autofac.ContainerBuilder builder)
		{
			// Executor marked static because it collect in queue operations to run and immediately returns control back.
			// As system resources become available executor gets accumulated operation from queue in work.
			builder.RegisterType<MultiThreadExecutor>()
				.As<IMultiThreadExecutor>()
				.SingleInstance();
		}

		protected override void AttachToComponentRegistration(
			IComponentRegistry componentRegistry,
			IComponentRegistration registration)
		{
		}
	}
}