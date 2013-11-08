using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;

namespace Validus.Core.Data.Interceptor
{
    internal class ChangeInterceptor<T> : TypeInterceptor
    {
        public ChangeInterceptor()
            : base(typeof(T))
        {

        }

        #region Overrides of Interceptor

        protected override void OnBefore(DbContext dbContext,Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            var tItem = (T)item.Item1.Entity;
            switch (item.Item2)
            {
                case EntityState.Added:
                    OnBeforeInsert(dbContext, item.Item1.ObjectStateManager, tItem);
                    break;
                case EntityState.Deleted:
                    OnBeforeDelete(dbContext, item.Item1.ObjectStateManager, tItem);
                    break;
                case EntityState.Modified:
                    OnBeforeUpdate(dbContext, item.Item1.ObjectStateManager, tItem);
                    break;
            }
        }

        protected override void OnBeforeRelationship(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item)
        {

            if (item.Item1.State == EntityState.Added)
            {
                var oKey0 = (EntityKey)item.Item1.CurrentValues[0];
                var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                                                EntityState.Deleted | EntityState.Unchanged).Single(e => e.EntityKey == oKey0);

                var oKey1 = (EntityKey)item.Item1.CurrentValues[1];
                var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                                                EntityState.Deleted | EntityState.Unchanged).Single(e => e.EntityKey == oKey1);

                OnBeforeInsertRelationship(dbContext, item.Item1.ObjectStateManager, item.Item1, item0.Entity, item1.Entity);
            }


            if (item.Item1.State == EntityState.Deleted)
            {
                var oKey0 = (EntityKey)item.Item1.OriginalValues[0];
                var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                                                EntityState.Deleted | EntityState.Unchanged).Single(e => e.EntityKey == oKey0);

                var oKey1 = (EntityKey)item.Item1.OriginalValues[1];

                var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                                                EntityState.Deleted | EntityState.Unchanged).Single(e => e.EntityKey == oKey1);
                OnBeforeDeleteRelationship(dbContext, item.Item1.ObjectStateManager, item.Item1, item0.Entity, item1.Entity);
            }


        }

        protected override void OnAfter(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            var tItem = (T)item.Item1.Entity;
            switch (item.Item2)
            {
                case EntityState.Added:
                    OnAfterInsert(dbContext, item.Item1.ObjectStateManager, tItem);
                    break;
                case EntityState.Deleted:
                    OnAfterDelete(dbContext, item.Item1.ObjectStateManager, tItem);
                    break;
                case EntityState.Modified:
                    OnAfterUpdate(dbContext, item.Item1.ObjectStateManager, tItem);
                    break;
            }

        }

        protected override void OnAfterRelationship(DbContext dbContext, Tuple<ObjectStateEntry, EntityState, bool> item)
        {

            if (item.Item2 == EntityState.Added)
            {
                var oKey0 = (EntityKey)item.Item1.CurrentValues[0];
                var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Unchanged).Single(e => e.EntityKey == oKey0);

                var oKey1 = (EntityKey)item.Item1.CurrentValues[1];
                var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Unchanged).Single(e => e.EntityKey == oKey1);

                OnAfterInsertRelationship(dbContext, item.Item1.ObjectStateManager, item.Item1, item0.Entity, item1.Entity);
            }


            //if (item.Item2 == EntityState.Deleted)
            //{
            //    var oKey0 = (EntityKey)item.Item1.OriginalValues[0];
            //    var item0 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
            //                                                                    EntityState.Deleted).Single(e => e.EntityKey == oKey0);

            //    var oKey1 = (EntityKey)item.Item1.OriginalValues[1];

            //    var item1 = item.Item1.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
            //                                                                    EntityState.Deleted).Single(e => e.EntityKey == oKey1);
            //    OnAfterDeleteRelationship(item.Item1.ObjectStateManager, item.Item1, item0.Entity, item1.Entity);
            //}
        }

        #endregion

        public virtual void OnBeforeInsert(DbContext dbContext, ObjectStateManager manager, T item)
        {
        }

        public virtual void OnAfterInsert(DbContext dbContext, ObjectStateManager manager, T item)
        {
        }

        public virtual void OnBeforeUpdate(DbContext dbContext, ObjectStateManager manager, T item)
        {
        }

        public virtual void OnAfterUpdate(DbContext dbContext, ObjectStateManager manager, T item)
        {
        }

        public virtual void OnBeforeDelete(DbContext dbContext, ObjectStateManager manager, T item)
        {
        }

        public virtual void OnAfterDelete(DbContext dbContext, ObjectStateManager manager, T item)
        {
        }


        public virtual void OnBeforeInsertRelationship(DbContext dbContext, ObjectStateManager manager, Object relation, params Object[] item)
        {
        }

        public virtual void OnBeforeDeleteRelationship(DbContext dbContext, ObjectStateManager manager, Object relation, params Object[] item)
        {
        }

        public virtual void OnAfterInsertRelationship(DbContext dbContext, ObjectStateManager manager, Object relation, params Object[] item)
        {
        }

        //public virtual void OnAfterDeleteRelationship(ObjectStateManager manager, Object relation, params Object[] item)
        //{
        //}


    }
}
