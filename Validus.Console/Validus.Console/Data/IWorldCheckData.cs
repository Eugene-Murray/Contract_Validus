using System.Collections.Generic;

namespace Validus.Console.Data
{
    public interface IWorldCheckData
    {
        List<WorldCheckService.SearchResult> GetWorldCheckMatches(string name, string username);

        List<WorldCheckService.SearchResult> GetWorldCheckMatches(string name, string keyword, string country,
                                                                  string category, string username);
        WorldCheckService.WorldCheckItem GetWorldCheckDetail(string uid);
        void LogWorldCheckMatch(string uid, string username);
        WorldCheckService.Country[] GetCountries();
        WorldCheckService.Category[] GetCategories();
    }
}