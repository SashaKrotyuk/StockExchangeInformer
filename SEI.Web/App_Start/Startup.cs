[assembly: Microsoft.Owin.OwinStartup(typeof(SEI.Web.Startup))]

namespace SEI.Web
{
	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens;
	using System.Linq;
	using System.Security.Claims;
	using System.Security.Cryptography.X509Certificates;
	using System.Threading.Tasks;
	using System.Web.Helpers;
	using System.Web.Hosting;
	using System.Web.Routing;

	using IdentityModel.Client;
	using IdentityServer3.Core;
	using IdentityServer3.Core.Configuration;
	using Infrastructure.Identity;
	using Microsoft.IdentityModel.Protocols;
	using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.Cookies;
	using Microsoft.Owin.Security.Google;
	using Microsoft.Owin.Security.OpenIdConnect;
	using Owin;
	
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
			JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

			Common.Logging.Logger.Initialize(HostingEnvironment.ApplicationPhysicalPath + "errorLog.config");
			
			app.MapSignalR();

			app.Map(
				pathMatch: "/identity",
				configuration: idsrvApp =>
				{
					idsrvApp.UseIdentityServer(
						new IdentityServerOptions
						{
							SiteName = "Embedded IdentityServer",
							SigningCertificate = LoadCertificate(),

							Factory = new IdentityServerServiceFactory()
										.UseInMemoryUsers(IdentityUsers.Get())
										.UseInMemoryClients(IdentityClients.Get())
										.UseInMemoryScopes(IdentityScopes.Get()),

							AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
							{
								IdentityProviders = ConfigureIdentityProviders
							}
						});
				});

			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = "Cookies"
			});

			app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
			{
				Authority = "https://localhost:44392/identity",
				ClientId = "mvc",
				Scope = "openid profile roles sampleApi",
				RedirectUri = "https://localhost:44392/",
				ResponseType = "id_token token",

				SignInAsAuthenticationType = "Cookies",
				UseTokenLifetime = false,

				Notifications = new OpenIdConnectAuthenticationNotifications
				{
					SecurityTokenValidated = async n =>
					{
						// create new identity and set name and role claim type
						var nid = new ClaimsIdentity(
							   n.AuthenticationTicket.Identity.AuthenticationType,
							   Constants.ClaimTypes.GivenName,
							   Constants.ClaimTypes.Role);

						// get userinfo data
						var userInfoClient = new UserInfoClient(n.Options.Authority + "/connect/userinfo");

						var userInfo = await userInfoClient.GetAsync(n.ProtocolMessage.AccessToken);
						userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Type, ui.Value)));

						nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

						// add access token for sample API
						nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

						// keep track of access token expiration
						nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

						// add some other app specific claim
						////nid.AddClaim(new Claim("app_specific", "some data"));

						n.AuthenticationTicket = new AuthenticationTicket(
							nid,
							n.AuthenticationTicket.Properties);
					},
					RedirectToIdentityProvider = n =>
					{
						if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
						{
							var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

							if (idTokenHint != null)
							{
								n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
							}
						}

						return Task.FromResult(0);
					}
				}
			});
		}

		private X509Certificate2 LoadCertificate()
		{
			return new X509Certificate2(
				string.Format(@"{0}\bin\Infrastructure\Identity\Certificates\ia.p12", AppDomain.CurrentDomain.BaseDirectory), "0mSuErIPcm4g4U7eDnX09pcam1Gzys8H23Qev0tKCHU");
		}

		private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
		{
			app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
			{
				AuthenticationType = "Google",
				Caption = "Sign-in with Google",
				SignInAsAuthenticationType = signInAsType,

				ClientId = "257179433951-rd0p28cn1c6ers07n20mklktfok6sau7.apps.googleusercontent.com",
				ClientSecret = "Sf52kKgPHZR6wSX7APRaVCBB"
			});
		}
	}
}