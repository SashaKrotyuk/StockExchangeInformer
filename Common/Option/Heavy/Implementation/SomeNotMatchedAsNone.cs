namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class SomeNotMatchedAsNone<T> : IFilteredNone<T>
	{
		public SomeNotMatchedAsNone(T value)
		{
			this.Value = value;
		}

		private T Value { get; }

		public IActionable<T> Do(Action action) =>
			new ActionOnSomeNotResolved<T>(this.Value);

		public IMapped<T, TResult> MapTo<TResult>(Func<TResult> mapping) =>
			new MappingOnSomeNotResolved<T, TResult>(this.Value);
	}
}