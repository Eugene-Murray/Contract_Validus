using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Validus.Console.Controllers
{
	/* [Validus.Core.MVC.MvcExceptionFilter()]
	 * 
	 * This attribute is only needed if it has not already been included in the GlobalFilterCollection
	 */
	public class ErrorController : Controller
    {
	    //
		// GET: /Error/
		/// <summary>
		/// This looks for the exception passed to it from the Application_Error event handler if
		/// it exists and throws the exception so that it will be handled by the MvcExceptionFilter,
		/// which builds the appropriate error response.
		/// </summary>
		/// <returns>Result from MvcExceptionFilter</returns>
		public ActionResult Index()
		{
			throw this.RouteData.Values["exception"] as HttpException ??
			      new HttpException((int) HttpStatusCode.InternalServerError, 
					"Unexpected Application Error");
		}
    }
}