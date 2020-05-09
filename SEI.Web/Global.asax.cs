namespace SEI.Web
{
	using System;
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;

	using SEI.Infrastructure.Integration;
	
	public class MvcApplication : HttpApplication
	{
		public void Application_Start(object sender, EventArgs e)
		{
			ContainerConfig.Configure();

			GlobalConfiguration.Configure(WebApiConfig.Register);

			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AutoMapperConfiguration.Configure();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception exception = Server.GetLastError();
#if RELEASE
						Response.Clear();
						Server.ClearError();

						Response.Redirect("error");
#endif
		}

		protected void Application_EndRequest()
		{
			if (Context.Response.StatusCode == 404)
			{
				Response.Clear();

				Server.ClearError();

				Response.Redirect("/404");
			}
		}
	}
}