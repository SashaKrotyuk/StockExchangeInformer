namespace SEI.News.Service.Infrastructure
{
	using System.Configuration;
	using System.Net.Http.Formatting;
	using System.Reflection;
	using System.Web.Http;
	using System.Web.Http.Cors;

	using Autofac;
	using Autofac.Integration.SignalR;
	using Common.IoC.Autofac;
	using IdentityServer3.AccessTokenValidation;
	using Microsoft.AspNet.SignalR;
	using Microsoft.Owin.Cors;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;
	using Owin;

	public static class AppConfiguration
	{
		public static IContainer RegisterIoC()
		{
			AutofacService.Instance.AddAssemblyWithModules(typeof(SEI.News.Service.ComponentRegistrationModule).Assembly);
			AutofacService.Instance.AddAssemblyWithModules(typeof(SEI.News.Integration.ComponentRegistrationModule).Assembly);
			AutofacService.Instance.Initialize();

			var builder = new ContainerBuilder();
			builder.RegisterAssemblyModules(typeof(SEI.News.Service.ComponentRegistrationModule).Assembly);
			builder.RegisterAssemblyModules(typeof(SEI.News.Integration.ComponentRegistrationModule).Assembly);
			builder.RegisterHubs(Assembly.GetExecutingAssembly());

			return builder.Build();
		}

		public static void ConfigureHttp(HttpConfiguration httpConfiguration)
		{
			httpConfiguration.EnableCors(new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE"));
			httpConfiguration.MapHttpAttributeRoutes();
			httpConfiguration.Formatters.Clear();
			httpConfiguration.Formatters.Add(new JsonMediaTypeFormatter());

			var jsonSettings = httpConfiguration.Formatters.JsonFormatter.SerializerSettings;
			jsonSettings.Formatting = Formatting.Indented;
			jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}

		public static void ConfigureSignalR(IAppBuilder appBuilder)
		{
			var container = RegisterIoC();
			GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);

			var settings = new JsonSerializerSettings();
			settings.ContractResolver = new SignalRContractResolver();
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

			var idProvider = new UserIdProvider();
			GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

			appBuilder.UseCors(CorsOptions.AllowAll);

			// var authority = ConfigurationManager.AppSettings["IdentityServerAddress"];
			//
			// appBuilder.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
			// {
			// 	Authority = authority,
			// 	RequiredScopes = new[]
			// 	{
			// 		"openid",
			// 		"profile",
			// 		"roles"
			// 	},
			// 	TokenProvider = new QueryStringOAuthBearerProvider()
			// });

			var hubConfiguration = new HubConfiguration();
			hubConfiguration.EnableDetailedErrors = true;
			appBuilder.MapSignalR(hubConfiguration);
			//GlobalHost.HubPipeline.RequireAuthentication();
		}
	}
}