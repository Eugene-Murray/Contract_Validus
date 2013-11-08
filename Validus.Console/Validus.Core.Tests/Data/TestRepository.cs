using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data;
using Validus.Core.Tests.Data.Model;


namespace Validus.Core.Tests.Data
{
    public class TestRepository : DataContext, ITestRepository
    {
       

        public TestRepository() : base("name=DatabaseContext"){}
        public TestRepository(DbConnection connection) : base(connection, true){}
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Office> Offices { get; set; }

    }
}
