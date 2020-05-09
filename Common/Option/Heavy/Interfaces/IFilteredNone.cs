namespace Common.Option.Interfaces
{
	using System;

	public interface IFilteredNone<T>
	{
		IActionable<T> Do(Action action);

		IMapped<T, TResult> MapTo<TResult>(Func<TResult> mapping);
	}
}