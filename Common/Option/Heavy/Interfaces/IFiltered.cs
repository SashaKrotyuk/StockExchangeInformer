namespace Common.Option.Interfaces
{
	using System;

	public interface IFiltered<T> : IFilteredActionable<T>
	{
		IMapped<T, TResult> MapTo<TResult>(Func<T, TResult> mapping);
	}
}