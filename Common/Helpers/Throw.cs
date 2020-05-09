namespace Common.Helpers
{
	using System;

	public static class Throw
	{
		/// <summary>
		/// Throws an <see cref="ArgumentNullException"/> if the given value is null
		/// </summary>
		public static void IfNull<T>(T value, string parameterName)
		{
			Throw<ArgumentNullException>.If(value == null, parameterName);
		}

		/// <summary>
		/// Throws an <see cref="ArgumentException"/> if the given condition is true
		/// </summary>
		public static void If(bool condition, string parameterName)
		{
			Throw<ArgumentException>.If(condition, parameterName);
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException"/> if the given value is outside of the specified range
		/// </summary>
		public static void IfOutOfRange<T>(T value, string paramName, T? min = null, T? max = null)
			where T : struct, IComparable<T>
		{
			if (min.HasValue && value.CompareTo(min.Value) < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, $"Expected: >= {min}, but was {value}");
			}

			if (max.HasValue && value.CompareTo(max.Value) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, $"Expected: <= {max}, but was {value}");
			}
		}
	}
}