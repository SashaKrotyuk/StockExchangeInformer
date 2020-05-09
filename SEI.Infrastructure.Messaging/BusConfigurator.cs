namespace SEI.Infrastructure.Messaging
{
	using System;

	using MassTransit;
	using MassTransit.RabbitMqTransport;
	using MessageBus;

	public static class BusConfigurator
	{
		public static IBusControl ConfigureRabbitMqBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
		{
			var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
			{
				var host = cfg.Host(
					new Uri(RabbitMqConstants.RabbitMqUri),
					hst =>
					{
						hst.Username(RabbitMqConstants.UserName);
						hst.Password(RabbitMqConstants.Password);
					});

				registrationAction?.Invoke(cfg, host);
			});

			////busControl.ConnectSendObserver(new SendObserver());
			busControl.ConnectPublishObserver(new PublishObserver());
			////busControl.ConnectReceiveObserver(new ReceiveObserver());
			////busControl.ConnectConsumeObserver(new ConsumeObserver());
			////busControl.ConnectConsumeMessageObserver<{T}>(new SpecificConsumeObserver<{T}>());

			return busControl;
		}

		public static IBusControl ConfigureInMemoryBus(Action<IInMemoryBusFactoryConfigurator> configuration)
		{
			return Bus.Factory.CreateUsingInMemory(configuration);
		}
	}
}