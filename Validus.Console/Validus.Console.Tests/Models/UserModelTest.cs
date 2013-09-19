using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.Controllers;
using Validus.Models;

namespace Validus.Console.Tests.Models
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }

    [TestClass]
    public class UserModelTest
    {
        [TestMethod]
        public void CanCreateUser()
        {
            User u = new User();
            u.DomainLogon = @"global\baillief";

            Assert.IsNotNull(u);
        }
    }
}
