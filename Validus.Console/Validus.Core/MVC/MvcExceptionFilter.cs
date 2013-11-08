using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Core.LogHandling;

namespace Validus.Core.MVC
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class MvcExceptionFilter : FilterAttribute, IExceptionFilter
	{
		protected ILogHandler LogHandler { get; set; }

		public string MasterName { get; set; }
		public string ViewName { get; set; }
		
		public IDictionary<Type, HttpStatusCode> Mappings { get; protected set; }

		public MvcExceptionFilter(string masterName = null, string viewName = null,
		                          IDictionary<Type, HttpStatusCode> mappings = null)
		{
			this.LogHandler = new LogHandler();

			this.MasterName = masterName;
			this.ViewName = viewName;

			this.Mappings = mappings ?? new Dictionary<Type, HttpStatusCode>
			{
				// 400 : Bad Request
				{typeof (ArgumentException), HttpStatusCode.BadRequest},
				{typeof (ArgumentNullException), HttpStatusCode.BadRequest},
				{typeof (ArgumentOutOfRangeException), HttpStatusCode.BadRequest},
				
				// 401 : Unauthorized
				{typeof (UnauthorizedAccessException), HttpStatusCode.Unauthorized},

				// 403 : Forbidden
				{typeof (AccessViolationException), HttpStatusCode.Forbidden},
				
				// 404 : Not Found
				{typeof (KeyNotFoundException), HttpStatusCode.NotFound},
				
				// 501 : Not Implemented
				{typeof (NotImplementedException), HttpStatusCode.NotImplemented},
				{typeof (NotSupportedException), HttpStatusCode.NotImplemented}
			};
		}

		public void OnException(ExceptionContext filterContext)
		{
			if (this.IsValidExceptionContext(filterContext))
			{
				var statusCode = this.GetStatusCode(filterContext.Exception);

				this.LogException(filterContext.Exception);

				if (statusCode >= (int) HttpStatusCode.InternalServerError)
				{
					this.SendNotification(filterContext);
				}

				filterContext.ExceptionHandled = true;
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.StatusCode = statusCode;
				filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

				if (this.IsJsonResponse(filterContext.HttpContext)
					|| filterContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.Result = new JsonNetResult
					{
						Data = new
						{
							Error = filterContext.Exception,
							Url = (filterContext.HttpContext.Request.Url != null)
								? filterContext.HttpContext.Request.Url.AbsoluteUri
								: string.Empty
						}
					};
				}
				else if (!string.IsNullOrEmpty(this.MasterName) & !string.IsNullOrEmpty(this.ViewName))
				{
					var model = new HandleErrorInfo(filterContext.Exception,
									Convert.ToString(filterContext.RouteData.Values["controller"]),
									Convert.ToString(filterContext.RouteData.Values["action"]));

					filterContext.Result = new ViewResult // TODO: What about partial views ?
					{
						ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
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
						Content = filterContext.Exception.Message
					};
				}
			}
		}

		protected bool IsValidExceptionContext(ExceptionContext filterContext)
		{
			return !filterContext.ExceptionHandled
			       && filterContext.HttpContext != null
			       && filterContext.Exception != null;
		}

		protected bool IsJsonResponse(HttpContextBase httpContext)
		{
			return httpContext.Request.AcceptTypes
			                  .Any(acceptType => acceptType.Contains("json"));
		}

		protected int GetStatusCode(Exception exception)
		{
			var httpException = exception as HttpException;
			var httpStatusCode = (httpException != null)
				                     ? httpException.GetHttpCode()
				                     : (this.Mappings.ContainsKey(exception.GetType()))
					                       ? (int)this.Mappings[exception.GetType()]
					                       : (int)HttpStatusCode.InternalServerError;

			return httpStatusCode;
		}

		protected void SendNotification(ExceptionContext filterContext)
		{
			// TODO: Send configurable email notification ?
		}

		protected void LogException(Exception exception)
		{
			if (this.LogHandler != null)
			{
				this.LogHandler.WriteLog(exception.Message, LogSeverity.Error, LogCategory.Controller);
			}
		}
	}
}