namespace Common.Scheduler
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	using Common.Abstractions.Repository;
	using Common.Scheduler.Contracts;
	using Hangfire;
	
	public class Scheduler : IScheduler
	{
		private readonly IAsyncGenericRepository _repository;

		public Scheduler(IAsyncGenericRepository genericRepository)
		{
			this._repository = genericRepository;
		}

		public void Schedule(Expression<Action> job, string correlationId, DateTime scheduledDate)
		{
			var jobId = BackgroundJob.Schedule(job, scheduledDate);

			var info = new ScheduledJobInfo
			{
				CorrelationId = correlationId,
				JobId = jobId,
				ExecutionDate = scheduledDate
			};

			this._repository.AddAndSave<ScheduledJobInfo, Guid>(info);
		}

		public async Task DeleteScheduledJobs(string correlationId)
		{
			var jobs = this._repository.Find<ScheduledJobInfo>(x => x.CorrelationId == correlationId);

			foreach (var job in jobs)
			{
				await this._repository.DeleteByIdAndSaveAsync<ScheduledJobInfo, Guid>(job.Id);
				BackgroundJob.Delete(job.JobId);
			}
		}

		public void Dispose()
		{
			this._repository.Dispose();
		}
	}
}