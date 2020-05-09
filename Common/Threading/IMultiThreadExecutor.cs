namespace Common.Threading
{
	using System;

	public interface IMultiThreadExecutor : IDisposable
	{
		IMultiThreadExecutor AddOperation(MultiThreadExecutorOperation operation);

		IMultiThreadExecutor Stop();

		IMultiThreadExecutor Pause();

		IMultiThreadExecutor Start();

		bool IsStarted();

		bool IsStopped();

		bool IsPaused();

		bool IsWorking();
	}
}