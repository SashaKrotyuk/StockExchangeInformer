namespace SEI.Web
{
	using System.Web.Mvc;
	using Infrastructure.ExceptionHandling;

	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new LogHandleErrorAttribute());
		}
	}
}