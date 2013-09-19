using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.BusinessLogic;
using Validus.Console.Fakes;
using Validus.Console.SubscribeService;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Models;
using System.Security.Principal;
using Validus.Core.LogHandling;
using Validus.Console.Data;
using System.Data.Common;
using Effort.DataLoaders;
using Validus.Console.Data.DbInitializer;

namespace Validus.Console.Tests.Modules.Submission
{
    [TestClass]
    public class UserLinksUnitTests
    {
        ISubmissionModule _submissionModule;
        private IWebSiteModuleManager _webSiteModuleManager;
        
        [TestInitialize]
        public void Setup()
        {
            IConsoleRepository _rep = new ConsoleRepository();
            
            var mockSubscribeService = new Mock<IPolicyService>();
            mockSubscribeService.Setup(s => s.CreateQuote(It.IsAny<CreateQuoteRequest>()))
                                .Returns(new CreateQuoteResponse
                                {
                                    CreateQuoteResult = new StandardOutput { OutputXml = MvcMockHelpers.CreateQuoteResponseXml() },
                                    objInfoCollection = new InfoCollection { PolId = "BAN165118A13" }
                                });
            mockSubscribeService.Setup(s => s.GetReference(It.IsAny<GetReferenceRequest>()))
                                .Returns(new GetReferenceResponse
                                {
                                    GetReferenceResult =
                                        new StandardOutput { OutputXml = MvcMockHelpers.CreateGetReferenceResponseXml() }
                                });

            var mockPolicyData = new Mock<IPolicyData>();
            

            var context = new Mock<ICurrentHttpContext>();
            var user = @"talbotdev\MurrayE";
            //user = user.Replace(@"\\", @"\");
            context.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            context.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());

            _webSiteModuleManager = new WebSiteModuleManager(_rep, context.Object);
            _submissionModule = new SubmissionModule(_rep, mockSubscribeService.Object, new LogHandler(), context.Object, _webSiteModuleManager, mockPolicyData.Object);
            //_submissionModule = new SubmissionModule(rep, null, null, cont);
        }

        [TestMethod]
        public void Can_Get_CurrentUser()
        {
            //  Arrange
            String userid = @"talbotdev\MurrayE";

            //  Act
            //var uws = _rep.Query<Underwriter>();
            var currentUser = _webSiteModuleManager.EnsureCurrentUser();

            //  Assert
                //Assert.IsTrue(uws.Count() > 0);
            Assert.AreEqual(userid, currentUser.DomainLogon);
        }
    }
}
