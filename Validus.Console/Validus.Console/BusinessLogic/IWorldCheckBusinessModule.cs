using System;
using System.Collections.Generic;
using System.Data;

namespace Validus.Console.BusinessLogic
{
	public interface IWorldCheckBusinessModule
	{
	    List<WorldCheckService.SearchResult> GetWorldCheckMatches(string insuredName);
	    WorldCheckService.WorldCheckItem GetWorldCheckDetail(string uid);
		DataTable SearchByInsured(String insuredName);
	    void SetWorldCheckMatches(string uid);
	}
}