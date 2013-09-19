using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
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

        public WorldCheckService.WorldCheckItem GetWorldCheckDetail(string uid)
        {
            return _rep.GetWorldCheckDetail(uid);
        }

        public void SetWorldCheckMatches(string uid)
        {
            _rep.LogWorldCheckMatch(uid,
                                    HttpContext.Current.User.Identity.Name.ToString(CultureInfo.InvariantCulture));
        }

		public DataTable SearchByInsured(string insuredName)
		{
			throw new NotImplementedException();
		}
	}
}