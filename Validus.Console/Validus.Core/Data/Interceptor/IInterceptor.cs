using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;

namespace Validus.Core.Data.Interceptor
{
    /// <summary>
    /// Interface to support taking some action in response
    /// to some activity taking place on an ObjectStateEntry item.
    /// </summary>
    internal interface IInterceptor
    {
        void Before(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> context);

        void After(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> context);

    }

}
