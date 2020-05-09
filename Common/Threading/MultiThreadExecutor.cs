namespace Common.Threading
{
	using System;
	using System.Collections.Concurrent;
	using System.Reflection;
	using System.Threading;

	using log4net;

	public sealed class MultiThreadExecutor : IMultiThreadExecutor
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly int maxThreadsNumber;

		private readonly ConcurrentQueue<MultiThreadExecutorOperation> operationQueue = new ConcurrentQueue<MultiThreadExecutorOperation>();

		private readonly ConcurrentDictionary<Guid, OperationInWorkState> operationsInWork = new ConcurrentDictionary<Guid, OperationInWorkState>();

		private readonly object sync = new object();

		private readonly object operationLoopSync = new object();

		private readonly ManualResetEventSlim simultaneouslyRunningLimit = new ManualResetEventSlim(false);

		private volatile bool stop;

		private volatile bool pause;

		private volatile bool start;

		private volatile bool operationLoop;
		
		private int runningThreads;

		private Thread mainThread;

		private volatile bool disposed;

		public MultiThreadExecutor()
		{
		}

		public MultiThreadExecutor(int maxThreads)
		{
			if (maxThreads > 0)
			{
				this.maxThreadsNumber = maxThreads;
			}
		}

		~MultiThreadExecutor()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			this.Dispose(false);
		}

		public IMultiThreadExecutor AddOperation(MultiThreadExecutorOperation operation)
		{
			this.operationQueue.Enqueue(operation);

			// Signal that operation added in to the queue.
			this.simultaneouslyRunningLimit.Set();

			lock (this.operationLoopSync)
			{
				if (this.operationQueue.Count == 1 && this.start && !this.operationLoop)
				{
					this.Start(true);
				}
			}

			return this;
		}

		public IMultiThreadExecutor Stop()
		{
			if (!this.stop)
			{
				lock (this.sync)
				{
					if (!this.stop)
					{
						this.stop = true;
						this.pause = false;
						this.start = false;

						if (this.mainThread != null && this.mainThread.IsAlive)
						{
							// Notify operation loop to wake up.
							this.simultaneouslyRunningLimit.Set();

							this.CancelOperationsInWork();

							// Wait while operation loop finished.
							this.mainThread.Join();
						}
					}
				}
			}

			return this;
		}

		public IMultiThreadExecutor Pause()
		{
			if (!this.pause)
			{
				lock (this.sync)
				{
					if (!this.pause)
					{
						this.pause = true;

						if (this.mainThread != null && this.mainThread.IsAlive)
						{
							// Notify operation loop to wake up.
							this.simultaneouslyRunningLimit.Set();
						}
					}
				}
			}

			return this;
		}

		public IMultiThreadExecutor Start()
		{
			return this.Start(false);
		}

		public bool IsStarted()
		{
			return this.start;
		}

		public bool IsStopped()
		{
			return this.stop;
		}

		public bool IsPaused()
		{
			return this.pause;
		}

		public void Dispose()
		{
			this.Dispose(true);

			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		public bool IsWorking()
		{
			return (this.runningThreads > 0 || this.operationLoop) || (this.start && !this.pause && this.operationQueue.Count > 0);
		}

		internal void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				// If disposing equals true, dispose all managed and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources here.       
				}

				if (this.mainThread != null && this.mainThread.IsAlive)
				{
					this.Stop();
					this.mainThread = null;
				}
			}

			this.disposed = true;
		}

		private void CancelOperationsInWork()
		{
			// Notify operations which in process rign now that they should cancel their work.
			foreach (var operationInWork in this.operationsInWork)
			{
				// Cancel operation. If not cancel in 1 sec then initiate thread abort.
				operationInWork.Value.Operation.Cancel();
				var workThread = operationInWork.Value.WorkThread;

				if (workThread != null && workThread.IsAlive)
				{
					workThread.Join(50);
				}

				if (!operationInWork.Value.Operation.Done.Wait(1000) && workThread != null && workThread.IsAlive)
				{
					Log.Debug("Operation in " + workThread.ManagedThreadId + " thread didn't respond on \"cancel\" event. Initiating abort...");
					workThread.Abort();
				}
			}
		}

		private int GetPossibleMaxThreads()
		{
			int workerThreads, completionsPortThreads;

			// Find the number of additional worker threads that can be started.
			ThreadPool.GetAvailableThreads(out workerThreads, out completionsPortThreads);
			var maxThreads = this.maxThreadsNumber == 0 ? workerThreads : Math.Min(workerThreads, this.maxThreadsNumber);

			return maxThreads == 0 ? 1 : maxThreads;
		}

		private void OperationsLoop(object state)
		{
			Log.Debug("Started operation loop in thread " + Thread.CurrentThread.ManagedThreadId);

			do
			{
				lock (this.operationLoopSync)
				{
					if (!(this.operationLoop = !this.stop && !this.operationQueue.IsEmpty))
					{
						// Wait before all sub threads are done.
						if (this.runningThreads > 0)
						{
							this.simultaneouslyRunningLimit.Reset();
							this.operationLoop = true;
						}
						else
						{
							break;
						}
					}
				}

				if (this.pause)
				{
					this.simultaneouslyRunningLimit.Reset();
				}

				// Here waiting opration add or available free thread.
				this.simultaneouslyRunningLimit.Wait();

				if (this.stop || this.pause)
				{
					continue;
				}

				var possibleMaxThreads = this.GetPossibleMaxThreads();

				// If system cannot run simultaneously one more thread or queue is empty then sleep on wait.
				if ((this.runningThreads == 0 && possibleMaxThreads <= this.runningThreads) || this.operationQueue.IsEmpty)
				{
					this.simultaneouslyRunningLimit.Reset();
					continue;
				}

				MultiThreadExecutorOperation operation;

				// If can not take operation in the processing then spend time on loop turn :o).
				if (this.operationQueue.TryDequeue(out operation))
				{
					Interlocked.Increment(ref this.runningThreads);
					var operationInWorkState = new OperationInWorkState(operation, null);
					this.operationsInWork.TryAdd(operation.Id, operationInWorkState);
					ThreadPool.QueueUserWorkItem(this.OperationProc, operationInWorkState);
				}
			}
			while (true);

			Log.Debug("Ended operation loop in thread " + Thread.CurrentThread.ManagedThreadId);
		}

		private void OperationProc(object state)
		{
			var operationInWorkState = (OperationInWorkState)state;
			var newOperationInWorkState = new OperationInWorkState(operationInWorkState.Operation, Thread.CurrentThread);
			this.operationsInWork.TryUpdate(operationInWorkState.Operation.Id, newOperationInWorkState, operationInWorkState);

			try
			{
				Log.Debug("Started operation in thread " + Thread.CurrentThread.ManagedThreadId);
				operationInWorkState.Operation.Invoke();
				Log.Debug("Ended operation in thread " + Thread.CurrentThread.ManagedThreadId);
			}
			catch (Exception ex)
			{
				Log.Error("Operation execution error in thread " + Thread.CurrentThread.ManagedThreadId, ex);
				throw;
			}
			finally
			{
				this.operationsInWork.TryRemove(operationInWorkState.Operation.Id, out operationInWorkState);
				Interlocked.Decrement(ref this.runningThreads);
				this.simultaneouslyRunningLimit.Set();
			}
		}

		private IMultiThreadExecutor Start(bool force)
		{
			// Start is valid in the following cases:
			// 1. Not started right now.
			// 2. Started and paused right now.
			// 3. Force start.
			if (force || !this.start || (this.start && this.pause))
			{
				lock (this.sync)
				{
					if (force || !this.start || (this.start && this.pause))
					{
						this.start = true;
						this.stop = false;
						this.pause = false;

						// Set flag that operation loop can start working ASAP.
						this.simultaneouslyRunningLimit.Set();

						if (force || ((this.mainThread == null || !this.mainThread.IsAlive) && this.operationQueue.Count > 0))
						{
							this.mainThread = new Thread(this.OperationsLoop);
							this.mainThread.Start(this);
						}
					}
				}
			}

			return this;
		}

		private sealed class OperationInWorkState
		{
			private readonly Thread workThread;

			private readonly MultiThreadExecutorOperation operation;

			public OperationInWorkState(MultiThreadExecutorOperation operation, Thread workThread)
			{
				this.operation = operation;
				this.workThread = workThread;
			}

			public MultiThreadExecutorOperation Operation => this.operation;

			public Thread WorkThread => this.workThread;
		}
	}
}