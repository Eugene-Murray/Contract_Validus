using System;
using System.Data.Entity;
using System.Data.Objects;
using System.Threading;

namespace Validus.Core.Data.Interceptor.Interceptors
{
    internal class AuditChangeInterceptor : ChangeInterceptor<IAudit>
    {
        public override void OnBeforeInsert(DbContext dbContext, ObjectStateManager manager, IAudit item)
        {
            base.OnBeforeInsert(dbContext,manager, item);

            item.CreatedOn = DateTime.Now;
            item.CreatedBy = Thread.CurrentPrincipal.Identity.Name;

            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
        }

        public override void OnBeforeUpdate(DbContext dbContext, ObjectStateManager manager, IAudit item)
        {
            base.OnBeforeUpdate(dbContext,manager, item);

			// TODO: Why is CreatedOn equal to DateTime.MinValue on update ?
	        if (item.CreatedOn == DateTime.MinValue)
	        {
		        item.CreatedOn = DateTime.Now;
		        item.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
	        }

            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
        }
    }
}
