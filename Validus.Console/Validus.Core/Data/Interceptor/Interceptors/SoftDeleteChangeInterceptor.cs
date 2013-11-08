using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;

namespace Validus.Core.Data.Interceptor.Interceptors
{
    internal class SoftDeleteChangeInterceptor : ChangeInterceptor<ISoftDelete>
    {
        public override void OnBeforeInsert(DbContext dbContext, ObjectStateManager manager, ISoftDelete item)
        {
            base.OnBeforeInsert(dbContext,manager, item);
            item.IsDeleted = false;
        }

        public override void OnBeforeDelete(DbContext dbContext, ObjectStateManager manager, ISoftDelete item)
        {
            if (item.IsDeleted)
                throw new InvalidOperationException("Item is already deleted.");

            base.OnBeforeDelete(dbContext,manager, item);
            dbContext.Entry(item).Reload();
            item.IsDeleted = true;
            manager.ChangeObjectState(item, EntityState.Modified);
        }
    }

}
