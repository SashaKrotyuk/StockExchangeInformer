namespace SEI.Web.Controllers
{
	using System.Threading.Tasks;
	using System.Web;
	using System.Web.Mvc;
	using IdentityModel.Client;

	public class AccountController : BaseController
	{
		public ActionResult Logout()
		{
			Request.GetOwinContext().Authentication.SignOut();
			return Redirect("/");
		}

		private async Task<TokenResponse> GetTokenAsync()
		{
			var client = new TokenClient(
				"https://localhost:44392/identity/connect/token",
				"mvc_service",
				"secret");

			return await client.RequestClientCredentialsAsync("sampleApi");
		}
	}
}