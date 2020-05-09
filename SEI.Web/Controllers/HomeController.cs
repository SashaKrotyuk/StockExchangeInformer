namespace SEI.Web.Controllers
{
	using System.Configuration;
	using System.Web.Mvc;

	using SEI.Web.Models.Home;
	
	public class HomeController : BaseController
	{
		[Route("")]
		[Route("news")]
		[Route("chart")]
		public ActionResult Index()
		{
			return File("~/Scripts/dist/index.html", "text/html");
		}

		[Route("News/Data")]
		public ActionResult NewsDataUris()
		{
			var newsServiceScheme = ConfigurationManager.AppSettings["NewsServiceScheme"];
			var newsServiceDomain = ConfigurationManager.AppSettings["NewsServiceDomain"];
			var newsServicePort = ConfigurationManager.AppSettings["NewsServicePort"];

			var dataUris = new NewsDataUris()
			{
				NewsServiceScheme = newsServiceScheme,
				NewsServiceDomain = newsServiceDomain,
				NewsServicePort = newsServicePort
			};

			return Json(dataUris, JsonRequestBehavior.AllowGet);
		}
	}
}