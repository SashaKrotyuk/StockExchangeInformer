namespace SEI.Infrastructure.Messaging.MessageBus
{
	using System;
	using System.Threading.Tasks;

	using MassTransit;

	public class ConsumeObserver : IConsumeObserver
	{
		Task IConsumeObserver.PreConsume<T>(ConsumeContext<T> context)
		{
			// called before the consumer's Consume method is called
			return Task.FromResult(0);
		}

		Task IConsumeObserver.PostConsume<T>(ConsumeContext<T> context)
		{
			// called after the consumer's Consume method is called
			// if an exception was thrown, the ConsumeFault method is called instead
			return Task.FromResult(0);
		}

		Task IConsumeObserver.ConsumeFault<T>(ConsumeContext<T> context, Exception exception)
		{
			// called if the consumer's Consume method throws an exception
			return Task.FromResult(0);
		}
	}
}
