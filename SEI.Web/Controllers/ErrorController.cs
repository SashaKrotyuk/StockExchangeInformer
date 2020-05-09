namespace SEI.Web.Controllers
{
	using System.Web.Mvc;

	public class ErrorController : Controller
	{
		// GET: Exception
		[Route("error")]
		[Route("404")]
		public ActionResult Index()
		{
			return File("~/Scripts/dist/index.html", "text/html");
		}
	}
}