using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using Validus.Console.DTO;
using Validus.Console.Data;

namespace Validus.Console.BusinessLogic
{
	public class WorldCheckBusinessModule : IWorldCheckBusinessModule
	{
	    private readonly IWorldCheckData _rep;

        public WorldCheckBusinessModule(IWorldCheckData rep)
        {
            _rep = rep;
        }

        public List<WorldCheckService.SearchResult> GetWorldCheckMatches(string insuredName)
        {
            return _rep.GetWorldCheckMatches(insuredName,
                                             HttpContext.Current.User.Identity.Name.ToString(
                                                 CultureInfo.InvariantCulture));
        }

        public List<WorldCheckService.SearchResult> GetWorldCheckMatches(WorldCheckSearchCriteriaDto criteria)
        {

            return _rep.GetWorldCheckMatches(string.IsNullOrEmpty(criteria.Name) ? null: criteria.Name ,
                                            string.IsNullOrEmpty(criteria.Keywords) ? null : criteria.Keywords,
                                            string.IsNullOrEmpty(criteria.Country) ? null : criteria.Country,
                                            string.IsNullOrEmpty(criteria.Category) ? null : criteria.Category,
                                             HttpContext.Current.User.Identity.Name.ToString(
                                                 CultureInfo.InvariantCulture));
        }

        public WorldCheckService.WorldCheckItem GetWorldCheckDetail(string uid)
        {
            return _rep.GetWorldCheckDetail(uid);
        }

        public void SetWorldCheckMatches(string uid)
        {
            _rep.LogWorldCheckMatch(uid,
                                    HttpContext.Current.User.Identity.Name.ToString(CultureInfo.InvariantCulture));
        }

        public WorldCheckService.Category[] GetCategories()
        {
            return _rep.GetCategories();
        }

        public WorldCheckService.Country[] GetCountries()
        {
            return _rep.GetCountries();
        }

		public DataTable SearchByInsured(string insuredName)
		{
			throw new NotImplementedException();
		}
	}
}