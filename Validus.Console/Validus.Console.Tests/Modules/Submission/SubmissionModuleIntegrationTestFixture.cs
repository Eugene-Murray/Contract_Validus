extern alias globalVM;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Moq;
using Validus.Console.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Validus.Console.DTO;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Console.Data.DbInitializer;
using System.Data.Entity;
using Validus.Console.Data;
using Validus.Console.SubscribeService;
using globalVM::Validus.Models;
using System.Runtime.Serialization;

namespace Validus.Console.Tests.Modules.Submission
{
    [TestClass]
    public class SubmissionModuleIntegrationTestFixture : IntegrationFixtureBase
    {
        private static IUnityContainer _container;
        private static ISubmissionModule _submissionModule;
        private static int _submissionId;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            GetMockSubscribeService();
            GetMockCurrentHttpContext();
            CreateBasicSubmission();
            CreateComplexSubmission();
            CreateBasicEnergySubmission();
            CreateComplexEnergySubmission();
            CreateBasicCargoSubmission();
            CreateComplexCargoSubmission();
            CreateBasicHullSubmission();
            CreateComplexHullSubmission();
            CreateBasicMarineSubmission();
            CreateComplexMarineSubmission();
                        
            _container = new UnityContainer();
            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            _container.RegisterType<ILogHandler, LogHandler>();
            _container.RegisterType<IConsoleRepository, ConsoleRepository>();
            _container.RegisterType<IWebSiteModuleManager, WebSiteModuleManager>();
            var mockPolicyData = new Mock<IPolicyData>();
            //_container.RegisterType<IPolicyData, PolicyData>();
            _container.RegisterInstance(typeof(IPolicyData), mockPolicyData.Object);
            var consoleRepository = _container.Resolve<IConsoleRepository>();

            SaveTestSubmission(consoleRepository);

            _submissionModule =
                _container.Resolve<SubmissionModule>(new ParameterOverride("currentHttpContext", _currentHttpContext),
                                                    new ParameterOverride("subscribeService", _mockSubscribeService));
        }

        private static void SaveTestSubmission(IConsoleRepository consoleRepository)
        {
            var submission = new globalVM::Validus.Models.SubmissionEN
                {
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now,
                    InsuredName = "- N/A",
                    BrokerCode = "1111",
                    BrokerPseudonym = "AAA",
                    BrokerSequenceId = 822,
                    InsuredId = 182396,
                    Brokerage = 1,
                    BrokerContact = "ALLAN MURRAY",
                   
                    UnderwriterCode = "AED",
                    UnderwriterContactCode = "JAC",
                    QuotingOfficeId = "LON",
                    Leader = "AG",
                    Domicile = "AD",
                    Title = "Unit Test Submission",
                    Options = new List<Option>
                        {
                            new Option
                                {
                                    Id = 1,
                                    Title = "Unit Test Submission",
                                    CreatedBy = "InitialSetup",
                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = "InitialSetup",
                                    ModifiedOn = DateTime.Now,
                                    OptionVersions = new List<OptionVersion>
                                        {
                                            new OptionVersion
                                                {
                                                    OptionId = 0,
                                                    VersionNumber = 0,
                                                    Comments = "OptionVersion Comments",
                                                    Title = "Unit Test Submission",
                                                    CreatedBy = "InitialSetup",
                                                    CreatedOn = DateTime.Now,
                                                    ModifiedBy = "InitialSetup",
                                                    ModifiedOn = DateTime.Now,
                                                    Quotes = new List<Quote>
                                                        {
                                                            new QuoteEN
                                                                {
                                                                    COBId = "AD",
                                                                    MOA = "FA",
                                                                    InceptionDate = DateTime.Now,
                                                                    ExpiryDate = DateTime.Now.AddMonths(12),
                                                                    QuoteExpiryDate = DateTime.Now,
                                                                    AccountYear = 2013,
                                                                    Currency = "USD",
                                                                    LimitCCY = "USD",
                                                                    ExcessCCY = "USD",
                                                                    CorrelationToken = Guid.NewGuid(),
                                                                    IsSubscribeMaster = true,
                                                                    PolicyType = "NONMARINE",
                                                                    EntryStatus = "PARTIAL",
                                                                    SubmissionStatus = "SUBMITTED",
                                                                    TechnicalPricingBindStatus = "PRE",
                                                                    TechnicalPricingPremiumPctgAmt = "AMT",
                                                                    TechnicalPricingMethod = "UW",
                                                                    CreatedBy = "InitialSetup",
                                                                    CreatedOn = DateTime.Now,
                                                                    ModifiedBy = "InitialSetup",
                                                                    ModifiedOn = DateTime.Now,
                                                                    OriginatingOfficeId = "LON",
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var savedSubmission = consoleRepository.Add(submission);
            consoleRepository.SaveChanges();

            _submissionId = savedSubmission.Id;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RemoveUnitTestData();
        }

        private static void RemoveUnitTestData()
        {
            using (IConsoleRepository repository = _container.Resolve<ConsoleRepository>())
            {
                var submissions =
                    repository.Query<globalVM::Validus.Models.Submission>(s => s.Title == "Unit Test Submission").ToList();

                if (submissions.Any())
                {
                    foreach (var submission in submissions)
                    {
                        repository.Delete(submission);
                        repository.SaveChanges();
                    }
                }
            }
        }

        [TestMethod]
        public void LoadXml()
        {
            Assert.IsNotNull(CreateQuoteResponseXml());
            Assert.IsNotNull(GetReferenceResponseXml());
            Assert.IsNotNull(UpdatePolicyResponseXml());
        }


        [TestMethod]
        public void GetSubmissionPreviewById_NoRecords_Success()
        {
            // Assign
            string sSearch = "Rage Against the Machine";
            int skip = 0;
            int take = 10;
            string sortCol = "Id";
            string sortDir = "desc";
            bool applyProfileFilters = true;
            int iTotalDisplayRecords;
            int iTotalRecords;

            // Act
            var actualSubmissionPreviewResult = _submissionModule.GetSubmissions(sSearch, skip, take, sortCol, sortDir,
                                                                                 applyProfileFilters,
                                                                                 out iTotalDisplayRecords,
                                                                                 out iTotalRecords, null);

            // Assert
            Assert.IsNotNull(actualSubmissionPreviewResult);
            Assert.AreEqual(0, iTotalDisplayRecords);
            Assert.AreEqual(0, iTotalRecords);
        }

        [Ignore]
        [TestMethod]
        public void GetSubmissionPreviewById_Success()
        {
            // Assign
            string sSearch = "AAA";
            int skip = 0;
            int take = 10;
            string sortCol = "Id";
            string sortDir = "desc";
            bool applyProfileFilters = true;
            int iTotalDisplayRecords;
            int iTotalRecords;
            var expectedTotal = 2;

            // Act
            var actualSubmissionPreviewResult = _submissionModule.GetSubmissions(sSearch, skip, take, sortCol, sortDir,
                                                                                 applyProfileFilters,
                                                                                 out iTotalDisplayRecords,
                                                                                 out iTotalRecords, null);

            // Assert
            Assert.IsNotNull(actualSubmissionPreviewResult);
            Assert.AreEqual(expectedTotal, iTotalDisplayRecords);
            Assert.AreEqual(expectedTotal, iTotalRecords);
        }

        [TestMethod]
        public void CreateBasicEnergySubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_basicEnergySubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }

        [TestMethod]
        public void CreateComplexEnergySubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_complexEnergySubmission, out subscribeErrors);


            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }

        //[TestMethod]
        //public void EditEnergySubmission_Success()
        //{
        //    // Assign
        //    var expectedErrorCount = 0;
        //    var errors = new List<String>();
        //    var quotes = new List<Quote>();
        //    var SubmissionEN = (SubmissionEN)_submissionModule.GetSubmissionById(_submissionId);
        //    SubmissionEN.Description = "Edit ES...";

        //    // Act
        //    List<String> subscribeErrors;
        //    _submissionModule.UpdateSubmission<SubmissionEN>(SubmissionEN, out errors, out quotes);

        //    // Assert
        //    var SubmissionENUpdated = (SubmissionEN)_submissionModule.GetSubmissionById(_submissionId);
        //    Assert.AreEqual(SubmissionEN.Description, SubmissionENUpdated.Description);
        //}

        [Ignore]
        [TestMethod]
        //[ExpectedException(typeof (DbEntityValidationException))]
        [ExpectedException(typeof(Exception))]
        public void CreateBasicSubmission_MissingValues_ReturnError()
        {
            // Assign
            var submission = new globalVM::Validus.Models.Submission
                {
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now,

                    InsuredName = "- N/A",
                    BrokerCode = "1111",
                    BrokerPseudonym = "AAA",
                    BrokerSequenceId = 822,
                    InsuredId = 182396,
                    Brokerage = 1,
                    BrokerContact = "ALLAN MURRAY",
                    UnderwriterCode = "AED",
                    UnderwriterContactCode = "JAC",
                    QuotingOfficeId = "LON",
                    Leader = "AG",
                    Domicile = "AD",
                    Title = "Unit Test Submission",
                    MarketWordingSettings = new List<MarketWordingSetting>(),
                    TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                    SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>(),
                    CustomMarketWordingSettings = new List<MarketWordingSetting>(),
                    CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>(),
                    CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                    Options = new List<Option>
                        {
                            new Option
                                {
                                    Id = 1,
                                    OptionVersions = new List<OptionVersion>
                                        {
                                            new OptionVersion
                                                {
                                                    OptionId = 0,
                                                    VersionNumber = 0,
                                                    Comments = "OptionVersion Comments",
                                                    Quotes = new List<Quote>
                                                        {
                                                            new Quote
                                                                {
                                                                    COBId = "BA",
                                                                    MOA = "FA",
                                                                    OriginatingOffice = new Office {Id = "DUB"},
                                                                    InceptionDate = DateTime.Now,
                                                                    QuoteExpiryDate = DateTime.Now,
                                                                    AccountYear = 2013,
                                                                    Currency = "USD",
                                                                    LimitCCY = "USD",
                                                                    ExcessCCY = "USD",
                                                                    CorrelationToken = Guid.NewGuid(),
                                                                    IsSubscribeMaster = true,
                                                                    PolicyType = "NONMARINE",
                                                                    EntryStatus = "PARTIAL",
                                                                    SubmissionStatus = "SUBMITTED"
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };

            // Act
            List<String> subscribeErrors;
            var actualSubmission = _submissionModule.CreateSubmission(submission, out subscribeErrors);

            // Assert

        }

        // TODO: Subscribe has been mocked so how can this ever have worked?
        //[TestMethod]
        //public void UpdateSubmission_ConcurrencyError_Fail()
        //{
        //    // Assign
        //    var expectedSubscribeErrors = 1;

        //    List<String> createSubscribeErrors;
        //    var submission = _submissionModule.CreateSubmission(_basicSubmission, out createSubscribeErrors);

        //    Assert.IsTrue(createSubscribeErrors.Count == 0);

        //    // Act
        //    List<String> subscribeErrors;
        //    List<Quote> userValues;
        //    _submissionModule.UpdateSubmission(_basicSubmission, out subscribeErrors, out userValues);

        //    // Assert
        //    Assert.AreEqual(subscribeErrors.Count, expectedSubscribeErrors);
        //}

        [TestMethod]
        public void GetSubmissionById_Success()
        {
            // Assign
            var submissionId = 1;
            var expectedTitle = "Seed Submission";

            // Act
            var actualSubmission = _submissionModule.GetSubmissionById(submissionId);

            // Assert
            Assert.AreEqual(actualSubmission.Title, expectedTitle);

        }

        [TestMethod]
        public void GetSubmissions_Success()
        {
            // Assign
            var sSearch = "";
            var iDisplayStart = 0;
            var iDisplayLength = 10;
            var sortCol = "Id";
            var sSortDir_0 = "0";
            var applyProfileFilters = true;
            var iTotalDisplayRecords = 0;
            var iTotalRecords = 0;

            // Act
            object[] actualSubmissions = _submissionModule.GetSubmissions(sSearch, iDisplayStart, iDisplayLength,
                                                                          sortCol, sSortDir_0, applyProfileFilters,
                                                                          out iTotalDisplayRecords, out iTotalRecords, null);

            // Assert
            Assert.IsNotNull(actualSubmissions);
        }

        [TestMethod]
        public void SaveEnergySubmissionAndEnergyQuote_Success()
        {
            // Assign
            var energySubmission = new globalVM::Validus.Models.SubmissionEN
            {
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now,
                InsuredName = "- N/A",
                BrokerCode = "1111",
                BrokerPseudonym = "AAA",
                BrokerSequenceId = 822,
                InsuredId = 182396,
                Brokerage = 1,
                BrokerContact = "ALLAN MURRAY",
                //Description = "Test Submission",
                UnderwriterCode = "AED",
                UnderwriterContactCode = "JAC",
                QuotingOfficeId = "LON",
                Leader = "AG",
                Domicile = "AD",
                Title = "Seed Submission",
                SubmissionTypeId = "EN",
                Options = new List<Option>{
                        new Option { 
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
                            Id = 1, 
                            Title = "Seed Submission",
                            OptionVersions = new List<OptionVersion>{
                                new OptionVersion { 
                                    OptionId = 0, 
                                    VersionNumber = 0, 
                                    Comments = "OptionVersion Comments", 
                                    Title = "Unit Test Submission", 
                                    CreatedBy = "InitialSetup",

                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = "InitialSetup",
                                    ModifiedOn = DateTime.Now,
                                    Quotes = new List<Quote>
                                        {
                                            new QuoteEN
                                            { 
                                                COBId = "AD", 
                                                MOA = "FA", 
                                                InceptionDate = DateTime.Now, 
                                                ExpiryDate = DateTime.Now.AddMonths(12), 
                                                QuoteExpiryDate = DateTime.Now, 
                                                AccountYear = 2013, 
                                                Currency = "USD", 
                                                LimitCCY = "USD", 
                                                ExcessCCY = "USD", 
                                                CorrelationToken = Guid.NewGuid(), 
                                                IsSubscribeMaster = true, 
                                                PolicyType = "NONMARINE", 
                                                EntryStatus = "PARTIAL", 
                                                SubmissionStatus = "SUBMITTED", 
                                                TechnicalPricingBindStatus = "PRE", 
                                                TechnicalPricingPremiumPctgAmt = "AMT", 
                                                TechnicalPricingMethod = "UW" ,
                                                OriginatingOfficeId = "LON",

                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now
                                            }
                                        }
                                }}
                        }}
            };

            // Act
            var consoleRepository = _container.Resolve<IConsoleRepository>();
            var actual = consoleRepository.Add(energySubmission);
            consoleRepository.SaveChanges();

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void SavePVSubmission_Success()
        {
            // Assign
            var energySubmission = new globalVM::Validus.Models.SubmissionPV
            {
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now,
                InsuredName = "- N/A",
                BrokerCode = "1111",
                BrokerPseudonym = "AAA",
                BrokerSequenceId = 822,
                InsuredId = 182396,
                Brokerage = 1,
                BrokerContact = "ALLAN MURRAY",
                //Description = "Test Submission",
                UnderwriterCode = "AED",
                UnderwriterContactCode = "JAC",
                QuotingOfficeId = "LON",
                Leader = "AG",
                Domicile = "AD",
                Title = "Seed Submission",
                SubmissionTypeId = "EN",
                //ExtraProperty1 = "Test Val 1",
                //ExtraProperty2 = "Test Val 2",
                //ExtraProperty3 = "Test Val 1",
                //ExtraProperty4 = "Test Val 2",
                Options = new List<Option>{
                        new Option { 
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
                            Id = 1, 
                            Title = "Seed Submission",
                            OptionVersions = new List<OptionVersion>{
                                new OptionVersion { 
                                    OptionId = 0, 
                                    VersionNumber = 0, 
                                    Comments = "OptionVersion Comments", 
                                    Title = "Unit Test Submission", 
                                    CreatedBy = "InitialSetup",

                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = "InitialSetup",
                                    ModifiedOn = DateTime.Now,
                                    Quotes = new List<Quote>
                                        {
                                            new Quote
                                            { 
                                                COBId = "AD", 
                                                MOA = "FA", 
                                                InceptionDate = DateTime.Now, 
                                                ExpiryDate = DateTime.Now.AddMonths(12), 
                                                QuoteExpiryDate = DateTime.Now, 
                                                AccountYear = 2013, 
                                                Currency = "USD", 
                                                LimitCCY = "USD", 
                                                ExcessCCY = "USD", 
                                                CorrelationToken = Guid.NewGuid(), 
                                                IsSubscribeMaster = true, 
                                                PolicyType = "NONMARINE", 
                                                EntryStatus = "PARTIAL", 
                                                SubmissionStatus = "SUBMITTED", 
                                                TechnicalPricingBindStatus = "PRE", 
                                                TechnicalPricingPremiumPctgAmt = "AMT", 
                                                TechnicalPricingMethod = "UW" ,
                                                OriginatingOfficeId = "LON",

                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now
                                            }
                                        }
                                }}
                        }}
            };

            // Act
            var consoleRepository = _container.Resolve<IConsoleRepository>();
            var actual = consoleRepository.Add(energySubmission);
            consoleRepository.SaveChanges();

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetEnergySubmissionById_Success()
        {
            // Assign 
            var submissionId = 3;
            var expectedQuoteType = "QuoteEN";

            // Act
            var actualSubmission = _submissionModule.GetSubmissionById(_submissionId);

            // Assert
            Assert.AreEqual(expectedQuoteType, actualSubmission.Options.FirstOrDefault().OptionVersions.FirstOrDefault().Quotes.FirstOrDefault().GetType().Name);
        }

        [Ignore]
        [TestMethod]
        public void CrossSellingCheck_Success()
        {
            // Assign 
            var expectedCount = 2;
            var insuredName = "- N/A";
            var thisSubmissionId = 0;

            // Act
            var actualCrossSellingListCount = _submissionModule.CrossSellingCheck(insuredName, thisSubmissionId).Count();

            // Assert
            Assert.AreEqual(expectedCount, actualCrossSellingListCount);
        }


        #region Submission cargo

        [TestMethod]
        public void CreateBasicCargoSubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_basicCargoSubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }

        [TestMethod]
        public void CreateComplexCargoSubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_complexCargoSubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }

        #endregion

        #region Submission Hull & Marine
        [TestMethod]
        public void CreateBasicHullSubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_basicHullSubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }

        [TestMethod]
        public void CreateComplexHullSubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_complexHullSubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }
        #endregion

        #region Submission Marine & Energy
        [TestMethod]
        public void CreateBasicMarineSubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_basicMarineSubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }

        [TestMethod]
        public void CreateComplexMarineSubmission_Success()
        {
            // Assign
            var expectedErrorCount = 0;

            // Act
            List<String> subscribeErrors;
            var submission = _submissionModule.CreateSubmission(_complexMarineSubmission, out subscribeErrors);

            // Assert
            Assert.AreEqual(expectedErrorCount, subscribeErrors.Count);
            Assert.AreNotEqual(0, submission.Id);
            Assert.AreNotEqual(-1, submission.Id);
        }
        #endregion
    }
}
