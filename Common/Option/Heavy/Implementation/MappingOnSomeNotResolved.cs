namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class MappingOnSomeNotResolved<T, TResult> : IMapped<T, TResult>
	{
		public MappingOnSomeNotResolved(T value)
		{
			this.Value = value;
		}

		private T Value { get; }

		public IFilteredMapped<T, TResult> When(Func<T, bool> predicate) =>
			predicate(this.Value)
				? (IFilteredMapped<T, TResult>)new SomeMatchedForMapping<T, TResult>(this.Value)
				: new SomeNotMatchedForMapping<T, TResult>(this.Value);

		public IFilteredMapped<T, TResult> WhenSome() =>
			new SomeMatchedForMapping<T, TResult>(this.Value);

		public IFilteredNoneMapped<T, TResult> WhenNone() =>
			new SomeNotMatchedAsNoneForMapping<T, TResult>(this.Value);

		public TResult Map()
		{
			throw new InvalidOperationException();
		}
	}
}