namespace Common.Abstractions
{
	using System;

	public interface IEvent
	{
		Guid CorrelationId { get; }

		DateTime DateTimeEventOccurred { get; }
	}
}