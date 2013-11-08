using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Console.SubscribeService;
using Validus.Core.LogHandling;

namespace Validus.Console.Tests.SubscribeService
{
    
    [TestClass]
    public class SubscribeServiceIntegrationFixture
    {
        private string _subscribeReference;
        private static IUnityContainer _container;
        private static ILogHandler _logHandler;
        private static string _logFileLocation;

        [TestInitialize]
        public void Init()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            _logHandler = _container.Resolve<LogHandler>();

            // Note: this path is also set in "Rolling Flat File Trace Listener" section
            _logFileLocation = ConfigurationManager.AppSettings["logFileLocation"].ToString();
        }
        
        [TestMethod]
        public void LoadXml()
        {
            Assert.IsNotNull(IntegrationFixtureBase.CreateQuoteResponseXml());
            var xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(IntegrationFixtureBase.CreateQuoteResponseXml());
            Assert.AreEqual("BAN165118A13", xmlDoc1.GetElementsByTagName("PolId")[0].InnerText);


            Assert.IsNotNull(IntegrationFixtureBase.GetReferenceResponseXml());
            var xmlDoc2 = new XmlDocument();
            xmlDoc2.LoadXml(IntegrationFixtureBase.GetReferenceResponseXml());
            Assert.AreEqual("ALLAN MURRAY", xmlDoc2.GetElementsByTagName("CtcNm")[0].InnerText);


            Assert.IsNotNull(IntegrationFixtureBase.UpdatePolicyResponseXml());
            var xmlDoc3 = new XmlDocument();
            xmlDoc3.LoadXml(IntegrationFixtureBase.UpdatePolicyResponseXml());
            Assert.AreEqual("AGY", xmlDoc3.GetElementsByTagName("BusinessUnit")[0].InnerText);
        }

        [Ignore]
        [TestMethod]
        public void CreateQuote_Success()
        {
            // Assign
            long actualTimeStamp = 0; 

            IPolicyService subscribeService = new PolicyServiceClient();
            var policyContract = new PolicyContract
            {
                UnitPsu = "AGY",
                PolTy = "NONMARINE",
                EntSt = "PARTIAL",
                InsdNm= "- N/A",
                InsdId= 182396,
                PolDsc= "Unit Test Submission",
                Uwr= "AED",
                DOM= "AD",
                BkrSeqId= 822,
                CtcNm= "ALLAN MURRAY",
                IncpDt= "20130628",
                ExpyDt= "20140628",
                AccYr= "2013",
                SettDueDt= "20130628",
                Brokerage= 1,
                DeclineReason= null,
                COB= "AD",
                MOA= "FA",
                NonLonBkr= null, 
                OrigOff= "LON",
                FacyPolId= null,
                RenPolId= null,
                PolId= null,
                LmtCcy= "USD",
                LmtAmt= null,
                ExsCcy= "USD",
                ExsAmt= null,
                BindSt= "PRE",
                PctgAmt= "AMT",
                PricingCcy= "USD",
                Method= "UW",
                TechPrm= null,
                BenchPrm= null,
                Status= "SUBMITTED",
                TimeStamp= null,
                BkrPsu= "AAA",
                BkrNo= "???",
                TechPmTy= "TechPm"
            };
            var request = new CreateQuoteRequest { objPolicyContract = policyContract };

            // Act
            var actualResponse = subscribeService.CreateQuote(request);

            if (actualResponse.CreateQuoteResult.ErrorInfo != null)
            {
                _logHandler.WriteLog(actualResponse.CreateQuoteResult.ErrorInfo.Description, LogSeverity.Error,
                                     LogCategory.BusinessComponent);
                _logHandler.WriteLog(actualResponse.CreateQuoteResult.ErrorInfo.ErrorXML, LogSeverity.Error,
                                     LogCategory.BusinessComponent);
                _logHandler.WriteLog(actualResponse.CreateQuoteResult.ErrorInfo.DetailedDescription, LogSeverity.Error,
                                     LogCategory.BusinessComponent);
                _logHandler.WriteLog(actualResponse.CreateQuoteResult.ErrorInfo.Severity.ToString(), LogSeverity.Error,
                                     LogCategory.BusinessComponent);
            }

            var doc = new XmlDocument();
            doc.LoadXml(actualResponse.CreateQuoteResult.OutputXml);

            _subscribeReference = doc.GetElementsByTagName("PolId")[0].InnerText;

            long subscribeTimestamp;
            actualTimeStamp = long.TryParse(doc.GetElementsByTagName("TimeStamp")[0].InnerText, out subscribeTimestamp) ?
                subscribeTimestamp : 0;

            // Assert
            Assert.IsNull(actualResponse.CreateQuoteResult.ErrorInfo);
            Assert.AreNotEqual(actualTimeStamp, 0);
            Assert.IsNotNull(_subscribeReference);
        }

        
        [TestMethod]
        [ExpectedException(typeof(CommunicationException))]
        public void CreateQuote_ExpectedException()
        {
            // Assign
            long actualTimeStamp = 0;

            IPolicyService subscribeService = new PolicyServiceClient();
            var policyContract = new PolicyContract();
            
            var request = new CreateQuoteRequest { objPolicyContract = policyContract };

            // Act
            var actualResponse = subscribeService.CreateQuote(request);

            // Assert
            Assert.IsNotNull(actualResponse.CreateQuoteResult.ErrorInfo);
        }

        
        [TestMethod]
        public void CreateQuote_Fail()
        {
            // Assign

            IPolicyService subscribeService = new PolicyServiceClient();
            var policyContract = new PolicyContract
                {
                    UnitPsu = "",
                    PolTy = "",
                    EntSt = "",
                    InsdNm = "",
                    InsdId = 0,
                    PolDsc = "",
                    Uwr = "",
                    DOM = "",
                    BkrSeqId = 0,
                    CtcNm = "",
                    IncpDt = "",
                    ExpyDt = "",
                    AccYr = "",
                    SettDueDt = "",
                    Brokerage = 0,
                    DeclineReason = "",
                    COB = "",
                    MOA = "",
                    NonLonBkr = "",
                    OrigOff = "",
                    FacyPolId = "",
                    RenPolId = "",
                    PolId = "",
                    LmtCcy = "",
                    LmtAmt = null,
                    ExsCcy = "",
                    ExsAmt = null,
                    BindSt = "",
                    PctgAmt = "",
                    PricingCcy = "",
                    Method = "",
                    TechPrm = null,
                    BenchPrm = null,
                    Status = "",
                    TimeStamp = null,
                    BkrPsu = "",
                    BkrNo = "",
                    TechPmTy = ""
                };

            var request = new CreateQuoteRequest { objPolicyContract = policyContract };

            // Act
            var actualResponse = subscribeService.CreateQuote(request);

            // Assert
            Assert.IsNotNull(actualResponse.CreateQuoteResult.ErrorInfo);
        }

        
        [TestMethod]
        public void UpdatePolicy_Fail()
        {
            // Assign
            IPolicyService subscribeService = new PolicyServiceClient();

            var policyContract = new PolicyContract
            {
                UnitPsu = "AGY",
                PolTy = "NONMARINE",
                EntSt = "PARTIAL",
                InsdNm = "- N/A",
                InsdId = 182396,
                PolDsc = "Unit Test Submission",
                Uwr = "AED",
                DOM = "AD",
                BkrSeqId = 822,
                CtcNm = "ALLAN MURRAY",
                IncpDt = "20130628",
                ExpyDt = "20140628",
                AccYr = "2013",
                SettDueDt = "20130628",
                Brokerage = 1,
                DeclineReason = null,
                COB = "AD",
                MOA = "FA",
                NonLonBkr = null,
                OrigOff = "LON",
                FacyPolId = null,
                RenPolId = null,
                PolId = null,
                LmtCcy = "USD",
                LmtAmt = null,
                ExsCcy = "USD",
                ExsAmt = null,
                BindSt = "PRE",
                PctgAmt = "AMT",
                PricingCcy = "USD",
                Method = "UW",
                TechPrm = null,
                BenchPrm = null,
                Status = "SUBMITTED",
                TimeStamp = null,
                BkrPsu = "AAA",
                BkrNo = "???",
                TechPmTy = "TechPm"
            };

            var updatePolicyRequest = new UpdatePolicyRequest { objPolicyContract = policyContract };

            // Act
            var actualResponse = subscribeService.UpdatePolicy(updatePolicyRequest);

            // Assert
            Assert.IsNotNull(actualResponse.UpdatePolicyResult.ErrorInfo);
        }

        
        [TestMethod]
        public void GetReference_ErrorReturned()
        {
            // Assign
            IPolicyService subscribeService = new PolicyServiceClient();
            var getReferenceRequest = new GetReferenceRequest();

            // Act
            var actualGetReferenceResponse = subscribeService.GetReference(getReferenceRequest);

            // Assert
            Assert.IsNotNull(actualGetReferenceResponse.GetReferenceResult.ErrorInfo);
        }

        [Ignore]
        [TestMethod]
        public void GetReference_Success()
        {
            // Assign
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(IntegrationFixtureBase.GetReferenceResponseXml());
            var expectedResponsePolicyType = xmlDoc.GetElementsByTagName("PolId")[0].InnerText;
            
            IPolicyService subscribeService = new PolicyServiceClient();
            var getReferenceRequest = new GetReferenceRequest {strPolId = "ADF169034A13" };

            // Act
            var actualResponse = subscribeService.GetReference(getReferenceRequest);

            if (actualResponse.GetReferenceResult.ErrorInfo != null)
            {
                _logHandler.WriteLog(actualResponse.GetReferenceResult.ErrorInfo.ErrorXML, LogSeverity.Error,
                                     LogCategory.BusinessComponent);
                _logHandler.WriteLog(actualResponse.GetReferenceResult.ErrorInfo.Description, LogSeverity.Error,
                                     LogCategory.BusinessComponent);
                _logHandler.WriteLog(actualResponse.GetReferenceResult.ErrorInfo.DetailedDescription, LogSeverity.Error,
                                     LogCategory.BusinessComponent);
            }

            Assert.IsNotNull(actualResponse.GetReferenceResult.OutputXml);

            var xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(actualResponse.GetReferenceResult.OutputXml);

            Assert.AreEqual(expectedResponsePolicyType, xmlDoc1.GetElementsByTagName("PolId")[0].InnerText);
        }

    }
}
