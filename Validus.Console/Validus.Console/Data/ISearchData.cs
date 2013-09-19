using System.Collections.Generic;
using Validus.Console.DTO;

namespace Validus.Console.Data
{
    public interface ISearchData
    {
        string GetSearchResultsString(string searchTerm, int skip, int take);
        System.Xml.XmlDocument GetSearchResultsXML(string searchTerm, int skip, int take);
        SearchResponseDto GetSearchResults(string searchTerm, int skip, int take);
    }
}
