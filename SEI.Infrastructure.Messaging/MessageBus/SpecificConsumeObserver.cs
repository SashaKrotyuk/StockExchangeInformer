namespace SEI.Infrastructure.Messaging.MessageBus
{
	using System;
	using System.Threading.Tasks;

	using MassTransit;

	public class SpecificConsumeObserver<T> : IConsumeMessageObserver<T> where T : class
	{
		Task IConsumeMessageObserver<T>.PreConsume(ConsumeContext<T> context)
		{
			// called before the consumer's Consume method is called
			return Task.FromResult(0);
		}

		Task IConsumeMessageObserver<T>.PostConsume(ConsumeContext<T> context)
		{
			// called after the consumer's Consume method was called
			// again, exceptions call the Fault method.
			return Task.FromResult(0);
		}

		Task IConsumeMessageObserver<T>.ConsumeFault(ConsumeContext<T> context, Exception exception)
		{
			// called when a consumer throws an exception consuming the message
			return Task.FromResult(0);
		}
	}
}