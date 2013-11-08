using System;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.BusinessLogic;
using Validus.Console.Data;
using Validus.Console.SubscribeService;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using System.Linq;

namespace Validus.Console.Tests.Modules.Policy
{
    [Ignore] // TODO: mock out AppFabric Caching
    [TestClass]
    public class PolicyBusinessModuleIntegrationTestFixture
    {
        private static IPolicyBusinessModule PolicyBusinessModule;
        private static IUnityContainer Container;

        [TestInitialize]
        public void Init()
        {
            Container = new UnityContainer();
            Container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            Container.RegisterType<ILogHandler, LogHandler>();
            Container.RegisterType<IConsoleRepository, ConsoleRepository>();
            
            
            var mockCurrentHttpContext = new Mock<ICurrentHttpContext>();
            var user = @"talbotdev\MurrayE";
            user = user.Replace(@"\\", @"\");
            mockCurrentHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            mockCurrentHttpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());
            var currentHttpContext = mockCurrentHttpContext.Object;

            var mockSubscribeService = new Mock<IPolicyService>();
            IPolicyService subscribeService = mockSubscribeService.Object;

            IPolicyData policyData = Container.Resolve<PolicyData>(new ParameterOverride("currentHttpContext", currentHttpContext));
            IWebSiteModuleManager webSiteModuleManager = Container.Resolve<WebSiteModuleManager>(new ParameterOverride("currentHttpContext", currentHttpContext));

            PolicyBusinessModule = Container.Resolve<PolicyBusinessModule>(
                new ParameterOverride("policyData", policyData), 
                new ParameterOverride("subscribeService", subscribeService),
                new ParameterOverride("webSiteModuleManager", webSiteModuleManager));
        }

        [Ignore] // TODO: mock out AppFabric Caching
        [TestMethod]
        public void GetRenewalPoliciesDetailed_InsuredName_ApplyProfileFilters_Success()
        {
            // Assign
            DateTime expiryStartDate = DateTime.Now;
            DateTime expiryEndDate = DateTime.Now.AddMonths(1);
            string searchTerm = "COMMERZBANK AG";
            string sortCol = "ExpiryDate";
            string sortDir = "asc";
            int skip = 0;
            int take = 50; 
            bool applyProfileFilters = true;
            Int32 count = 0; 
            Int32 totalCount = 0;

            var expectedLength = 9;

            // Act
            var actualResult = PolicyBusinessModule.GetRenewalPoliciesDetailed(expiryStartDate, expiryEndDate, searchTerm, sortCol, sortDir, skip, take, applyProfileFilters, out count, out totalCount);

            // Assert
            Assert.AreEqual(expectedLength, actualResult.Length);
            Assert.IsTrue(actualResult.All(p => p.InsuredName == "COMMERZBANK AG"));
        }

        [Ignore] // TODO: mock out AppFabric Caching
        [TestMethod]
        public void GetRenewalPoliciesDetailed_Broker_ApplyProfileFilters_Success()
        {
            // Assign
            DateTime expiryStartDate = DateTime.Now;
            DateTime expiryEndDate = DateTime.Now.AddMonths(1);
            string searchTerm = "CTB 0509";
            string sortCol = "ExpiryDate";
            string sortDir = "asc";
            int skip = 0;
            int take = 50;
            bool applyProfileFilters = true;
            Int32 count = 0;
            Int32 totalCount = 0;

            var expectedLength = 50;

            // Act
            var actualResult = PolicyBusinessModule.GetRenewalPoliciesDetailed(expiryStartDate, expiryEndDate, searchTerm, sortCol, sortDir, skip, take, applyProfileFilters, out count, out totalCount);

            // Assert
            Assert.AreEqual(expectedLength, actualResult.Length);
            Assert.IsTrue(actualResult.All(p => p.Broker == "CTB 0509"));
        }

        [Ignore] // TODO: mock out AppFabric Caching
        [TestMethod]
        public void GetRenewalPoliciesDetailed_Broker_Success()
        {
            // Assign
            DateTime expiryStartDate = DateTime.Now;
            DateTime expiryEndDate = DateTime.Now.AddMonths(1);
            string searchTerm = "CTB 0509";
            string sortCol = "ExpiryDate";
            string sortDir = "asc";
            int skip = 0;
            int take = 50;
            bool applyProfileFilters = false;
            Int32 count = 0;
            Int32 totalCount = 0;

            var expectedLength = 50;

            // Act
            var actualResult = PolicyBusinessModule.GetRenewalPoliciesDetailed(expiryStartDate, expiryEndDate, searchTerm, sortCol, sortDir, skip, take, applyProfileFilters, out count, out totalCount);

            // Assert
            Assert.AreEqual(expectedLength, actualResult.Length);
            Assert.IsTrue(actualResult.All(p => p.Broker == "CTB 0509"));
        }
    }
}
