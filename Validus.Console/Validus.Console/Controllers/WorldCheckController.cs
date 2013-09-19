using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.WorldCheckService;
using Validus.Core.LogHandling;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead")]
    public class WorldCheckController : Controller
    {
        private IWorldCheckBusinessModule _bm;
        private readonly ILogHandler _logHandler;

        public WorldCheckController(IWorldCheckBusinessModule bm, ILogHandler logHandler)
        {
            _bm = bm;
            _logHandler = logHandler;
        }

        [HttpGet]
		[OutputCache(CacheProfile = "NoCacheProfile")]
		public ActionResult _WorldCheckSearchMatches(string insuredName)
		{
            try
			{
				if (string.IsNullOrEmpty(insuredName)) return PartialView(new List<SearchResult>());

                List<SearchResult> matches = _bm.GetWorldCheckMatches(Uri.UnescapeDataString(insuredName));
                Response.StatusCode = (Int32)HttpStatusCode.Created;
		        return PartialView(matches);
			}
			catch (Exception ex)
			{
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
				throw new HttpException(500, "Server Error", ex);
			}
		}

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _WorldCheckDetailsModal(string uid)
        {
            try{
                WorldCheckService.WorldCheckItem detail = _bm.GetWorldCheckDetail(uid);
                Response.StatusCode = (Int32)HttpStatusCode.Created;
                return PartialView(detail);
			}
			catch (Exception ex)
			{
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
				throw new HttpException(500, "Server Error", ex);
			}
        }

        [HttpPost]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _LogWorldCheckMatch(string uid)
        {
            try
            {
                _bm.SetWorldCheckMatches(uid);
                Response.StatusCode = (Int32)HttpStatusCode.Created;
                return null;
			}
			catch (Exception ex)
			{
				_logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.Controller);
				throw new HttpException(500, "Server Error", ex);
			}
        }

    }
}
