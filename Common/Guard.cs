namespace Common
{
	using System;

	public static class Guard
	{
		public static void ForEmptyGuid(Guid value, string parameterName)
		{
			if (value == Guid.Empty)
			{
				throw new ArgumentException(parameterName);
			}
		}

		public static void ForLessEqualZero(int value, string parameterName)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(parameterName);
			}
		}

		public static void ForPrecedesDate(DateTime value, DateTime dateToPrecede, string parameterName)
		{
			if (value >= dateToPrecede)
			{
				throw new ArgumentOutOfRangeException(parameterName);
			}
		}

		public static void ForNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		public static void ForNullOrEmpty(string value, string parameterName)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentOutOfRangeException(parameterName);
			}
		}
	}
}