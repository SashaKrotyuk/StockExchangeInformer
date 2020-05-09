namespace Common.Option.Interfaces
{
	using System;

	public interface IFilteredNoneMapped<T, TResult>
	{
		IMapped<T, TResult> MapTo(Func<TResult> mapping);
	}
}