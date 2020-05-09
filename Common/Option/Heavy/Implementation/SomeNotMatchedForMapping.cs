namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class SomeNotMatchedForMapping<T, TResult> : IFilteredMapped<T, TResult>
	{
		private T value;

		public SomeNotMatchedForMapping(T value)
		{
			this.value = value;
		}

		public IMapped<T, TResult> MapTo(Func<T, TResult> mapping) =>
			new MappingOnSomeNotResolved<T, TResult>(this.value);
	}
}