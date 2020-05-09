namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class NoneMatchedForMapping<T, TResult> : IFilteredNoneMapped<T, TResult>
	{
		public IMapped<T, TResult> MapTo(Func<TResult> mapping) =>
			new MappingResolved<T, TResult>(mapping());
	}
}