namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class SomeMatchedForMapping<T, TResult> : IFilteredMapped<T, TResult>
	{
		private T value;

		public SomeMatchedForMapping(T value)
		{
			this.value = value;
		}

		public IMapped<T, TResult> MapTo(Func<T, TResult> mapping) =>
			new MappingResolved<T, TResult>(mapping(this.value));
	}
}