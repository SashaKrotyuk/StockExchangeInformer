namespace Common.Option.Interfaces
{
	using System;

	public interface IOption<T>
	{
		IFiltered<T> When(Func<T, bool> predicate);

		IFiltered<T> WhenSome();

		IFilteredNone<T> WhenNone();
	}
}