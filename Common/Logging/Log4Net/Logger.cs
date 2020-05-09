namespace Common.Logging
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Common.Logging.Contracts;
	using log4net.Config;

	public static class Logger
	{
		private static readonly object LockObj = new object();
		private static readonly IDictionary<Type, ILogger> TypeLoggers = new Dictionary<Type, ILogger>();
		private static readonly IDictionary<string, ILogger> NameLoggers = new Dictionary<string, ILogger>();

		private static bool isInitialized;

		public static void Initialize()
		{
			Initialize(null);
		}

		public static void Initialize(string configFile)
		{
			if (!isInitialized)
			{
				if (!string.IsNullOrEmpty(configFile))
				{
					XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
				}
				else
				{
					XmlConfigurator.Configure();
				}

				isInitialized = true;
			}
		}

		public static ILogger Get<T>()
		{
			Type loggerType = typeof(T);
			if (!TypeLoggers.ContainsKey(loggerType))
			{
				lock (LockObj)
				{
					if (!TypeLoggers.ContainsKey(loggerType))
					{
						TypeLoggers[loggerType] = Log4NetLogger.GetInstance(loggerType);
					}
				}
			}

			return TypeLoggers[loggerType];
		}

		public static ILogger Get(string name)
		{
			if (!NameLoggers.ContainsKey(name))
			{
				lock (LockObj)
				{
					if (!NameLoggers.ContainsKey(name))
					{
						NameLoggers[name] = Log4NetLogger.GetInstance(name);
					}
				}
			}

			return NameLoggers[name];
		}
	}
}