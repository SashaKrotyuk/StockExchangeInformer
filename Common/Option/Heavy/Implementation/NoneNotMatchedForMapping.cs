namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class NoneNotMatchedForMapping<T, TResult> : IFilteredNoneMapped<T, TResult>, IFilteredMapped<T, TResult>
	{
		public IMapped<T, TResult> MapTo(Func<TResult> mapping) =>
			new MappingOnNoneNotResolved<T, TResult>();

		public IMapped<T, TResult> MapTo(Func<T, TResult> mapping) =>
			new MappingOnNoneNotResolved<T, TResult>();
	}
}