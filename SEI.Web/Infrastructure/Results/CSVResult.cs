namespace SEI.Web.Infrastructure.Results
{
	using System.Collections;
	using System.IO;
	using System.Text;
	using System.Web;
	using System.Web.Mvc;
    
	public class CSVResult : FileResult
	{
		private readonly IEnumerable data;

		public CSVResult(IEnumerable data, string fileName) : base("text/csv")
		{
			this.data = data;
			FileDownloadName = fileName;
		}

		public static string GetValue(object item, string propName)
		{
			return item.GetType().GetProperty(propName).GetValue(item, null).ToString();
		}

		protected override void WriteFile(HttpResponseBase response)
		{
			var builder = new StringBuilder();
			var stringWriter = new StringWriter(builder);

			foreach (var item in this.data)
			{
				var properties = item.GetType().GetProperties();
				foreach (var prop in properties)
				{
					stringWriter.Write(GetValue(item, prop.Name));
					stringWriter.Write(", ");
				}

				stringWriter.WriteLine();
			}

			response.Write(builder);
		}
	}
}