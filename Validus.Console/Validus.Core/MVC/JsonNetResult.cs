using System;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Validus.Core.MVC
{
	public class JsonNetResult : ActionResult
	{
		public JsonSerializerSettings SerializerSettings { get; set; }
		public Encoding ContentEncoding { get; set; }
		public Formatting Formatting { get; set; }
		public string ContentType { get; set; }
		public object Data { get; set; }

		public JsonNetResult(JsonSerializerSettings jsonSettings = null)
		{
			this.SerializerSettings = jsonSettings
			                          ?? GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
			                          ?? new JsonSerializerSettings();
		}

		public override void ExecuteResult(ControllerContext controllerContext)
		{
			if (controllerContext == null) throw new ArgumentNullException("controllerContext");

			var httpResponse = controllerContext.HttpContext.Response;

			httpResponse.ContentType = this.ContentType ?? "application/json";
			httpResponse.ContentEncoding = this.ContentEncoding ?? Encoding.Default;

			if (this.Data != null)
			{
				var jsonSerializer = JsonSerializer.Create(this.SerializerSettings);
				var jsonWriter = new JsonTextWriter(httpResponse.Output)
					{
						Formatting = this.Formatting
					};

				jsonSerializer.Serialize(jsonWriter, this.Data);
				jsonWriter.Flush();
			}
		}
	}
}