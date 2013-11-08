using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Validus.Core.Tests.Data
{
    [TestClass]
    public class TestRepositoryDbInitializer : DropCreateDatabaseAlways<TestRepository>
    {
        protected override void Seed(TestRepository context)
        {
            context.SaveChanges();
        }

        [AssemblyInitialize]
        public static void TestDbInitializer(TestContext context)
        {
            Database.SetInitializer(new TestRepositoryDbInitializer());
        }
    }


}
