namespace SEI.Web.Infrastructure.ExceptionHandling
{
	using System;
	using System.Web.Mvc;
	using Common.Logging;

	public class LogHandleErrorAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			Exception ex = filterContext.Exception;

			// Log to elmah
			Elmah.ErrorSignal.FromContext(filterContext.HttpContext.ApplicationInstance.Context).Raise(ex);
			Logger.Get<LogHandleErrorAttribute>().Error("Unhandled exception:", ex);

			filterContext.ExceptionHandled = true;

			// if the request is AJAX return JSON else view.
			if (IsAjax(filterContext))
			{
				filterContext.HttpContext.Response.Clear();

				// Because its a exception raised after ajax invocation
				// Lets return Json
				filterContext.Result = new JsonResult()
				{
					Data = filterContext.Exception.Message,
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}
			else
			{
				filterContext.HttpContext.Response.Redirect("/error");
			}
		}

		private bool IsAjax(ExceptionContext filterContext)
		{
			return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}
	}
}