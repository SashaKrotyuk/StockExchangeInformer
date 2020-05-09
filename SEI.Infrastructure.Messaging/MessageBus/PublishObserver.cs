namespace SEI.Infrastructure.Messaging.MessageBus
{
	using System;
	using System.Threading.Tasks;
	using Common.IoC.Autofac;
	using MassTransit;
	using SEI.Infrastructure.EventLog.Contracts.Repositories;

	public class PublishObserver : IPublishObserver
	{
		public Task PrePublish<T>(PublishContext<T> context)
			where T : class
		{
			// called right before the message is published (sent to exchange or topic)
			return Task.FromResult(0);
		}

		public Task PostPublish<T>(PublishContext<T> context)
			where T : class
		{
			using (var eventInfoRepo = AutofacService.Instance.Resolve<IEventInfoRepository>())
			{
				eventInfoRepo.Add(context.Message);
			}

			// called after the message is published (and acked by the broker if RabbitMQ)
			return Task.FromResult(0);
		}

		public Task PublishFault<T>(PublishContext<T> context, Exception exception)
			where T : class
		{
			// called if there was an exception publishing the message
			return Task.FromResult(0);
		}
	}
}
