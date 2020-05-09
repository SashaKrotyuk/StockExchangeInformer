namespace Common.Tests
{
	using System.Collections.Generic;

	using Autofac;

	using Common.IoC.Autofac;

	public abstract class IntegrationTestBase
	{
		public T Resolve<T>()
		{
			return AutofacService.Instance.Container.Resolve<T>();
		}

		public IEnumerable<T> ResolveEnumerable<T>()
		{
			return AutofacService.Instance.Container.Resolve<IEnumerable<T>>();
		}

		protected void RegisterService<T>(T service) where T : class
		{
			var builder = new ContainerBuilder();

			builder
				.RegisterInstance(service)
				.As<T>()
				.SingleInstance();

#pragma warning disable CS0618 // Type or member is obsolete
            builder.Update(AutofacService.Instance.Container);
#pragma warning restore CS0618 // Type or member is obsolete
        }
	}
}