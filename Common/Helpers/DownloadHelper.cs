namespace Common.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Reflection;
	using System.Text;
	using System.Web;

	using Common.Serialization;

	using log4net;

	public static class DownloadHelper
	{
		public static TResponse GetData<TResponse>(string url, Dictionary<string, string> headers, ResponseDeserializer deserializer) where TResponse : class
		{
			return DoRequest<TResponse>(url, "GET", null, headers, deserializer);
		}

		public static TResponse PostData<TResponse>(string url, Dictionary<string, string> body, ResponseDeserializer deserializer) where TResponse : class
		{
			return DoRequest<TResponse>(url, "POST", body, null, deserializer);
		}

		public static T ParseJson<T>(string rawJson) where T : class
		{
			return (T)ParseJson(typeof(T), rawJson);
		}

		public static string ToUrlEncoded(this Dictionary<string, string> parameters)
		{
			var sb = new StringBuilder();

			var b = true;

			foreach (var p in parameters)
			{
				if (!b)
				{
					sb.Append("&");
				}

				b = false;

				sb.Append(p.Key);
				sb.Append("=");
				sb.Append(HttpUtility.UrlEncode(p.Value));
			}

			return sb.ToString();
		}

		private static TItem DoRequest<TItem>(string url, string method, Dictionary<string, string> data, Dictionary<string, string> headers, ResponseDeserializer deserializer) where TItem : class
		{
			var log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			var request = (HttpWebRequest)WebRequest.Create(url);

			request.Method = method;
			request.AllowAutoRedirect = false;
			request.ContentLength = 0;

			if (data != null)
			{
				request.ContentType = "application/x-www-form-urlencoded";
				var body = data.ToUrlEncoded();
				request.ContentLength = body.Length;

				using (var sm = request.GetRequestStream())
				{
					using (var sw = new StreamWriter(sm))
					{
						sw.Write(body);
					}
				}

				log.DebugFormat("{2} {0} with body {1}", url, body, method);
			}

			if (headers != null)
			{
				request.Headers = new WebHeaderCollection();

				foreach (var header in headers)
				{
					request.Headers.Add(header.Key, header.Value);
				}

				log.DebugFormat("{2} {0} with headers {1}", url, string.Join(";", headers.Select(x => x.Key + ":" + x.Value)), method);
			}

			using (var response = request.GetResponse() as HttpWebResponse)
			{
				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new HttpException($"Response status is {response.StatusCode}");
				}

				if (response.ContentLength == 0)
				{
					throw new HttpException("Response doesn't contain content");
				}

				using (var rs = response.GetResponseStream())
				{
					using (var sr = new StreamReader(rs))
					{
						var content = sr.ReadToEnd();

						log.DebugFormat("{1} {2} response body\n{0}", content, method, url);

						return (TItem)deserializer.Deserialize(typeof(TItem), content);
					}
				}
			}
		}

		private static object ParseJson(Type type, string rawJson)
		{
			return Serializer.Json.Deserialize(type, rawJson);
		}

		public abstract class ResponseDeserializer
		{
			public abstract object Deserialize(Type objectType, string content);
		}

		public class JsonDeserializer : ResponseDeserializer
		{
			public override object Deserialize(Type objectType, string content)
			{
				return ParseJson(objectType, content);
			}
		}

		public class XmlDeserializer : ResponseDeserializer
		{
			public override object Deserialize(Type objectType, string content)
			{
				return Serializer.Xml.Deserialize(objectType, content);
			}
		}
	}
}