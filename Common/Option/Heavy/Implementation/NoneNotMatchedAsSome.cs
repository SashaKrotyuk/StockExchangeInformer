namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class NoneNotMatchedAsSome<T> : IFiltered<T>
	{
		public IActionable<T> Do(Action<T> action) =>
			new ActionOnNoneNotResolved<T>();

		public IMapped<T, TResult> MapTo<TResult>(Func<T, TResult> mapping) =>
			new MappingOnNoneNotResolved<T, TResult>();
	}
}