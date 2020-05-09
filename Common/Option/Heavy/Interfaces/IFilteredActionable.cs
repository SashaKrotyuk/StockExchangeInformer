namespace Common.Option.Interfaces
{
	using System;

	public interface IFilteredActionable<T>
	{
		IActionable<T> Do(Action<T> action);
	}
}