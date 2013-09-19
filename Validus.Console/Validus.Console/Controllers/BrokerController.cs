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
    public class BrokerController : Controller
    {
        private readonly IBrokerModuleManager _brokerServiceModuleManager;
		private readonly ILogHandler _logHandler;

        public BrokerController(IBrokerModuleManager brokerServiceModuleManager, ILogHandler logHandler)
        {
            _brokerServiceModuleManager = brokerServiceModuleManager;
            _logHandler = logHandler;
        }

		[HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public JsonResult GetBrokerDetailsById(string brokerCd)
        {
            try
            {
                var brokerDetails = _brokerServiceModuleManager.GetBrokerDetailsById(brokerCd);
                return Json(brokerDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
                throw new HttpException(500, "Server Error", ex);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public JsonResult GetBrokerMeasuresById(string brokerCd)
        {
            try
            {
                var team = _brokerServiceModuleManager.GetBrokerMeasuresById(brokerCd);
                return Json(team, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
                throw new HttpException(500, "Server Error", ex);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public JsonResult GetBrokerSummaryById(string brokerCd)
        {
            try
            {
                var summary = _brokerServiceModuleManager.GetBrokerSummaryById(brokerCd);
                return Json(summary, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
                throw new HttpException(500, "Server Error", ex);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public JsonResult ListBrokerMeasures()
        {
            try
            {
                var team = _brokerServiceModuleManager.ListBrokerMeasures();
                return Json(team, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
				throw new HttpException(500, "Server Error", ex);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public JsonResult GetBrokerDevelopmentStatsById(string brokerCd)
        {
            try
            {
                var result = _brokerServiceModuleManager.GetBrokerDevelopmentStatsById(brokerCd);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
				this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
				throw new HttpException(500, "Server Error", ex);
            }
        }

        public ActionResult _BrokerSearchResultPreview(int bkrSeqId)
        {
            Broker bkr = _brokerServiceModuleManager.GetBrokerBySeqId(bkrSeqId);
            return PartialView(bkr);
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult CreateBrokerContact(CreateBrokerContactDto createBrokerContactDto)
        {
            try
            {
                _brokerServiceModuleManager.CreateBrokerContact(createBrokerContactDto);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                this._logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
                throw new HttpException(500, "Server Error", ex);
            }
        }
    }
}