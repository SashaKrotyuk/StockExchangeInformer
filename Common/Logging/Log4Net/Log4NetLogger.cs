namespace Common.Logging
{
	using System;

	using Common.Logging.Contracts;
	using log4net;

	public class Log4NetLogger : ILogger
	{
		private ILog log;

		private Log4NetLogger()
		{
		}

		private Log4NetLogger(string name)
		{
			this.log = LogManager.GetLogger(name);
		}

		private Log4NetLogger(Type type)
		{
			this.log = LogManager.GetLogger(type);
		}

		public static ILogger GetInstance(string name)
		{
			return new Log4NetLogger(name);
		}

		public static ILogger GetInstance(Type type)
		{
			return new Log4NetLogger(type);
		}

		public void Error(object message, Exception exception = null)
		{
			if (this.log.IsErrorEnabled)
			{
				if (exception == null)
				{
					this.log.Error(message);
				}
				else
				{
					this.log.Error(message, exception);
				}
			}
		}

		public void ErrorFormat(string format, params object[] arguments)
		{
			if (this.log.IsErrorEnabled)
			{
				this.log.ErrorFormat(format, arguments);
			}
		}

		public void Debug(object message, Exception exception = null)
		{
			if (this.log.IsDebugEnabled)
			{
				if (exception == null)
				{
					this.log.Debug(message);
				}
				else
				{
					this.log.Debug(message, exception);
				}
			}
		}

		public void DebugFormat(string format, params object[] arguments)
		{
			if (this.log.IsDebugEnabled)
			{
				this.log.DebugFormat(format, arguments);
			}
		}

		public void Info(object message, Exception exception = null)
		{
			if (this.log.IsInfoEnabled)
			{
				if (exception == null)
				{
					this.log.Info(message);
				}
				else
				{
					this.log.Info(message, exception);
				}
			}
		}

		public void InfoFormat(string format, params object[] arguments)
		{
			if (this.log.IsInfoEnabled)
			{
				this.log.InfoFormat(format, arguments);
			}
		}
	}
}
