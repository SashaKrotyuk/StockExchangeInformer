[assembly: Microsoft.Owin.OwinStartup(typeof(SEI.Api.Startup))]

namespace SEI.Api
{
	using System.Web.Http;

	using IdentityServer3.AccessTokenValidation;
	using Owin;
	
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
			{
				Authority = "https://localhost:44392/identity",
				RequiredScopes = new[] { "sampleApi" }
			});

			// web api configuration
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();

			app.UseWebApi(config);
		}
	}
}
