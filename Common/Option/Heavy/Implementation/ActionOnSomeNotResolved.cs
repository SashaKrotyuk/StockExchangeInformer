namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class ActionOnSomeNotResolved<T> : IActionable<T>
	{
		public ActionOnSomeNotResolved(T value)
		{
			this.Value = value;
		}

		private T Value { get; }

		public IFilteredActionable<T> When(Func<T, bool> predicate) =>
			predicate(this.Value) ? (IFilteredActionable<T>)new SomeMatched<T>(this.Value) : new SomeNotMatched<T>(this.Value);

		public IFilteredActionable<T> WhenSome() =>
			new SomeMatched<T>(this.Value);

		public IFilteredNoneActionable<T> WhenNone() =>
			new SomeNotMatched<T>(this.Value);

		public void Execute()
		{
		}
	}
}