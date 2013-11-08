using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Console.Data;
using Validus.Console.Tests.Helpers;
using Validus.Models;
using System.Linq;

namespace Validus.Console.UiTests.Helper
{
    [TestClass]
    public class DefaultTest
    {
        private static int _initCount;

        [AssemblyInitialize]
        public static void SetupIntegrationTests(TestContext context)
        {
            Database.SetInitializer(new TestConsoleDbInitializer());
            if (_initCount++ == 0)
            {
                new ConsoleRepository().Set<User>();//.FirstOrDefault(u => u.DomainLogon == "aa");
            }
        }

        [AssemblyCleanup]
        public static void TeardownIntegrationTests()
        {
            if (--_initCount == 0)
            {
                
            }
        }

    }
}
