namespace Common.Option.Light
{
	using System.Collections;
	using System.Collections.Generic;

	public class Option<T> : IEnumerable<T>
	{
		private Option(IEnumerable<T> content)
		{
			this.Content = content;
		}

		private IEnumerable<T> Content { get; }

		public static Option<T> Some(T value) => new Option<T>(new[]
		{
			value
		});

		public static Option<T> None() => new Option<T>(new T[0]);

		public IEnumerator<T> GetEnumerator() => this.Content.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}
}
