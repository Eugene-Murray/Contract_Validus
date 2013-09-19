using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Core.Data;
using Validus.Core.Tests.Data.Model;

namespace Validus.Core.Tests.Data
{
    [TestClass]
    public class SoftDeleteTest
    {

        [TestMethod]
        public void EntityDeleteTestForQuery()
        {
            using (var testRep = new TestRepository())
            {
                testRep.Database.Delete();
            }

            using (IRepository testRep = new TestRepository())
            {
                var personOne = new Person { Name = "PersonOne" };
                var personTwo = new Person { Name = "PersonTwo" };
                var personThree = new Person { Name = "PersonThree" };

                var addressHome = new Address
                {
                    Street = "HomeStreet",
                    Persons = new List<Person> { personOne, personTwo, personThree }
                };
                Assert.IsTrue(addressHome.Persons.Count == 3, "Soft delete test failed expectation");

                testRep.Add(addressHome);
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var personOne = testRep.Query<Person>(p => p.Name == "PersonOne").FirstOrDefault();
                Assert.IsNotNull(personOne, "Soft delete test failed expectation");
                testRep.Delete(personOne);
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var personOne = testRep.Query<Person>(p => p.Name == "PersonOne").FirstOrDefault();
                Assert.IsNull(personOne, "Soft delete test failed expectation");
            }
        }


        [TestMethod]
        public void EntityDeleteTestForEagerLoad()
        {
            using (var testRep = new TestRepository())
            {
                testRep.Database.Delete();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var personOne = new Person {Name = "PersonOne"};
                var personTwo = new Person { Name = "PersonTwo" };
                var personThree = new Person { Name = "PersonThree" };

                var addressHome = new Address
                    {
                        Street = "HomeStreet",
                        Persons = new List<Person> {personOne, personTwo, personThree}
                    };
                Assert.IsTrue(addressHome.Persons.Count == 3, "Soft delete test failed expectation");

                testRep.Add(addressHome);
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var personOne = testRep.Query<Person>(p => p.Name == "PersonOne").FirstOrDefault();
                var addressHome = testRep.Query<Address>(a => a.Street.Contains("HomeStreet"), a => a.Persons).FirstOrDefault();
                Assert.IsTrue(addressHome.Persons.Count == 3, "Soft delete test failed expectation");
                testRep.Delete(personOne);
                testRep.SaveChanges();
            }

            using (ITestRepository testRep = new TestRepository())
            {
                var addressHome = testRep.Query<Address>(a => a.Street.Contains("HomeStreet"), a => a.Persons).FirstOrDefault();
                if (addressHome == null) throw new Exception("Soft delete test failed, addressHome is null");
                Assert.IsTrue(addressHome.Persons.Count == 2, "Soft delete test failed expectation");
            }

        }


    }
}
