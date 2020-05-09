namespace SEI.Web
{
	using System.Net.Http;
	using System.Web.Http;
	using System.Web.Http.ExceptionHandling;
	using System.Web.Http.Routing;

	using Infrastructure.ExceptionHandling;
	using Newtonsoft.Json.Serialization;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

			config.MapHttpAttributeRoutes();

			config.Services.Add(typeof(IExceptionLogger), new WebApiGlobalExceptionLogger());

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });
			config.Routes.MapHttpRoute(
				 name: "DefaultApiWithAction",
				 routeTemplate: "Api/{controller}/{action}/{id}",
				 defaults: new { id = RouteParameter.Optional });
			config.Routes.MapHttpRoute("DefaultApiGet", "Api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
			config.Routes.MapHttpRoute("DefaultApiPost", "Api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
		}
	}
}