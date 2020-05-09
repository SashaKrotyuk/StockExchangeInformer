namespace Common.Scheduler.Contracts
{
	using System;

	using Abstractions;
	using Abstractions.Entities;

	public class ScheduledJobInfo : Entity<Guid>, IAggregateRoot
	{
		public string JobId { get; set; }

		public string CorrelationId { get; set; }

		public DateTime ExecutionDate { get; set; }
	}
}