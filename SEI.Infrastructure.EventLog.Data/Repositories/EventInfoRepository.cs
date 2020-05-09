namespace SEI.Infrastructure.EventLog.Data.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Common.Abstractions;
	using Common.Abstractions.Repository;
	using SEI.Infrastructure.EventLog.Contracts.Model;
	using SEI.Infrastructure.EventLog.Contracts.Repositories;

	public class EventInfoRepository : IEventInfoRepository
	{
		private readonly IAsyncGenericRepository _repository;

		public EventInfoRepository(IAsyncGenericRepository genericRepository)
		{
			this._repository = genericRepository;
		}

		public void Add<T>(T message)
		{
			IEvent @event = message as IEvent;
			if (@event != null)
			{
				var info = new EventInfo
				{
					Id = Guid.NewGuid(),
					CorrelationId = @event.CorrelationId,
					AdditionalInfo = message,
					Description = @event.ToString()
				};

				this._repository.AddAndSave<EventInfo, Guid>(info);
			}
		}
		
		public IEnumerable<EventInfo> GetByCorrelationId(Guid correlationId)
		{
			return this._repository.Find<EventInfo>(x => x.CorrelationId == correlationId).ToList();
		}

		public void Dispose()
		{
			this._repository.Dispose();
		}
	}
}