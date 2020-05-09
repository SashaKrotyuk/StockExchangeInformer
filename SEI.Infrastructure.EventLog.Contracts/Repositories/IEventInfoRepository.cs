namespace SEI.Infrastructure.EventLog.Contracts.Repositories
{
	using SEI.Infrastructure.EventLog.Contracts.Model;

	using System;
	using System.Collections.Generic;

	public interface IEventInfoRepository : IDisposable
	{
		void Add<T>(T message);

		IEnumerable<EventInfo> GetByCorrelationId(Guid correlationId);
	}
}