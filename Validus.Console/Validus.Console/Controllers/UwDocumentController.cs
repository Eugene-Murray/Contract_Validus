using System;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Core.LogHandling;

namespace Validus.Console.Controllers
{
	[Authorize(Roles = @"ConsoleRead")]
    public class UwDocumentController : Controller
	{
		private readonly IUwDocumentBusinessModule _bm;
		private readonly ILogHandler _logHandler;

		public UwDocumentController(IUwDocumentBusinessModule bm, ILogHandler logHandler)
		{
			this._bm = bm;
			this._logHandler = logHandler;
        }

		//
		// GET: /UwDocument/_Details/GUID
		[HttpGet]
		public PartialViewResult _Details(string id)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);

				throw new HttpException(500, "Server Error", ex);
			}
		}

		//
		// GET: /Submission/_Search?term=AJL099683A10;AJT111424A10;ADT117210A10
		[HttpGet,
		OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public PartialViewResult _Search(String term)
		{
			try
			{
				var uwDocuments = this._bm.SearchByPolicyIds(Uri.UnescapeDataString(term));

				return PartialView(uwDocuments);
			}
			catch (Exception ex)
			{
				this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);

				throw new HttpException(500, "Server Error", ex);
			}
		}
    }
}
