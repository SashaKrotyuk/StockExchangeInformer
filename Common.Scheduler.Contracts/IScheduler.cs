namespace Common.Scheduler.Contracts
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public interface IScheduler : IDisposable
	{
		void Schedule(Expression<Action> job, string correlationId, DateTime scheduledDate);

		Task DeleteScheduledJobs(string correlationId);
	}
}