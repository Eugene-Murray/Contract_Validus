using System;
using System.Data;
using System.Data.Objects;

namespace Validus.Core.Data.Interceptor
{
    /// <summary>
    /// Adds conditional execution to an IInterceptor.
    /// </summary>
    internal interface IConditionalInterceptor : IInterceptor
    {
        bool IsTargetEntity(ObjectStateEntry item);
        bool IsTargetRelationshipEntityBefore(Tuple<ObjectStateEntry, EntityState, bool> item);
        bool IsTargetRelationshipEntityAfter(Tuple<ObjectStateEntry, EntityState, bool> item);
    }
}
