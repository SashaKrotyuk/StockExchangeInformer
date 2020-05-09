namespace SEI.Web.Infrastructure.Results
{
	using System.Web.Mvc;
	using System.Xml.Serialization;

	public class XMLResult : ActionResult
	{
		private readonly object data;

		public XMLResult(object data)
		{
			this.data = data;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			XmlSerializer serializer = new XmlSerializer(this.data.GetType());
			var response = context.HttpContext.Response;
			response.ContentType = "text/xml";
			serializer.Serialize(response.Output, this.data);
		}
	}
}