namespace SEI.Infrastructure.EventLog.Contracts.Model
{
	using System;
	
	using Common.Abstractions.Entities;

	public class EventInfo : IAggregateRoot
	{
		public Guid Id { get; set; }

		public Guid CorrelationId { get; set; }

		public string Description { get; set; }

		public object AdditionalInfo { get; set; }
	}
}