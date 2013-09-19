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
using Validus.Models;

namespace Validus.Console.Tests.Modules.Submission
{
    [TestClass]
    public class SubmissionModuleHelpersIntegrationFixture : IntegrationFixtureBase
    {
        private static IUnityContainer _container;
        private static IConsoleRepository _consoleRepository;
        private static ILogHandler _logHandler;
        
        
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            GetMockSubscribeService();
            GetMockCurrentHttpContext();
            CreateBasicSubmission();
            CreateComplexSubmission();
            
            _container = new UnityContainer();
            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            _logHandler = _container.Resolve<LogHandler>();
            _consoleRepository = _container.Resolve<ConsoleRepository>();

            var mockCurrentHttpContext = new Mock<CurrentHttpContext>();
            var user = @"talbotdev\MurrayE";
            user = user.Replace(@"\\", @"\");
            mockCurrentHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            mockCurrentHttpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }
        
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void SetCopiedFrom_Success()
        {
            // Assign 
            var quote = new Quote();
            var submission = new Validus.Models.Submission();

            // Act 
            SubmissionModuleHelpers.SetCopiedFrom(quote, submission);

            // Assert
        }

        [TestMethod]
        public void CopyQuote_Success()
        {
            // Assign 
            var quote = new Quote {Id = 1, Comment = "TestComment"};

            // Act 
            var actualQuote = SubmissionModuleHelpers.CopyQuote(quote);

            // Assert
            Assert.AreEqual(quote.Id, actualQuote.Id);
            Assert.AreEqual(quote.Comment, actualQuote.Comment);
        }

        [TestMethod]
        public void UpdateSubscribeRecord_Success()
        {
            // Assign 
            var quote = new Quote();
            var submission = new Validus.Models.Submission();

            // Act 
            var actualUpdatePolicyResponse = SubmissionModuleHelpers.UpdateSubscribeRecord(quote, submission, _logHandler, _mockSubscribeService);

            // Assert
            Assert.IsNotNull(actualUpdatePolicyResponse);
        }

        // TODO: Subscribe has been mocked so how can this ever have worked?
        //[TestMethod]
        //[ExpectedException(typeof(Exception))]
        //public void UpdateSubscribeRecord_ExpectedException()
        //{
        //    // Assign 
        //    var quote = new Quote();
        //    var submission = new Validus.Models.Submission();

        //    // Act 
        //    var actualUpdatePolicyResponse = SubmissionModuleHelpers.UpdateSubscribeRecord(quote, submission, _logHandler, _mockSubscribeService);

        //    // Assert
        //}

        [TestMethod]
        public void CreateSubscribeRecord_Success()
        {
            // Assign 
            var quote = new Quote(); 
            var submission = new Validus.Models.Submission();
            var expectedPolId = "BAN165118A13";

            // Act 
            var actualResponse = SubmissionModuleHelpers.CreateSubscribeRecord(quote, submission, _logHandler, _mockSubscribeService);

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(actualResponse.objInfoCollection.PolId, expectedPolId);
        }

        // TODO: Subscribe has been mocked so how can this ever have worked?
        //[TestMethod]
        //[ExpectedException(typeof(Exception))]
        //public void CreateSubscribeRecord_ExpectException()
        //{
        //    // Assign 
        //    var quote = new Quote();
        //    var submission = new Validus.Models.Submission(); 

        //    // Act 
        //    SubmissionModuleHelpers.CreateSubscribeRecord(quote, submission, _logHandler, _mockSubscribeService);

        //    // Assert
        //}

        [TestMethod]
        public void ParseDetailedError_Success()
        {
            // Assign 
            var error = "This is an error...";

            // Act 
            var actualMessage = SubmissionModuleHelpers.ParseDetailedError(error);

            // Assert
            Assert.IsNotNull(actualMessage);
        }

        [TestMethod]
        public void QuoteValuesMatchSubscribePolicy_False()
        {
            // Assign 
            var quote = new Quote();
            var submission = new Validus.Models.Submission();
            var policyContract = new PolicyContract { InsdNm = "Insured Name...", BkrSeqId = 1, NonLonBkr = "AON", Uwr = "UC", Brokerage = 3, AccYr = "1976", ExpyDt = "19761229", IncpDt = "19761229", SettDueDt = "19761229" };

            // Act 
            var actualResult = SubmissionModuleHelpers.QuoteValuesMatchSubscribePolicy(policyContract, quote, submission);

            // Assert
            Assert.IsFalse(actualResult);
        }

        // TODO: set all properties correctly
        //[TestMethod]
        //public void QuoteValuesMatchSubscribePolicy_True()
        //{
        //    // Assign 
        //    var quote = new Quote { AccountYear = 1976 };
        //    var submission = new Validus.Models.Submission
        //        {
        //            BrokerSequenceId = 1,
        //            Brokerage = 3,
        //            InsuredName = "Insured Name...",
        //            NonLondonBrokerCode = "AON",
        //            UnderwriterCode = "UC"
        //        };
        //    var policyContract = new PolicyContract { InsdNm = "Insured Name...", BkrSeqId = 1, NonLonBkr = "AON", Uwr = "UC", Brokerage = 3, AccYr = "1976", ExpyDt = "19761229", IncpDt = "19761229", SettDueDt = "19761229" };

        //    // Act 
        //    var actualResult = SubmissionModuleHelpers.QuoteValuesMatchSubscribePolicy(policyContract, quote, submission);

        //    // Assert
        //    Assert.IsTrue(actualResult);
        //}

        [TestMethod]
        public void SynchroniseSubmission_Success()
        {
            // Assign
            var submission = new Validus.Models.Submission();
            var policyContract = new PolicyContract { InsdNm = "Insured Name...", BkrSeqId = 1, NonLonBkr = "AON", Uwr = "UC", Brokerage = 3, AccYr = "1976", ExpyDt = "19761229", IncpDt = "19761229", SettDueDt = "19761229" };

            // Act 
            SubmissionModuleHelpers.SynchroniseSubmission(submission, policyContract);

            // Assert
            Assert.AreEqual(submission.InsuredName, policyContract.InsdNm);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SynchroniseQuote_ExpectException()
        {
            // Assign 
            var quote = new Quote();
            var policyContract = new PolicyContract {AccYr = "29/12/1976"};

            // Act 
            SubmissionModuleHelpers.SynchroniseQuote(quote, policyContract);

            // Assert
            Assert.AreEqual(quote.AccountYear, policyContract.AccYr);
        }

        [TestMethod]
        public void SynchroniseQuote_Success()
        {
            // Assign 
            var quote = new Quote();
            var policyContract = new PolicyContract { AccYr = "1976", ExpyDt = "19761229", IncpDt = "19761229", SettDueDt = "19761229" };

            // Act 
            SubmissionModuleHelpers.SynchroniseQuote(quote, policyContract);

            // Assert
            Assert.AreEqual(quote.AccountYear.ToString(), policyContract.AccYr);
        }
    
    }
}
