using System.Collections.Generic;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
    public interface ISearchBusinessModule
    {
        string GetSearchResultsHtml(string searchTerm, int skip, int take);
        SearchResponseDto GetSearchResults(string searchTerm, int skip, int take);
    }
}
