namespace Common.Logging.Contracts
{
	using System;

	public interface ILogger
	{
		void Debug(object message, Exception exception = null);

		void DebugFormat(string format, params object[] arguments);

		void Error(object message, Exception exception = null);

		void ErrorFormat(string format, params object[] arguments);

		void Info(object message, Exception exception = null);

		void InfoFormat(string format, params object[] arguments);
	}
}