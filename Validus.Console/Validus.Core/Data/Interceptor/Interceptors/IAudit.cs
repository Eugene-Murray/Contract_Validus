using System;

namespace Validus.Core.Data.Interceptor.Interceptors
{
    public interface IAudit
    {
        DateTime? CreatedOn { get; set; }
        DateTime? ModifiedOn { set; }
        string CreatedBy { set; }
        string ModifiedBy { set; }
    }
}