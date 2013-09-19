using System;
using System.Linq;
using System.Linq.Expressions;

namespace Validus.Core.Data
{
    public interface IRepository : IDisposable
    {

        IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : class;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        
        int SaveChanges();
        T Attach<T>(T entity) where T : class;
        T AttachUnchanged<T>(T entity) where T : class;
        T Add<T>(T entity) where T : class;
        void AddOrUpdate<T>(T entity) where T : class;
        T Delete<T>(T entity) where T : class;   
    }
}
