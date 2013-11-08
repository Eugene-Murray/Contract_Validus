using System.Collections.Generic;
using Validus.Console.DTO;
using System;

namespace Validus.Console.Data
{
    public interface ISearchData
    {
        string GetSearchResultsString(string searchTerm, int skip, int take);
        System.Xml.XmlDocument GetSearchResultsXML(string searchTerm, int skip, int take);
        SearchResponseDto GetSearchResults(string searchTerm, Dictionary<string, string> sources, Dictionary<String, String> refiners, int skip, int take);        
    }
}
