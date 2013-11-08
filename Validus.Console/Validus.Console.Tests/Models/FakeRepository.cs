using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.Fakes;
using Validus.Core.Data;

namespace Validus.Console.Tests.Models
{
    public class FakeRepository :IRepository
    {
      

         IQueryable<T> IRepository.Query<T>(params Expression<Func<T, object>>[] includes) 
        {
            return (this as IRepository).Query(null, includes);
        }

         IQueryable<T> IRepository.Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
         {
             var query = _map.Get<T>().AsQueryable();
             return predicate != null ? query.Where(predicate) : query;
         }

         IQueryable<T> IRepository.RawQuery<T>()
         {
             return _map.Get<T>().AsQueryable();
         }

        int IRepository.SaveChanges()
        {
            ChangesSaved = true;
            return 0;
        }

        public bool ChangesSaved { get; set; }

        void IDisposable.Dispose()
        {

        }

         T IRepository.Attach<T>(T entity)
        {
            _map.Get<T>().Add(entity);
            return entity;
        }

         T IRepository.Detach<T>(T entity)
         {
             return entity;
         }

         T IRepository.AttachUnchanged<T>(T entity) 
        {
            _map.Get<T>().Add(entity);
            return entity;
        }
        

         T IRepository.Add<T>(T entity)
        {
            _map.Get<T>().Add(entity);
            return entity;
        }

         void IRepository.AddOrUpdate<T>(T entity) 
        {
            _map.Get<T>().Add(entity);
            return;
        }

         T IRepository.Delete<T>(T entity)
        {
            _map.Get<T>().Remove(entity);
            return entity;
        }



        protected SetMap _map = new SetMap();

        protected class SetMap : KeyedCollection<Type, object>
        {
            public HashSet<T> Use<T>(IEnumerable<T> sourceData)
            {
                var set = new HashSet<T>(sourceData);
                if (Contains(typeof(T)))
                {
                    Remove(typeof(T));
                }
                Add(set);
                return set;
            }

            public HashSet<T> Get<T>()
            {
                return (HashSet<T>)this[typeof(T)];
            }

            protected override Type GetKeyForItem(object item)
            {
                return item.GetType().GetGenericArguments().Single();
            }
        }
    }
}
