using System;
using System.Data.Entity;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using System.Web.Http.Validation.Providers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Validus.Console.App_Start;
using Validus.Console.Controllers;
using Validus.Console.Data;
using Validus.Console.Init;
using Validus.Core.LogHandling;

namespace Validus.Console
{
    // NOTE: For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			IoCConfig.RegisterIoC(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.Services.RemoveAll(
 typeof(System.Web.Http.Validation.ModelValidatorProvider),
 v => v is InvalidModelValidatorProvider);
            // TODO: comment back in if you want to regenerate the DB...
			//Database.SetInitializer(new MigrateDatabaseToLatestVersion<ConsoleRepository, Configuration>());
			//new ConsoleRepository().Database.Initialize(true);

			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            DatabaseInit.SyncSemiStaticData();

			new LogHandler().WriteLog("Application_Start", LogSeverity.Information, LogCategory.UI);
        }

		protected void Application_Error(object sender, EventArgs eventArgs)
		{
			var httpException = this.Server.GetLastError() as HttpException ??
			                    new HttpException((int) HttpStatusCode.InternalServerError, 
									"Unexpected Application Error", this.Server.GetLastError());

			/* TODO: Specific controller actions dependent on http status code ?
			var httpStatusCode = (HttpStatusCode) httpException.GetHttpCode();
			var routeAction = "Index";

			switch (httpStatusCode)
			{
				case HttpStatusCode.BadRequest:		routeAction = "BadRequest"; break;
				case HttpStatusCode.Unauthorized:	routeAction = "Unauthorised"; break;
				case HttpStatusCode.Forbidden:		routeAction = "Forbidden"; break;
				case HttpStatusCode.NotFound:		routeAction = "NotFound"; break;
				case HttpStatusCode.RequestTimeout:	routeAction = "RequestTimeout"; break;
				case HttpStatusCode.Conflict:		routeAction = "Conflict"; break;
				case HttpStatusCode.NotImplemented:	routeAction = "NotImplemented"; break;
				default: routeAction = "InternalServerError"; break;
			}*/

			this.Context.ClearError();
			this.Context.Response.Clear();
			this.Context.Response.TrySkipIisCustomErrors = true;
			this.Context.Response.StatusCode = httpException.GetHttpCode();
			
			var routeData = new RouteData();

			routeData.Values["controller"] = "Error";
			routeData.Values["action"] = "Index"; // TODO: routeAction; ?
			routeData.Values["exception"] = httpException;

			IController errorController = new ErrorController();

			errorController.Execute(new RequestContext(new HttpContextWrapper(this.Context), routeData));
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var xSubmissionType = this.Context.Request.Headers["X-SubmissionType"];
            if(!string.IsNullOrEmpty(xSubmissionType))
                CallContext.LogicalSetData("X-SubmissionType", xSubmissionType);
        }

    }
}