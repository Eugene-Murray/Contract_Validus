using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;

namespace Validus.Core.Data.Interceptor
{
    internal abstract class TypeInterceptor : IConditionalInterceptor
    {
        private readonly Type _targetType;
        public Type TargetType { get { return _targetType; } }
        protected TypeInterceptor(Type targetType)
        {
            _targetType = targetType;
        }

        public virtual bool IsTargetEntity(ObjectStateEntry item)
        {

            return item.State != EntityState.Detached &&
                   TargetType.IsInstanceOfType(item.Entity);

        }
        public virtual bool IsTargetRelationshipEntityBefore(Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            if (item.Item2 != EntityState.Detached && item.Item3)
            {
                if (item.Item2 == EntityState.Added)
                {
                    var oKey0 = (EntityKey)item.Item1.CurrentValues[0];
                    var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified |
                                                                                    EntityState.Deleted|EntityState.Unchanged)
                                    .Single(e => e.EntityKey == oKey0);

                    var oKey1 = (EntityKey)item.Item1.CurrentValues[1];
                    var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified |
                                                                                    EntityState.Deleted | EntityState.Unchanged)
                                    .Single(e => e.EntityKey == oKey1);

                    return TargetType.IsInstanceOfType(item0.Entity) || TargetType.IsInstanceOfType(item1.Entity);
                }


                if (item.Item2 == EntityState.Deleted)
                {
                    var oKey0 = (EntityKey)item.Item1.OriginalValues[0];
                    var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified |
                                                                                    EntityState.Deleted | EntityState.Unchanged)
                                    .Single(e => e.EntityKey == oKey0);

                    var oKey1 = (EntityKey)item.Item1.OriginalValues[1];

                    var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified |
                                                                                    EntityState.Deleted | EntityState.Unchanged)
                                    .Single(e => e.EntityKey == oKey1);
                    return TargetType.IsInstanceOfType(item0.Entity) || TargetType.IsInstanceOfType(item1.Entity);
                }

            }
            return false;
        }

        public virtual bool IsTargetRelationshipEntityAfter(Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            if (item.Item2 != EntityState.Detached && item.Item3)
            {
                if (item.Item2 == EntityState.Added)
                {
                    var oKey0 = (EntityKey)item.Item1.CurrentValues[0];
                    var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified |
                                                                                    EntityState.Deleted | EntityState.Unchanged)
                                    .Single(e => e.EntityKey == oKey0);

                    var oKey1 = (EntityKey)item.Item1.CurrentValues[1];
                    var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified |
                                                                                    EntityState.Deleted | EntityState.Unchanged)
                                    .Single(e => e.EntityKey == oKey1);

                    return TargetType.IsInstanceOfType(item0.Entity) || TargetType.IsInstanceOfType(item1.Entity);
                }
            }
            return false;
        }
        public void Before(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item )
        {
            if (IsTargetEntity(item.Item1))
                OnBefore(dbContext, item );
            if (IsTargetRelationshipEntityBefore(item))
                OnBeforeRelationship(dbContext,item);
        }

        protected abstract void OnBefore(DbContext dbContext,Tuple<ObjectStateEntry, EntityState, bool> item );

        protected abstract void OnBeforeRelationship(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item);


        public void After(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            if (IsTargetEntity(item.Item1))
                OnAfter(dbContext, item);
            if (IsTargetRelationshipEntityAfter(item))
                OnAfterRelationship(dbContext, item);
        }

        protected abstract void OnAfter(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item);

        protected abstract void OnAfterRelationship(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item);
       

    }
}
