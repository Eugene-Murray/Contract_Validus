using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Console.Data;
using Validus.Console.Data.DbInitializer;
using Validus.Console.SubscribeService;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.Tests
{

    [TestClass]
    public class IntegrationFixtureBase
    {
        public static IPolicyService _mockSubscribeService;
        public static ICurrentHttpContext _currentHttpContext;
        public static Validus.Models.Submission _basicSubmission;
        public static Validus.Models.Submission _complexSubmission;
        public static Validus.Models.SubmissionEN _basicEnergySubmission;
        public static Validus.Models.SubmissionEN _complexEnergySubmission;

        [AssemblyInitialize]
        public static void SeedDb(TestContext context)
        {
            Database.SetInitializer(new TestConsoleDbInitializer());
        }

        public static string CreateQuoteResponseXml()
        {
            var doc = new XmlDocument();

            try
            {
                doc.Load(File.Exists(@"TestXml\CreateQuoteResponse.xml")
                         ? @"TestXml\CreateQuoteResponse.xml"
                         : "CreateQuoteResponse.xml");

                Assert.IsNotNull(doc.InnerXml, "innerxml is not null");
                //Assert.IsNotNull(doc.Value, doc.Value);

                if(string.IsNullOrEmpty(doc.InnerXml))
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                Debug.Print(ex.ToString());
                throw;
            }

            return doc.InnerXml;
        }

        public static string GetReferenceResponseXml()
        {
            var doc = new XmlDocument();

            try
            {
                doc.Load(File.Exists(@"TestXml\GetReferenceResponse.xml")
                         ? @"TestXml\GetReferenceResponse.xml"
                         : "GetReferenceResponse.xml");

                Assert.IsNotNull(doc.InnerXml, "innerxml is not null");

                if (string.IsNullOrEmpty(doc.InnerXml))
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                Debug.Print(ex.ToString());
                throw;
            }

            return doc.InnerXml;

        }

        public static string UpdatePolicyResponseXml()
        {


            var doc = new XmlDocument();

            try
            {
                doc.Load(File.Exists(@"TestXml\UpdatePolicyResponse.xml")
                         ? @"TestXml\UpdatePolicyResponse.xml"
                         : "UpdatePolicyResponse.xml");

                Assert.IsNotNull(doc.InnerXml, "innerxml is not null");

                if (string.IsNullOrEmpty(doc.InnerXml))
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                Debug.Print(ex.ToString());
                throw;
            }

            return doc.InnerXml;
        }

        public static void GetMockSubscribeService()
        {
            var mockSubscribeService = new Mock<IPolicyService>();
            
            mockSubscribeService.Setup(s => s.CreateQuote(It.IsAny<CreateQuoteRequest>()))
                                 .Returns(new CreateQuoteResponse
                                 {
                                     CreateQuoteResult = new StandardOutput { OutputXml = CreateQuoteResponseXml() },
                                     objInfoCollection = new InfoCollection { PolId = "BAN165118A13" }
                                 });
            mockSubscribeService.Setup(s => s.GetReference(It.IsAny<GetReferenceRequest>()))
                                 .Returns(new GetReferenceResponse
                                 {
                                     GetReferenceResult =
                                         new StandardOutput { OutputXml = GetReferenceResponseXml() }
                                 });
            mockSubscribeService.Setup(s => s.UpdatePolicy(It.IsAny<UpdatePolicyRequest>()))
                .Returns(new UpdatePolicyResponse
                    {
                        UpdatePolicyResult = new StandardOutput { OutputXml = UpdatePolicyResponseXml() },
                        objInfoCollection = new InfoCollection { PolId = "BAN165118A13" }
                    });

            _mockSubscribeService = mockSubscribeService.Object;
        }

        public static void GetMockCurrentHttpContext()
        {
            var mockCurrentHttpContext = new Mock<CurrentHttpContext>();
            var user = @"talbotdev\MurrayE";
            user = user.Replace(@"\\", @"\");
            mockCurrentHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            mockCurrentHttpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());
            _currentHttpContext = mockCurrentHttpContext.Object;
        }

        public static void CreateBasicSubmission()
        {
            _basicSubmission = new Validus.Models.Submission
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
                Description = "Unit Test Submission",
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
                Options = new List<Option>{
                        new Option 
                        { 
                            Id = 1, 
                            Title = "Unit Test Submission",
                            CreatedBy = "InitialSetup",
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
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
                                            CreatedBy = "InitialSetup",
                                            CreatedOn = DateTime.Now,
                                            ModifiedBy = "InitialSetup",
                                            ModifiedOn = DateTime.Now,
                                            OriginatingOfficeId = "LON"
                                            }
                                        }
                                }}
                        }}
            };
        }

        public static void CreateComplexSubmission()
        {
            _complexSubmission = new Validus.Models.Submission
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
                Description = "Unit Test Submission",
                UnderwriterCode = "AED",
                UnderwriterContactCode = "JAC",
                QuotingOfficeId = "LON",
                Leader = "AG",
                Domicile = "AD",
                Title = "Unit Test Submission",
                SubmissionTypeId = "EN",
                MarketWordingSettings = new List<MarketWordingSetting>(),
                TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>(),
                CustomMarketWordingSettings = new List<MarketWordingSetting>(),
                CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>(),
                CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                Options = new List<Option>{
                        new Option 
                        { 
                            Id = 1, 
                            Title = "Option 1 - Unit Test Submission",
                            CreatedBy = "InitialSetup",
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
                            OptionVersions = new List<OptionVersion>{
                                new OptionVersion { 
                                    OptionId = 0, 
                                    VersionNumber = 0, 
                                    Comments = "OptionVersion Comments", 
                                    Title = "OptionVersion 1 - Unit Test Submission", 
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            },
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            }
                                        }
                                }}
                        }, 
                        new Option 
                        { 
                            Id = 2, 
                            Title = "Option 2 - Unit Test Submission",
                            CreatedBy = "InitialSetup",
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
                            OptionVersions = new List<OptionVersion>{
                                new OptionVersion { 
                                    OptionId = 0, 
                                    VersionNumber = 0, 
                                    Comments = "OptionVersion Comments", 
                                    Title = "OptionVersion 2 - Unit Test Submission", 
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            },
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            },
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            }

                                        }
                                }
                            }
                        }
                }
            };
        }

        public static void CreateBasicEnergySubmission()
        {
            _basicEnergySubmission = new Validus.Models.SubmissionEN
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
                Description = "Unit Test Submission",
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
                Options = new List<Option>{
                        new Option 
                        { 
                            Id = 1, 
                            Title = "Unit Test Submission",
                            CreatedBy = "InitialSetup",
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
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
                                            CreatedBy = "InitialSetup",
                                            CreatedOn = DateTime.Now,
                                            ModifiedBy = "InitialSetup",
                                            ModifiedOn = DateTime.Now,
                                            OriginatingOfficeId = "LON",
                                          }
                                        }
                                }}
                        }}
            };
        }

        public static void CreateComplexEnergySubmission()
        {
            _complexEnergySubmission = new Validus.Models.SubmissionEN
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
                Description = "Unit Test Submission",
                UnderwriterCode = "AED",
                UnderwriterContactCode = "JAC",
                QuotingOfficeId = "LON",
                Leader = "AG",
                Domicile = "AD",
                Title = "Unit Test Submission",
                SubmissionTypeId = "EN",
                MarketWordingSettings = new List<MarketWordingSetting>(),
                TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>(),
                CustomMarketWordingSettings = new List<MarketWordingSetting>(),
                CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>(),
                CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                Options = new List<Option>{
                        new Option 
                        { 
                            Id = 1, 
                            Title = "Option 1 - Unit Test Submission",
                            CreatedBy = "InitialSetup",
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
                            OptionVersions = new List<OptionVersion>{
                                new OptionVersion { 
                                    OptionId = 0, 
                                    VersionNumber = 0, 
                                    Comments = "OptionVersion Comments", 
                                    Title = "OptionVersion 1 - Unit Test Submission", 
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            },
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON"
                                            }
                                        }
                                }}
                        }, 
                        new Option 
                        { 
                            Id = 2, 
                            Title = "Option 2 - Unit Test Submission",
                            CreatedBy = "InitialSetup",
                            CreatedOn = DateTime.Now,
                            ModifiedBy = "InitialSetup",
                            ModifiedOn = DateTime.Now,
                            OptionVersions = new List<OptionVersion>{
                                new OptionVersion { 
                                    OptionId = 0, 
                                    VersionNumber = 0, 
                                    Comments = "OptionVersion Comments", 
                                    Title = "OptionVersion 2 - Unit Test Submission", 
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON",
                                            },
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
                                                CreatedBy = "InitialSetup",
                                                CreatedOn = DateTime.Now,
                                                ModifiedBy = "InitialSetup",
                                                ModifiedOn = DateTime.Now,
                                                OriginatingOfficeId = "LON",
                                            },
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
        }
    }
}
