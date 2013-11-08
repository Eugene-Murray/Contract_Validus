using System;
using System.Collections.Generic;
using System.Data;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
	public interface IWorldCheckBusinessModule
	{
	    List<WorldCheckService.SearchResult> GetWorldCheckMatches(string insuredName);
	    List<WorldCheckService.SearchResult> GetWorldCheckMatches(WorldCheckSearchCriteriaDto criteria);
	    WorldCheckService.WorldCheckItem GetWorldCheckDetail(string uid);
		DataTable SearchByInsured(String insuredName);
        void SetWorldCheckMatches(string uid);
	    WorldCheckService.Category[] GetCategories();
	    WorldCheckService.Country[] GetCountries();
	}
}