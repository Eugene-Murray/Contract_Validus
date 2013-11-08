using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Console.WorldCheckService;
using Validus.Core.LogHandling;
using Validus.Core.MVC;

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
        public ActionResult _WorldCheckSearchForm()
        {
            var searchCriteria = new WorldCheckSearchCriteriaDto();
            return PartialView(searchCriteria);
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _WorldCheckSearchFormData()
        {
            var _categories = _bm.GetCategories();
            var _countries = _bm.GetCountries();
            var categories = new List<string>();
            var countries = new List<string>();
            // add the all/blank
            categories.Add(string.Empty);
            countries.Add(string.Empty);
            countries.AddRange(_countries.Select(item => item.Name));
            categories.AddRange(_categories.Select(item => item.Name));
            var searchCriteria = new WorldCheckSearchCriteriaDto();
            return new JsonNetResult
            {
                Data = new { Countries = countries, Categories = categories, Criteria = searchCriteria }
            };
        }
        
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _Search(WorldCheckSearchCriteriaDto criteria)
        {
            try
            {
                if((string.IsNullOrEmpty(criteria.Name)) 
                    && (string.IsNullOrEmpty(criteria.Keywords))
                    && (string.IsNullOrEmpty(criteria.Country))
                    && (string.IsNullOrEmpty(criteria.Category)))
                    return PartialView(new List<SearchResult>());

                List<SearchResult> matches = _bm.GetWorldCheckMatches(criteria);
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
        public ActionResult _GetCountries()
        {
            var aaData = _bm.GetCountries();

            return new JsonNetResult
            {
                Data = aaData
            };
        }

        [HttpGet]
        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _GetCategories()
        {
            var aaData = _bm.GetCategories();

            return Json(new { aaData }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
		[OutputCache(CacheProfile = "NoCacheProfile")]
		public ActionResult _WorldCheckSearchMatches(string insuredName)
		{
            try
			{
				if (string.IsNullOrEmpty(insuredName)) return PartialView(new List<SearchResult>());

                List<SearchResult> matches = _bm.GetWorldCheckMatches(Uri.UnescapeDataString(insuredName.Trim()));
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
