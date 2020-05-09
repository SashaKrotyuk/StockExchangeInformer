namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class MappingOnNoneNotResolved<T, TResult> : IMapped<T, TResult>
	{
		public IFilteredMapped<T, TResult> When(Func<T, bool> predicate) =>
			new NoneNotMatchedForMapping<T, TResult>();

		public IFilteredMapped<T, TResult> WhenSome() =>
			new NoneNotMatchedForMapping<T, TResult>();

		public IFilteredNoneMapped<T, TResult> WhenNone() =>
			new NoneMatchedForMapping<T, TResult>();

		public TResult Map()
		{
			throw new InvalidOperationException();
		}
	}
}