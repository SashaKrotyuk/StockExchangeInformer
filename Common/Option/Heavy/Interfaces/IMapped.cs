namespace Common.Option.Interfaces
{
	using System;

	public interface IMapped<T, TResult>
	{
		IFilteredMapped<T, TResult> When(Func<T, bool> predicate);

		IFilteredMapped<T, TResult> WhenSome();

		IFilteredNoneMapped<T, TResult> WhenNone();

		TResult Map();
	}
}