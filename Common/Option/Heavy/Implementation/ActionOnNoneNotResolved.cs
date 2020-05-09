namespace Common.Option.Implementation
{
	using System;
	using Common.Option.Interfaces;

	internal class ActionOnNoneNotResolved<T> : IActionable<T>
	{
		public IFilteredActionable<T> When(Func<T, bool> predicate) =>
			new NoneNotMatchedAsSome<T>();

		public IFilteredActionable<T> WhenSome() =>
			new NoneNotMatchedAsSome<T>();

		public IFilteredNoneActionable<T> WhenNone() =>
			new NoneMatched<T>();

		public void Execute()
		{
		}
	}
}