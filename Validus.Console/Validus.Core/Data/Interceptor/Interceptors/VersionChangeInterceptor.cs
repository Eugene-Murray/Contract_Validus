using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

namespace Validus.Core.Data.Interceptor.Interceptors
{
    internal class VersionChangeInterceptor : ChangeInterceptor<IVersion>
    {
        public override void OnBeforeInsert(DbContext dbContext, ObjectStateManager manager, IVersion item)
        {
            base.OnBeforeInsert(dbContext, manager, item);
            item.VersionNo = 1;
            item.Key = Guid.NewGuid();
            item.IsObsolete = false;
        }

        public override void OnBeforeUpdate(DbContext dbContext, ObjectStateManager manager, IVersion item)
        {
            if (item.VersionNo ==0) throw new Exception("Unexpected , version number should not be zero while update.");
            base.OnBeforeUpdate(dbContext, manager, item);
            item.VersionNo = item.VersionNo + 1;
            manager.ChangeObjectState(item, EntityState.Added);
        }


        public override void OnAfterUpdate(DbContext dbContext, ObjectStateManager manager, IVersion item)
        {
            //http://stackoverflow.com/questions/15481685/querying-against-dbcontext-settypevariable-in-entity-framework

            base.OnBeforeInsert(dbContext, manager, item);
            var set = dbContext.Set(item.GetType());

            // ReSharper disable SuspiciousTypeConversion.Global
            var query = set as IQueryable<IVersion>;
            // ReSharper restore SuspiciousTypeConversion.Global

            if (query != null)
            {
                var versionEntitys = query.Where(e => e.Key == item.Key && e.IsObsolete == false).ToList();
                foreach (var versionEntity in versionEntitys.Where(version => version.VersionNo != versionEntitys.Max(v => v.VersionNo)))
                {
                    versionEntity.IsObsolete = true;
                }
            }

            //todo: Need to work on this.

            //var types = new List<Type>();

            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //foreach (var assembly in assemblies)
            //{
            //    types.AddRange(assembly.GetTypes().Where(type => type.IsDefined(typeof(VersionAttribute), false)));
            //}

            //foreach (var type in types)
            //{
            //    foreach (var memberInfo in type.GetMembers())
            //    {
            //        memberInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof (VersionAttribute));
            //    }
            //}
        }

    }

}
