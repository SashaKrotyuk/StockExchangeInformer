namespace Common
{
	using System;
	using System.Globalization;

	public static class DateTimeExtensions
	{
		private static readonly DateTime JavaScriptZero = new DateTime(1970, 1, 1);

		public static string ToJsTime(this DateTime timestamp)
		{
			var timePassed = timestamp - JavaScriptZero;
			var totalMilliseconds = (long)timePassed.TotalMilliseconds;
			return totalMilliseconds.ToString(CultureInfo.InvariantCulture);
		}

		public static double ConvertToUnixTimestamp(this DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan diff = date - origin;
			return Math.Floor(diff.TotalSeconds);
		}

		public static DateTime GetFirstDayOfWeek(this DateTime date)
		{
			// ??? get last day of week (Saturday or Sunday) from culture info
			// Sunday = 0, Saturday = 6
			int daysAhead = (int)DayOfWeek.Sunday - (int)date.DayOfWeek;
			return date.Date.AddDays(daysAhead);
		}

		public static DateTime GetLastDayOfWeek(this DateTime date)
		{
			// Sunday = 0, Saturday = 6
			int daysAhead = (int)DayOfWeek.Saturday - (int)date.DayOfWeek;
			return date.Date.AddDays(daysAhead);
		}
	}
}