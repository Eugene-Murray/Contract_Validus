using System;

namespace Validus.Core.Data.Interceptor.Interceptors
{
    public interface IVersion
    {
        int VersionNo { get; set; }
        Guid Key { get; set; }
        bool IsObsolete { get; set; }
    }
}
