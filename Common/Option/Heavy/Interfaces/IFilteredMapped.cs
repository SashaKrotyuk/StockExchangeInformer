namespace Common.Option.Interfaces
{
	using System;

	public interface IFilteredMapped<T, TResult>
	{
		IMapped<T, TResult> MapTo(Func<T, TResult> mapping);
	}
}