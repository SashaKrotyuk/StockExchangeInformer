namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class SomeMatched<T> : IFiltered<T>
	{
		public SomeMatched(T value)
		{
			this.Value = value;
		}

		private T Value { get; }

		public IActionable<T> Do(Action<T> action) =>
			new ActionResolved<T>(() => action(this.Value));

		public IMapped<T, TResult> MapTo<TResult>(Func<T, TResult> mapping) =>
			new MappingResolved<T, TResult>(mapping(this.Value));
	}
}