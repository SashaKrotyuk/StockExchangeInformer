namespace SEI.Web.Controllers
{
	using System.Web.Mvc;

	public class StaticFilesController : BaseController
	{
		public FileContentResult Js(string fileName)
		{
			return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/{fileName}.js")), "text/javascript");
		}

		public FileContentResult Css(string fileName)
		{
			return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/{fileName}.css")), "text/css");
		}

		public FileContentResult Woff2(string fileName)
		{
			return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/{fileName}.woff2")), "application/octet-stream");
		}

		public FileContentResult Woff(string fileName)
		{
			return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/{fileName}.woff")), "application/octet-stream");
		}

		public FileContentResult Ttf(string fileName)
		{
			return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/{fileName}.ttf")), "application/octet-stream");
		}

		public FileContentResult Json(string fileName)
		{
			if (fileName == "manifest")
			{
				return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/assets/{fileName}.json")), "application/json");
			}

			return File(System.IO.File.ReadAllBytes(Server.MapPath($"/Scripts/dist/assets/i18n/{fileName}.json")), "application/json");
		}
	}
}