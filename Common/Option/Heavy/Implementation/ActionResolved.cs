namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class ActionResolved<T> : IActionable<T>, IFilteredActionable<T>, IFilteredNoneActionable<T>
	{
		public ActionResolved(Action action)
		{
			this.Action = action;
		}

		private Action Action { get; }

		public IFilteredActionable<T> When(Func<T, bool> predicate) => this;

		public IFilteredActionable<T> WhenSome() => this;

		public IFilteredNoneActionable<T> WhenNone() => this;

		public IActionable<T> Do(Action<T> action) => this;

		public IActionable<T> Do(Action action) => this;

		public void Execute() => this.Action();
	}
}