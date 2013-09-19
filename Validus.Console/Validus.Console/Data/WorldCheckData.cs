using System.Collections.Generic;
using System.Linq;

namespace Validus.Console.Data
{
    public class WorldCheckData : IWorldCheckData
    {
        public List<WorldCheckService.SearchResult> GetWorldCheckMatches(string name, string username)
        {
            using (var client = new WorldCheckService.WorldCheckService())
            {
                client.UseDefaultCredentials = true;

                var searchResults = client.GetSearchMatches(name, null, null, null, null, username);

				// TODO: Remove Take(200) once WorldCheck service has been improved
                return searchResults.Take(200).ToList();
            }
        }

        public WorldCheckService.WorldCheckItem GetWorldCheckDetail(string uid)
        {
            using (var client = new WorldCheckService.WorldCheckService())
            {
                client.UseDefaultCredentials = true;

				return client.GetWorldCheckItemByUID(uid);
            }
        }

        public void LogWorldCheckMatch(string uid, string username)
        {
            using (var client = new WorldCheckService.WorldCheckService())
            {
                client.UseDefaultCredentials = true;

                client.SetItemAsSaved(uid, username, null);
            }
        }
    }
}