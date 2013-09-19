using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Validus.Core.MVC
{
	// TODO: Are these AttributeUsage arguments correct ?
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class MvcModelStateFilter : ActionFilterAttribute
	{
		public string MasterName { get; set; }
		public string ViewName { get; set; }

		public MvcModelStateFilter()
		{ }

		public MvcModelStateFilter(string masterName, string viewName)
		{
			this.MasterName = masterName;
			this.ViewName = viewName;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var modelState = filterContext.Controller.ViewData.ModelState;

			if (modelState != null && !modelState.IsValid)
			{
				var modelErrors = modelState.Where(kvp => kvp.Value.Errors.Count > 0)
				                            .ToDictionary(kvp => kvp.Key,
				                                          kvp => kvp.Value.Errors
				                                                    .Select(error => error.ErrorMessage)
				                                                    .ToArray());

				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;

				if (this.IsJsonResponse(filterContext.HttpContext)
					|| filterContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.Result = new JsonNetResult
					{
						Data = new
						{
							Error = modelErrors,
							Url = (filterContext.HttpContext.Request.Url != null)
								? filterContext.HttpContext.Request.Url.AbsoluteUri
								: string.Empty
						}
					};
				}
				else if (!string.IsNullOrEmpty(this.MasterName) & !string.IsNullOrEmpty(this.ViewName))
				{
					filterContext.Result = new ViewResult // TODO: What about partial views ?
					{
						ViewData = new ViewDataDictionary(modelErrors),
						TempData = filterContext.Controller.TempData,
						MasterName = this.MasterName,
						ViewName = this.ViewName
					};
				}
				else
				{
					filterContext.Result = new ContentResult
					{
						ContentType = "plain/text",
						Content = string.Join(" ", modelErrors.Select
						(
							error => string.Format("{0}={1}", error.Key, error.Value))
						)
					};
				}
			}
		}

		protected bool IsJsonResponse(HttpContextBase httpContext)
		{
			return httpContext.Request.AcceptTypes
							  .Any(acceptType => acceptType.Contains("json"));
		}
	}
}