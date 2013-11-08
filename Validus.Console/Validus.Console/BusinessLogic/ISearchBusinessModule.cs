using System.Collections.Generic;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
    public interface ISearchBusinessModule
    {
        string GetSearchResultsHtml(string searchTerm, int skip, int take);
        SearchResponseDto GetSearchResults(string searchTerm, Dictionary<string, string> sources, Dictionary<string, string> refiners, int skip, int take);
    }
}
