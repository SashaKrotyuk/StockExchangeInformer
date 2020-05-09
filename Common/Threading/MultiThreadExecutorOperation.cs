namespace Common.Threading
{
	using System;
	using System.Threading;

	public sealed class MultiThreadExecutorOperation
	{
		private readonly Action<object> operation;

		private readonly object state;

		private readonly ManualResetEventSlim done = new ManualResetEventSlim(false);

		private volatile bool cancel;

		public MultiThreadExecutorOperation(Action<object> operation, object state)
		{
			this.operation = operation;
			this.state = state;
		}

		public event EventHandler CancelEvent;

		public Guid Id { get; } = Guid.NewGuid();

		public ManualResetEventSlim Done => this.done;

		public object State => this.state;

		public void Cancel()
		{
			this.cancel = true;
			var temp = this.CancelEvent;

			temp?.Invoke(this, null);
		}

		public void Invoke()
		{
			try
			{
				if (!this.cancel)
				{
					this.operation(this.state);
				}
			}
			finally
			{
				this.done.Set();
			}
		}
	}
}