using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.InsuredService;
using Validus.Core.LogHandling;

namespace Validus.Console.Controllers
{
	[Authorize(Roles = @"ConsoleRead")]
    public class InsuredController : Controller
    {
		private readonly IInsuredBusinessModule _bm;
		private readonly ILogHandler _logHandler;

		public InsuredController(IInsuredBusinessModule bm, ILogHandler logHandler)
		{
			this._bm = bm;
			this._logHandler = logHandler;
		}

        public ActionResult _InsuredDetailsPreview(string insuredName)
        {
			try
			{
				var insuredDetails = (!string.IsNullOrEmpty(insuredName))
										 ? this._bm.GetInsuredDetailsByName(Uri.UnescapeDataString(insuredName))
										 : new List<InsuredDetails>();

				return PartialView(insuredDetails);
			}
			catch (Exception ex)
			{
				this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);

				throw new HttpException(500, "Server Error", ex);
			}
        }

		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
        public ActionResult _InsuredDetailsMinimal(string insuredName)
        {
            try
            {
	            var insuredDetails = (!string.IsNullOrEmpty(insuredName))
		                                 ? this._bm.GetInsuredDetailsByName(Uri.UnescapeDataString(insuredName))
		                                 : new List<InsuredDetails>();

                return PartialView(insuredDetails);
            }
            catch (Exception ex)
            {
                this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);

                throw new HttpException(500, "Server Error", ex);
            }
        }

        public ActionResult _InsuredDetailsMinimalByCobs(string insuredName)
        {
            try
			{
				var insuredDetails = (!string.IsNullOrEmpty(insuredName))
										 ? this._bm.GetInsuredDetailsByNameAndCobs(Uri.UnescapeDataString(insuredName))
										 : new List<InsuredDetails>();

				return PartialView(insuredDetails);
            }
            catch (Exception ex)
            {
                this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);

                throw new HttpException(500, "Server Error", ex);
            }
        }

        public ActionResult _InsuredDetailsPreviewByCob(string insuredName)
        {
            try
			{
				var insuredDetails = (!string.IsNullOrEmpty(insuredName))
										 ? this._bm.GetInsuredDetailsByNameAndCobs(Uri.UnescapeDataString(insuredName))
										 : new List<InsuredDetails>();

				return PartialView(insuredDetails);
            }
            catch (Exception ex)
            {
                this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);

                throw new HttpException(500, "Server Error", ex);
            }
        }

        public ActionResult _InsuredQuickview()
        {
            return PartialView();
        }

        public ActionResult _InsuredSearchResultPreview(string insuredName)
        {
            ViewBag.InsuredName = insuredName;
            return PartialView();
        }
    }
}