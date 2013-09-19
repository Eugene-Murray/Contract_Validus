using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Core.Tests.Data.Model;

namespace Validus.Core.Tests.Data
{
    [TestClass]
    public class VersionTest
    {

        [TestMethod]
        public void EntityVesrsionTestWithAutoIncrement()
        {
            using (var testRep = new TestRepository())
            {
                testRep.Database.Delete();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                testRep.Add(new Address { Street = "HomeStreet" });
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet").FirstOrDefault();
                Assert.IsTrue(address != null && address.VersionNo == 1);
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet").FirstOrDefault();
                if (address != null)
                {
                    address.Street = "HomeStreet02";
                }
                testRep.SaveChanges();

                Assert.IsTrue(address != null && address.VersionNo == 2);
            }

            using (ITestRepository testRep = new TestRepository())
            {

                Assert.IsTrue(testRep.Query<Address>(a => a.Street.Contains("Street")).Count() == 1);
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet02").FirstOrDefault();
                Assert.IsTrue(address != null && address.VersionNo == 2);
            }
        }

        [TestMethod]
        public void EntityVesrsionTestWithRelatioship()
        {
            using (var testRep = new TestRepository())
            {
                testRep.Database.Delete();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                testRep.Add(new Address { Street = "HomeStreet" });
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet").FirstOrDefault();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet").FirstOrDefault();
                if (address != null)
                {
                    address.Street = "HomeStreet02";
                }
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                Assert.IsTrue(testRep.Query<Address>(a => a.Street.Contains("Street")).Count() == 1);
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet02").FirstOrDefault();
                address.Persons = new List<Person> { new Person { Name = "PersonOne" }, new Person { Name = "Persontwo" }, new Person { Name = "Personthree" } };
                testRep.SaveChanges();

            }
            using (ITestRepository testRep = new TestRepository())
            {
                Assert.IsTrue(testRep.Query<Address>(a => a.Street.Contains("Street")).Count() == 1);
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet02").FirstOrDefault();
                Assert.IsTrue(address != null && address.VersionNo == 2);
            }
        }


        [TestMethod]
        public void EntityVesrsionRetrivalInObjectGraph()
        {
            using (var testRep = new TestRepository())
            {
                testRep.Database.Delete();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                testRep.Add(new Address { Street = "HomeStreet" });
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet").FirstOrDefault();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet").FirstOrDefault();
                if (address != null)
                {
                    address.Street = "HomeStreet02";
                }
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                Assert.IsTrue(testRep.Query<Address>(a => a.Street.Contains("Street")).Count() == 1);
                var address = testRep.Query<Address>(a => a.Street == "HomeStreet02").FirstOrDefault();
                address.Persons = new List<Person> { new Person { Name = "PersonOne" }, new Person { Name = "Persontwo" }, new Person { Name = "Personthree" } };
                testRep.SaveChanges();

            }
            using (ITestRepository testRep = new TestRepository())
            {

                var address = testRep.Query<Address>(a => a.Street.Contains("Street")).FirstOrDefault();
                address.Street = "HomeStreet03";
                testRep.SaveChanges();
            }


            using (ITestRepository testRep = new TestRepository())
            {

                var person = testRep.Query<Person>(p => p.Name.Contains("PersonOne"), p => p.Address).FirstOrDefault();
                Assert.IsTrue(person.Address.Street == "HomeStreet02");
                testRep.SaveChanges();
            }

        }

    }
}
