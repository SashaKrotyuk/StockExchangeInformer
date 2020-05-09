namespace Common.Extensions
{
	using System;
	using System.Reflection;
	using log4net;
	using log4net.Core;

	public static class LoggerExtenstions
	{
		/// <summary>
		/// Method executes message calculation only when knows that given log level enabled.
		/// </summary>
		/// <param name="log">Log4Net log instance.</param>
		/// <param name="level">Level of logging.</param>
		/// <param name="getMessage">Function that returns log message.</param>
		/// <param name="ex">Exception that should logged.</param>
		public static void Add(this ILog log, Level level, Func<string> getMessage, Exception ex = null)
		{
			if (log.Logger.IsEnabledFor(level))
			{
				var message = getMessage == null ? string.Empty : getMessage();
				log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, level, message, ex);
			}
		}
	}
}