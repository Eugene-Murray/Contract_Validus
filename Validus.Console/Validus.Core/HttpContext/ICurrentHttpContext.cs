using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Validus.Core.HttpContext
{
    public interface ICurrentHttpContext
    {
        IPrincipal CurrentUser { get; set; }
        HttpContextBase Context  { get; set; }
    }
}
