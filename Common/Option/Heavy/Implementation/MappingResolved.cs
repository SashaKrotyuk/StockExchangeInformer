namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class MappingResolved<T, TResult> : IMapped<T, TResult>, IFilteredMapped<T, TResult>, IFilteredNoneMapped<T, TResult>
	{
		public MappingResolved(TResult result)
		{
			this.Result = result;
		}

		private TResult Result { get; }

		public IFilteredMapped<T, TResult> When(Func<T, bool> predicate) => this;

		public IFilteredMapped<T, TResult> WhenSome() => this;

		public IFilteredNoneMapped<T, TResult> WhenNone() => this;

		public IMapped<T, TResult> MapTo(Func<T, TResult> mapping) => this;

		public IMapped<T, TResult> MapTo(Func<TResult> mapping) => this;

		public TResult Map() => this.Result;
	}
}