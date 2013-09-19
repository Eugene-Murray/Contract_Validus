using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.Controllers
{
	[Authorize(Roles = @"ConsoleRead")]
    public class RiskCodeController : Controller
    {
        private readonly IRiskCodeModuleManager _riskCodeServiceModuleManager;
		private readonly ILogHandler _logHandler;

        public RiskCodeController(IRiskCodeModuleManager riskCodeServiceModuleManager, ILogHandler logHandler)
        {
            _riskCodeServiceModuleManager = riskCodeServiceModuleManager;
            _logHandler = logHandler;
        }

		[HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public JsonResult GetBySubmissionTypeId(string submissionTypeId)
        {
            try
            {
                var riskCodes = _riskCodeServiceModuleManager.GetRiskCodesBySubmissionTypeId(submissionTypeId);
                return Json(riskCodes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
                throw new HttpException(500, "Server Error", ex);
            }
        }

    }
}