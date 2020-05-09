namespace SEI.Infrastructure.Messaging.MessageBus
{
	using System;
	using System.Threading.Tasks;

	using MassTransit;

	public class SendObserver : ISendObserver
	{
		public Task PreSend<T>(SendContext<T> context)
			where T : class
		{
			// called just before a message is sent, all the headers should be setup and everything
			return Task.FromResult(0);
		}

		public Task PostSend<T>(SendContext<T> context)
			where T : class
		{
			// called just after a message it sent to the transport and acknowledged (RabbitMQ)
			return Task.FromResult(0);
		}

		public Task SendFault<T>(SendContext<T> context, Exception exception)
			where T : class
		{
			// called if an exception occurred sending the message
			return Task.FromResult(0);
		}
	}
}