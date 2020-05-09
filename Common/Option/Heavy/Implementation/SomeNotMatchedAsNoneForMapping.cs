namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class SomeNotMatchedAsNoneForMapping<T, TResult> : IFilteredNoneMapped<T, TResult>
	{
		public SomeNotMatchedAsNoneForMapping(T value)
		{
			this.Value = value;
		}

		private T Value { get; }

		public IMapped<T, TResult> MapTo(Func<TResult> mapping) =>
			new MappingOnSomeNotResolved<T, TResult>(this.Value);
	}
}