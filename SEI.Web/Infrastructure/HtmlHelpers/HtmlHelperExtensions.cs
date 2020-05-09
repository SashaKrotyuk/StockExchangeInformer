namespace SEI.Web.Infrastructure.HtmlHelpers
{
	using System.Web.Mvc;

	public static class HtmlHelperExtensions
	{
		public static string ActivePage(this HtmlHelper helper, string controller, string action = "Index", string area = "")
		{
			string classValue = string.Empty;

			string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
			string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();
			string currentArea = helper.ViewContext.RouteData.DataTokens["area"]?.ToString();

			if (currentController == controller && currentAction == action && currentArea == area)
			{
				classValue = "active";
			}

			return classValue;
		}

		public static bool IsDebug(this HtmlHelper htmlHelper)
		{
#if DEBUG
			return true;
#else
			return false;
#endif
		}
	}
}