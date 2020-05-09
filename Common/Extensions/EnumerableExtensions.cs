namespace Common.Extensions
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;

	public static class EnumerableExtensions
	{
		public static T SafeFirstOrDefault<T>(this IEnumerable<T> enumerator)
			where T : class, new()
		{
			if (enumerator == null)
			{
				return new T();
			}

			return enumerator.FirstOrDefault() ?? new T();
		}

		public static T SafeFirstOrDefault<T>(this IEnumerable<T> enumerator, Func<T, bool> predicate)
			where T : class, new()
		{
			if (enumerator == null)
			{
				return new T();
			}

			return enumerator.FirstOrDefault(predicate) ?? new T();
		}

		public static IList<TResult> CreateArray<TSource, TResult>(this ICollection<TSource> list, Func<TSource, TResult> selector, bool excludeNullItems = false)
		{
			return list?.Select(selector).Where(x => !excludeNullItems || !Equals(x, null)).ToArray();
		}

		public static T WithMinimum<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion)
			where T : class
			where TKey : IComparable<TKey>
		{
			return sequence.Select(obj => Tuple.Create(obj, criterion(obj)))
				.Aggregate(
					(Tuple<T, TKey>)null,
					(best, cur) => best == null || cur.Item2.CompareTo(best.Item2) < 0 ? cur : best)
				.Item1;
		}

		public static void Do<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (T obj in sequence)
			{
				action(obj);
			}
		}

		public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
		{
			foreach (var element in toAdd)
			{
				@this.Add(element);
			}
		}

		public static void ForEach<TElement>(this IEnumerable<TElement> collection, Action<TElement> action)
		{
			if (collection == null)
			{
				return;
			}

			foreach (var item in collection)
			{
				action(item);
			}
		}
	}
}