////namespace SEI.Web.Infrastructure.Identity
////{
////	using System;
////	using System.Collections.Generic;
////	using System.Configuration;
////	using System.Net.Http;
////	using System.Net.Http.Headers;
////	using System.Security.Claims;
////	using System.Threading.Tasks;

////	using AMP.Identity.Contracts;
////	using Common.Extensions;
////	using Common.Helpers;
////	using Microsoft.AspNet.Identity;
////	using Newtonsoft.Json;
////	using Thinktecture.IdentityModel.Clients;
////	using Thinktecture.IdentityModel.Extensions;

////	public class IdentityHelper
////	{
////		private readonly IUserManager _userManager;

////		public IdentityHelper(IUserManager userManager)
////		{
////			this._userManager = userManager;
////		}

////		public async Task<ClaimsIdentity> RequestUserIdentity(string username, string password)
////		{
////			var identityServerAddress = ConfigurationManager.AppSettings["IdentityServerAddress"];
////			var identityServerTokenEndpoint = $"{identityServerAddress}/connect/token";

////			var clientId = ConfigurationManager.AppSettings["ClientId"];
////			var clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

////			var client = new OAuth2Client(new Uri(identityServerTokenEndpoint), clientId, clientSecret);

////			// TODO: remove hardcoded scope. Create separate scope builder
////			var requestAccessTokenResponse = client.RequestAccessTokenUserName(username, password, "openid profile roles offline_access");

////			var claims = new List<Claim>
////			{
////				new Claim("access_token", requestAccessTokenResponse.AccessToken),
////				new Claim("refresh_token", requestAccessTokenResponse.RefreshToken),
////				new Claim("expires_at", (DateTime.UtcNow.ToEpochTime() + requestAccessTokenResponse.ExpiresIn).ToDateTimeFromEpoch().ToString())
////			};

////			var userInfoClaims = await this.RequestUserInfoClaims(requestAccessTokenResponse.AccessToken);
////			claims.AddRange(userInfoClaims);

////			var userInfo = await this._userManager.FindAsync(username, password);
////			claims.Add(new Claim(ClaimTypes.Name, userInfo.FirstName));
////			claims.Add(new Claim(ClaimTypes.Surname, userInfo.LastName));
////			claims.Add(new Claim(ClaimTypes.Sid, userInfo.Id.ToString()));
////			claims.Add(new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()));

////			var result = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

////			return result;
////		}

////		// TODO: Refactor
////		private async Task<List<Claim>> RequestUserInfoClaims(string accessToken)
////		{
////			Throw.If(accessToken.IsNullOrWhitespace(), accessToken);

////			var identityServerAddress = ConfigurationManager.AppSettings["IdentityServerAddress"];
////			var identityServerUserInfoEndpoint = $"{identityServerAddress}/connect/userinfo";

////			var claims = new List<Claim>();
////			using (var httpClient = new HttpClient())
////			{
////				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
				
////				var response = await httpClient.GetAsync(identityServerUserInfoEndpoint);
////				if (response.IsSuccessStatusCode)
////				{
////					var responseString = await response.Content.ReadAsStringAsync();
////					try
////					{
////						var userInfoResponse = JsonConvert.DeserializeObject<IdentityServerUserInfoResponse>(responseString);

////						claims.Add(new Claim(ClaimTypes.Name, userInfoResponse.Preferred_username));

////						if (userInfoResponse.Role != null)
////						{
////							foreach (var role in userInfoResponse.Role)
////							{
////								claims.Add(new Claim(ClaimTypes.Role, role));
////							}
////						}
////					}
////					catch
////					{
////						var userInfoResponse = JsonConvert.DeserializeObject<IdentityServerUserInfoResponseWithOneRole>(responseString);

////						claims.Add(new Claim(ClaimTypes.Name, userInfoResponse.Preferred_username));

////						if (userInfoResponse.Role != null)
////						{
////							claims.Add(new Claim(ClaimTypes.Role, userInfoResponse.Role));
////						}
////					}
////				}
////			}

////			return claims;
////		}

////		private class IdentityServerUserInfoResponse
////		{
////			// ReSharper disable once InconsistentNaming
////			public string Preferred_username { get; set; }

////			// ReSharper disable once InconsistentNaming
////			public List<string> Role { get; set; }
////		}

////		private class IdentityServerUserInfoResponseWithOneRole
////		{
////			// ReSharper disable once InconsistentNaming
////			public string Preferred_username { get; set; }

////			// ReSharper disable once InconsistentNaming
////			public string Role { get; set; }
////		}
////	}
////}