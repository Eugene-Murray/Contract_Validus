using System;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;

namespace Validus.Console.Controllers
{
	[Authorize(Roles = @"ConsoleRead")]
    public class SearchController : Controller
    {
        private readonly ISearchBusinessModule _businessModule;

        public SearchController(ISearchBusinessModule businessModule)
        {
            this._businessModule = businessModule;
        }

        public string Index()
        {
            return string.Empty;
        }

		// TODO: Why are there two return types ? (a view of html and a string of html)
		[OutputCache(CacheProfile = "NoCacheProfile")] // TODO: Can't we just vary cache by parameter, when does it crawl ?
        public string IndexPartial(String searchTerm, Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            return _businessModule.GetSearchResultsHtml(searchTerm, iDisplayStart, iDisplayLength);
        }

		// TODO: Why are there two return types ? (a view of html and a string of html)
		[OutputCache(CacheProfile = "NoCacheProfile")] // TODO: Can't we just vary cache by parameter, when does it crawl ?
        public ActionResult _SearchResults(String searchTerm, Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            var searchContent = _businessModule.GetSearchResults(searchTerm, iDisplayStart, iDisplayLength);

            return PartialView(searchContent);
        }

        public ActionResult _SearchClaim(SearchResultsBaseDto item)
        {
            return PartialView((SearchResultClaimDto)item);
        }

        public ActionResult _SearchBroker(SearchResultsBaseDto item)
        {
            return PartialView((SearchResultBrokerDto)item);
        }

        public ActionResult _SearchSubscribe(SearchResultsBaseDto item)
        {
            return PartialView((SearchResultSubscribeDto)item);
        }
    }
}