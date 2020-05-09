namespace SEI.Web
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;

	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapMvcAttributeRoutes();
			routes.MapRoute(
				name: "StaticJsFiles",
				url: "{fileName}.js",
				defaults: new { controller = "StaticFiles", action = "Js" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.MapRoute(
				name: "StaticCssFiles",
				url: "{fileName}.css",
				defaults: new { controller = "StaticFiles", action = "Css" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.MapRoute(
				name: "StaticWoff2Files",
				url: "{fileName}.woff2",
				defaults: new { controller = "StaticFiles", action = "Woff2" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.MapRoute(
				name: "StaticWoffFiles",
				url: "{fileName}.woff",
				defaults: new { controller = "StaticFiles", action = "Woff" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.MapRoute(
				name: "StaticTtfFiles",
				url: "{fileName}.ttf",
				defaults: new { controller = "StaticFiles", action = "Ttf" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.MapRoute(
				name: "Manifest",
				url: "assets/{fileName}.json",
				defaults: new { controller = "StaticFiles", action = "Json" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.MapRoute(
				name: "StaticJsonFiles",
				url: "assets/i18n/{fileName}.json",
				defaults: new { controller = "StaticFiles", action = "Json" },
				namespaces: new[] { "SEI.Web.Controllers" });

			routes.Add(
				name: "Default",
				item: new Route(
						url: "{controller}/{action}/{id}",
						defaults: new RouteValueDictionary(new { controller = "Home", action = "Index", id = UrlParameter.Optional }),
						constraints: null,
						dataTokens: new RouteValueDictionary(
							new
							{
								Namespaces = new string[] { "SEI.Web.Controllers" },
								UseNamespaceFallback = false
							}),
				routeHandler: new MvcRouteHandler()));
		}
	}
}