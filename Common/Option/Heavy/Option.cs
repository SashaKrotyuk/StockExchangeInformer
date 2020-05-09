namespace Common.Option.Heavy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Option.Implementation;
	using Common.Option.Interfaces;

	public class Option<T> : IOption<T>
	{
		private Option(IEnumerable<T> content)
		{
			this.Content = content;
		}

		private IEnumerable<T> Content { get; }

		public static IOption<T> Some(T value) =>
			new Option<T>(new[]
			{
				value
			});

		public static IOption<T> None() =>
			new Option<T>(new T[0]);

		public IFiltered<T> When(Func<T, bool> predicate)
		{
			return
				this.Content
					.Select(item => this.WhenSome(item, predicate))
					.DefaultIfEmpty(new NoneNotMatchedAsSome<T>())
					.Single();
		}
		
		public IFiltered<T> WhenSome()
		{
			return
				this.Content
					.Select<T, IFiltered<T>>(item => new SomeMatched<T>(item))
					.DefaultIfEmpty(new NoneNotMatchedAsSome<T>())
					.Single();
		}

		public IFilteredNone<T> WhenNone()
		{
			return
				this.Content
					.Select<T, IFilteredNone<T>>(item => new SomeNotMatchedAsNone<T>(item))
					.DefaultIfEmpty(new NoneMatched<T>())
					.Single();
		}

		private IFiltered<T> WhenSome(T value, Func<T, bool> predicate) =>
			predicate(value) ? (IFiltered<T>)new SomeMatched<T>(value) : (IFiltered<T>)new SomeNotMatched<T>(value);
	}
}
