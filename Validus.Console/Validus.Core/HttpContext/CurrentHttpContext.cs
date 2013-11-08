using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Validus.Core.HttpContext
{
    public class CurrentHttpContext : ICurrentHttpContext
    {
        private IPrincipal _principalVal;
        private HttpContextBase _contextBase;

        public virtual IPrincipal CurrentUser
        {
            get { return _principalVal ?? (_principalVal = System.Web.HttpContext.Current.User); }
            set { _principalVal = value; }
        }

        public virtual HttpContextBase Context
        {
            get { return _contextBase ?? (new HttpContextWrapper(System.Web.HttpContext.Current)); }
            set { _contextBase = value;}
        }
    }

    

    
}
