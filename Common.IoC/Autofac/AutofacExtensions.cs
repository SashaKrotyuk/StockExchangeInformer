﻿namespace Common.IoC.Autofac
{
	using global::Autofac;

	public static class AutofacExtensions
	{
		public static void Register<TInterface, TClass>(this ContainerBuilder builder) where TClass : TInterface
		{
			builder.RegisterType<TClass>()
				   .As<TInterface>()
				   .InstancePerLifetimeScope();
		}
	}
}