namespace Common.Helpers
{
	using System;
	using System.Configuration;

	public class ConfigurationManagerHelper
	{
		public static string GetConnectionString(string connectionStringName)
		{
			if (ConfigurationManager.ConnectionStrings[connectionStringName] != null)
			{
				return ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
			}

			throw new InvalidOperationException("Please specify the connection string");
		}
	}
}