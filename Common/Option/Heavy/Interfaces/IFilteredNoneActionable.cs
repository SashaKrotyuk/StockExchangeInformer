namespace Common.Option.Interfaces
{
	using System;

	public interface IFilteredNoneActionable<T>
	{
		IActionable<T> Do(Action action);
	}
}