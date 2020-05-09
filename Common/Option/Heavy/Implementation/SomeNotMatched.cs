namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class SomeNotMatched<T> : IFiltered<T>, IFilteredNoneActionable<T>
	{
		public SomeNotMatched(T value)
		{
			this.Value = value;
		}

		private T Value { get; }

		public IActionable<T> Do(Action<T> action) =>
			new ActionOnSomeNotResolved<T>(this.Value);

		public IActionable<T> Do(Action action) =>
			new ActionOnSomeNotResolved<T>(this.Value);

		public IMapped<T, TResult> MapTo<TResult>(Func<T, TResult> mapping) =>
			new MappingOnSomeNotResolved<T, TResult>(this.Value);
	}
}