namespace Common.Scheduler
{
	using Autofac;
	using Common.IoC.Autofac;
	using MassTransit;
	using SEI.Infrastructure.Messaging;

	public class ServiceComponentRegistrationModule : AutofacModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(
				context =>
				{
					var busControl = BusConfigurator.ConfigureRabbitMqBus(
						(cfg, host) =>
						{
							cfg.ReceiveEndpoint(
								host,
								////RabbitMqConstants.FlagQueue,
								e =>
								{
								});
						});

					busControl.Start();
					return busControl;
				})
			.SingleInstance()
			.As<IBusControl>()
			.As<IBus>();
		}
	}
}