namespace SEI.News.Service.Infrastructure
{
	using System.Linq;
	using System.Security.Claims;

	using Microsoft.AspNet.SignalR;

	public class UserIdProvider : IUserIdProvider
	{
		public string GetUserId(IRequest request)
		{
			var claimsPrincipal = request.User as ClaimsPrincipal;

			if (claimsPrincipal == null)
			{
				return null;
			}

			var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

			if (userIdClaim != null)
			{
				return userIdClaim.Value;
			}

			return null;
		}
	}
}