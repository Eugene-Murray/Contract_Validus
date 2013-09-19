using System.Web.Mvc;
using Validus.Core.MVC;

namespace Validus.Console
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//filters.Add(new MvcModelStateFilter()); // TODO: Add global filter once all controllers is ready
			filters.Add(new MvcExceptionFilter());
        }
    }
}