using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Validus.Core.HTTP
{
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HttpModelStateFilter:ActionFilterAttribute
    {
	     public override void OnActionExecuting(HttpActionContext filterContext)
	     {
		     var modelState = filterContext.ModelState;

		     if (modelState != null && !modelState.IsValid)
		     {
			     // TODO: Exclude errors without an error message
			     var modelErrors = modelState.Where(kvp => !kvp.Value.Errors.All(e => string.IsNullOrEmpty(e.ErrorMessage)))
			                                 .ToDictionary(kvp => kvp.Key,
			                                               kvp => kvp.Value.Errors
			                                                         .Select(error => error.ErrorMessage)
			                                                         .ToArray());

			     filterContext.Response = filterContext.Request.CreateResponse<object>(HttpStatusCode.BadRequest, new
			     {
				     Error = modelErrors,
			     });
		     }
	     }
    }
}
