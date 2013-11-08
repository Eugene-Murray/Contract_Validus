extern alias globalVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Validus.Console.Data;
using Validus.Console.Tests.Models;
using globalVM::Validus.Models;

namespace Validus.Console.Fakes
{
    public class FakeConsoleRepository : FakeRepository, IConsoleRepository
    {

        public IQueryable<Submission> Submissions
        {
            get { return _map.Get<Submission>().AsQueryable(); }
            set { _map.Use<Submission>(value); }
        }

        public IQueryable<User> Users
        {
            get { return _map.Get<User>().AsQueryable(); }
            set { _map.Use<User>(value); }
        }

        public IQueryable<Link> Links
        {
            get { return _map.Get<Link>().AsQueryable(); }
            set { _map.Use<Link>(value); }
        }
     

        //IQueryable<T> IConsoleRepository.Query<T>(Expression<Func<T, bool>> predicate, string[] eagerIncludeStrings)
        //{
        //    //DbQuery<T> query = Set<T>();

        //    //if (includeStrings != null && includeStrings.Length > 0)
        //    //{
        //    //    foreach (String item in includeStrings)
        //    //    {
        //    //        query = query.Include(item);
        //    //    }
        //    //}

        //    var query = _map.Get<T>().AsQueryable();
        //    if (predicate != null)
        //        return query.Where(predicate);
        //    else
        //        return query;                
        //}

      
    }
}