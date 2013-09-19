using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Console.Data;
using Validus.Models;
using Validus.Core.HttpContext;
using Moq;
using System.Collections.Generic;
using System.Linq;


namespace Validus.Console.Tests.Modules.Admin
{
    [TestClass]
    public class AdminModuleWording
    {
        static Mock<ICurrentHttpContext> context;
        static int totalMarketWording = 0;
     

        [ClassInitialize]
        public static void Setup(TestContext t)
        {
            using (IConsoleRepository _rep = new ConsoleRepository())
            {
                 totalMarketWording = _rep.Query<MarketWording>(mw => true).ToList().Count;
                _rep.SaveChanges();
              
            }
        }


        [TestMethod]
        public void CheckSeedData()
        {
            using (IConsoleRepository _rep = new ConsoleRepository())
            {
                var tempTotalMarketWording = _rep.Query<MarketWording>(mw => true).ToList().Count;
                Assert.AreEqual(totalMarketWording, tempTotalMarketWording);


            }
        }
    }
}
