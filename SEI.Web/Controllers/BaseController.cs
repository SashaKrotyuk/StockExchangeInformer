namespace SEI.Web.Controllers
{
	using System.Collections;
	using System.Text;
	using System.Web.Mvc;

	using SEI.Web.Infrastructure.Results;

	public abstract class BaseController : Controller
	{
		public ActionResult XML(object model)
		{
			return new XMLResult(model);
		}

		public ActionResult CSV(IEnumerable model, string fileName)
		{
			return new CSVResult(model, fileName);
		}

		protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new JsonDotNetResult
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
				JsonRequestBehavior = behavior
			};
		}
	}
}