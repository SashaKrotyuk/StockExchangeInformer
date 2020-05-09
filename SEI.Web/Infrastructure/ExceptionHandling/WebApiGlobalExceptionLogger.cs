namespace SEI.Web.Infrastructure.ExceptionHandling
{
	using System.Net.Http;
	using System.Text;
	using System.Web.Http.ExceptionHandling;

	using Common.Logging;

	public class WebApiGlobalExceptionLogger : ExceptionLogger
	{
		public override void Log(ExceptionLoggerContext context)
		{
			Logger.Get<WebApiGlobalExceptionLogger>().Error(string.Format("Web API call error from: {0}", RequestToString(context.Request)), context.Exception);
		}

		private string RequestToString(HttpRequestMessage request)
		{
			var message = new StringBuilder();
			if (request.Method != null)
			{
				message.Append(request.Method);
			}
			
			if (request.RequestUri != null)
			{
				message.Append(" ").Append(request.RequestUri);
			}	

			return message.ToString();
		}
	}
}