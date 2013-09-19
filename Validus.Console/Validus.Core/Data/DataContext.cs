using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using Validus.Core.Data.Interceptor;
using Validus.Core.Data.Interceptor.Interceptors;
using IsolationLevel = System.Data.IsolationLevel;

namespace Validus.Core.Data
{
    public class DataContext : DbContext, IRepository
    {
        public DataContext(string connectionName)
            : base(connectionName)
        {
            ObjectContext.ObjectMaterialized += ObjectContext_ObjectMaterialized;
            Interceptors.Add(new AuditChangeInterceptor());
            //Interceptors.Add(new AuditTrailChangeInterceptor());
            Interceptors.Add(new SoftDeleteChangeInterceptor());
            Interceptors.Add(new VersionChangeInterceptor());

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;            
        }

        public DataContext(DbConnection connection, Boolean contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            ObjectContext.ObjectMaterialized += ObjectContext_ObjectMaterialized;
            Interceptors.Add(new AuditChangeInterceptor());
            //Interceptors.Add(new AuditTrailChangeInterceptor());
            Interceptors.Add(new SoftDeleteChangeInterceptor());
            Interceptors.Add(new VersionChangeInterceptor());

            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        private void ObjectContext_ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            var delete = e.Entity as ISoftDelete;
            if (delete == null) return;
            if (delete.IsDeleted)
                ObjectContext.Detach(delete);
        }

        /// <summary>
        /// Retrieve the underlying ObjectContext
        /// </summary>
        private ObjectContext ObjectContext
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext;
            }
        }

        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();
        internal List<IInterceptor> Interceptors
        {
            get { return _interceptors; }
        }
        private void InterceptBefore(Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            Interceptors.ForEach(intercept => intercept.Before(this, item));
        }

        private void InterceptAfter(Tuple<ObjectStateEntry, EntityState, bool> item)
        {
            Interceptors.ForEach(intercept => intercept.After(this, item));
        }

        public override int SaveChanges()
        {
            const EntityState entitiesToTrack = EntityState.Added |
                                                EntityState.Modified |
                                                EntityState.Deleted;
            ObjectContext.DetectChanges();
            var elementsToSave =
                ObjectContext
                    .ObjectStateManager
                    .GetObjectStateEntries(entitiesToTrack).Select(ose => new Tuple<ObjectStateEntry, EntityState, bool>(ose, ose.State, ose.IsRelationship))
                    .ToList();
            //http://stackoverflow.com/questions/6028626/ef-code-first-dbcontext-and-transactions
            //http://petermeinl.wordpress.com/2011/03/13/avoiding-unwanted-escalation-to-distributed-transactions/
            using (var transactopnScope = new System.Transactions.TransactionScope(TransactionScopeOption.Required,
    new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                try
                {
                    elementsToSave.ForEach(InterceptBefore);
                    var result = base.SaveChanges();
                    elementsToSave.ForEach(InterceptAfter);
                    var resultAfter = base.SaveChanges();
                    transactopnScope.Complete();
                    return result;
                }
                catch (DbEntityValidationException e)
                {
                    //http://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert
                    var errormessage = new StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        errormessage.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        errormessage.AppendLine();
                        foreach (var ve in eve.ValidationErrors)
                        {
                            errormessage.AppendFormat("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                            errormessage.AppendLine();
                        }
                    }
                    throw new Exception(errormessage.ToString());
                }
            }
            //Debug.WriteLine(((AuditTrailChangeInterceptor) Interceptors.Single(i => i is AuditTrailChangeInterceptor)).AuditDump());
        }

        IQueryable<T> IRepository.Query<T>(params Expression<Func<T, object>>[] includes)
        {
            return (this as IRepository).Query(null, includes);
        }

        IQueryable<T> IRepository.Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = predicate != null ? Set<T>().Where(predicate).AsQueryable() : Set<T>().AsQueryable();

            if (typeof(T).GetInterfaces().Any(i => i.IsAssignableFrom(typeof(ISoftDelete))))
            {
                var softDeleteParam = Expression.Parameter(typeof(T));
                Expression aLeft = Expression.PropertyOrField(softDeleteParam, "IsDeleted");
                Expression aRight = Expression.Constant(false);
                Expression makeEqualsActive = Expression.Equal(aLeft, aRight);

                var predicateActiveRecordOnly = Expression.Lambda<Func<T, bool>>(makeEqualsActive, softDeleteParam);
                query = query.Where(predicateActiveRecordOnly);
            }
            //Entity.GroupBy(a => a.Key, (k,s) => s.OrderByDescending(c=> c.VersionNo).FirstOrDefault())
            if (typeof(T).GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IVersion))))
            {
                var versionObsoleteParam = Expression.Parameter(typeof(T));
                Expression aLeft = Expression.PropertyOrField(versionObsoleteParam, "IsObsolete");
                Expression aRight = Expression.Constant(false);
                Expression makeEqualsObsolete = Expression.Equal(aLeft, aRight);

                var predicateNewRecordOnly = Expression.Lambda<Func<T, bool>>(makeEqualsObsolete, versionObsoleteParam);
                query = query.Where(predicateNewRecordOnly);
            }
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        int IRepository.SaveChanges()
        {
            return SaveChanges();
        }

        T IRepository.Attach<T>(T entity)
        {
            var entry = Entry(entity);
            entry.State = System.Data.EntityState.Modified;
            return entity;
        }

        T IRepository.Add<T>(T entity)
        {
            return Set<T>().Add(entity);
        }

        void IRepository.AddOrUpdate<T>(T entity)
        {
            Set<T>().AddOrUpdate(entity);
            return;
        }

        T IRepository.Delete<T>(T entity)
        {
            return Set<T>().Remove(entity);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        T IRepository.AttachUnchanged<T>(T entity)
        {
            var entry = Entry(entity);
            Set<T>().Attach(entity);
            return entity;
        }
    }
}