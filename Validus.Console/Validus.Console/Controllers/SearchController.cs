using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using System.Linq;

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
        public ActionResult _SearchResults(String searchTerm, String[] sources, String[] refiners, Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            Dictionary<String, String> srcs = null;
            if (sources != null && sources.Length > 0)
            {
                srcs = sources.Select(src => src.Split(':')).ToDictionary(defs => defs[0], defs => defs[1]);
            }

            Dictionary<String, String> refs = null;
            if (refiners != null && refiners.Length > 0)
            {
                var rs = refiners.Select(r => new Tuple<String, String>(r.Split(new Char[] { ':' })[0], r.Split(new Char[] { ':' })[1]))
                    .GroupBy(r => r.Item1);
                refs = rs.ToDictionary(g => g.Key, g => g.Select(t => t.Item2).Aggregate((a, b) => a + "," + b));
            }

            var searchContent = _businessModule.GetSearchResults(searchTerm, srcs, refs, iDisplayStart, iDisplayLength);

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