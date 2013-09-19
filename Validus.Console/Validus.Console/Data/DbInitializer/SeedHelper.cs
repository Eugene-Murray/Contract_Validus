using System;
using System.Collections.Generic;
using System.Linq;
using Validus.Models;

namespace Validus.Console.Data.DbInitializer
{
    public static class SeedHelper
    {
        public static void SeedData(IConsoleRepository context)
        {
            var domainPrefix = global::Validus.Console.Properties.Settings.Default.DomainPrefix;

            var AG = new COB { Id = "AG", Narrative = "Direct - Property - Construction", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.AddOrUpdate<COB>(AG);
            var AR = new COB { Id = "AR", Narrative = "Direct - Property - Rig", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.AddOrUpdate<COB>(AR);
            var AT = new COB { Id = "AT", Narrative = "Direct - Property - Hull", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.AddOrUpdate<COB>(AT);
            var CC = new COB { Id = "CC", Narrative = "Direct - Casualty - Construction", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.AddOrUpdate<COB>(CC);


            var c = new COB { Id = "CA", Narrative = "Cargo", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.AddOrUpdate<COB>(c);

            var c1 = new COB { Id = "AD", Narrative = "Direct - Property - Contingency", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.AddOrUpdate<COB>(c1);

            var userWriter1 = new Underwriter
            {
                Name = "Alex Colquhoun",
                Code = "JAC",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.AddOrUpdate<Underwriter>(userWriter1);

            var userWriter2 = new Underwriter
            {
                Name = "Alexandra Davies",
                Code = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.AddOrUpdate<Underwriter>(userWriter2);

            var underWriter = new User
            {
                DomainLogon = domainPrefix + @"\defaultunderwriter",
                IsActive = true,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now,
                UnderwriterCode = "AED"
            };
            context.AddOrUpdate<User>(underWriter);

            var quoteSheetPerson = new User
            {
                DomainLogon = domainPrefix + @"\quoteSheetPerson",
                IsActive = true,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now,
                UnderwriterCode = "AED"
            };
            context.AddOrUpdate<User>(quoteSheetPerson);

            // Note - SubmissionStatus - SUBMITTED, QUOTED, FIRM ORDER, DECLINED, WITHDRAWN, ADDL INFO REQST, FIRM ORDER REQST, OTHER OFFICE
            var masterQuote = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "ADC118440A110006", //"AJH105451C12", // Policy Ref
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 25000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "ADC118440A110006",
                SubmissionStatus = "QUOTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 60000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = false,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q3 = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJE125510B12",
                SubmissionStatus = "DECLINED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 360000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = false,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q4master = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJE125510B12",
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 100000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q5 = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJE125510B12",
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 1000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = false,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                // OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q6master = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJY087986D12",
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 890000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q7master = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJJ144268A12",
                SubmissionStatus = "ADDL INFO REQST",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 39000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };


            var quoteSheet1 = new QuoteSheet { Title = "QS_1", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-1), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet2 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-2), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet3 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-3), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet4 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-4), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet5 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-5), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet6 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-6), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet7 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-7), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet8 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-8), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            var ov = new OptionVersion
            {
                Title = "Version 1",
                VersionNumber = 1,
                Quotes = new List<Quote> { q, masterQuote },
                QuoteSheets =
                    new List<QuoteSheet>
                                {
                                    quoteSheet1,
                                    quoteSheet2,
                                    quoteSheet3,
                                    quoteSheet4,
                                    quoteSheet5,
                                    quoteSheet6
                                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var ov2 = new OptionVersion
            {
                Title = "Version 2",
                VersionNumber = 2,
                Quotes = new List<Quote> { q3, q4master, q5 },
                QuoteSheets = new List<QuoteSheet> { quoteSheet7, quoteSheet8 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };


            var ov3 = new OptionVersion
            {
                Title = "Version 3",
                VersionNumber = 1,
                Quotes = new List<Quote> { q6master },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };


            var ov4 = new OptionVersion
            {
                Title = "Version 4",
                VersionNumber = 1,
                Quotes = new List<Quote> { q7master },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var o1 = new Option
            {
                Title = "Option 1",
                Comments =
                    "There is a known issue with this technique where creating databases is not supported on SqlClient provider. Other providers may or may not support this functionality depending on implementation. In general, because of that it is recommended to use unwrapped connections when using DDL APIs (CreateDatabase, DeleteDatabase, DatabaseExists()) as demonstrated in the sample.",
                OptionVersions = new List<OptionVersion> { ov, ov2 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var o2 = new Option
            {
                Title = "Option 2",
                Comments =
                    "In order to efficiently manage tracing for the application we need to create a central factory class which will create ObjectContext instances for us. This is the place where we will create tracing provider connection and use it to instantiate ObjectContext. Assuming our Object Context class is called MyContainer, the factory class will be called MyContainerFactory and will have a method called CreateContext, so the usage becomes:",
                OptionVersions = new List<OptionVersion> { ov3 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var o3 = new Option
            {
                Title = "Option 3",
                Comments =
                    "Entity Framework/Code First feature released as part of Feature CTP 3 can work with any EF-enabled data provider. In addition to regular providers which target databases, it is possible to use wrapping providers which can add interesting functionality, such as caching and tracing. In this post I’m going to explain how to use EFTracingProvider to produce diagnostic trace of all SQL commands executed by EF in Code First.",
                OptionVersions = new List<OptionVersion> { ov4 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var lon = new Office
            {
                Id = "LON",
                Name = "London",
                Title = "Talbot Underwriting Ltd",
                Footer = "Talbot Underwriting Ltd is Authorised by the Prudential Regulation Authority and regulated by the Financial Conduct Authority and the Prudential Regulation Authority.",
                Address = new Address
                {
                    AddressLine1 = "60 Threadneedle Street",
                    AddressLine2 = "",
                    City = "London",
                    ZipPostalCode = "EC2R 8HP",
                    Country = "England",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var mia = new Office
            {
                Id = "MIA",
                Name = "Miami",
                Title = "Validus Reaseguros, Inc.",
                Address = new Address
                {
                    AddressLine1 = "2601 South Bayshore Drive",
                    AddressLine2 = "Suite 1850, Coconut Grove",
                    City = "Miami",
                    StateProvinceRegion = "Florida",
                    ZipPostalCode = "33133",
                    Country = "U.S.A.",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var lab = new Office
            {
                Id = "LAB",
                Name = "Labuan",
                Title = "Talbot Risk Services (Labuan) Pte Ltd",
                Address = new Address
                {
                    AddressLine1 = "Brighton Place, Ground Floor,",
                    AddressLine2 = "No. U0215, Jalan Bahasa,",
                    City = "Labuan FT",
                    Country = "Malaysia",
                    ZipPostalCode = "87000",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var nyc = new Office
            {
                Id = "NYC",
                Name = "New York",
                Title = "Talbot Underwriting (US) Ltd. ",
                Address = new Address
                {
                    AddressLine1 = "48 Wall Street 17th Floor",
                    AddressLine2 = "",
                    Country = "USA",
                    StateProvinceRegion = "NY",
                    City = "New York",
                    ZipPostalCode = "10005",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var sng = new Office
            {
                Id = "SNG",
                Name = "Singapore",
                Title = "Talbot Risk Services Pte Ltd",
                Address = new Address
                {
                    AddressLine1 = "8 Marina View #14-01",
                    AddressLine2 = "Asia Square Tower 1",
                    City = "Singapore",
                    ZipPostalCode = "018960",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var dubai = new Office
            {
                Id = "DUB",
                Name = "Dubai",
                Title = "Talbot Underwriting (MENA) Ltd",
                Footer = "Underwriting Risk Services (Middle East) Ltd ('URSME') is authorised and regulated by the Dubai Financial Services Authority.",
                Address = new Address
                {
                    AddressLine1 = "Dubai International Financial Centre",
                    AddressLine2 = "Gate Village Building 10, Level 5",
                    City = "Dubai",
                    Country = "UAE",
                    ZipPostalCode = "PO Box 506809",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var usrl = new Office
            {
                Id = "URS",
                Name = "URSL",
                Title = "Talbot Underwriting Risk Services Ltd",
                Footer = "Talbot Underwriting Risk Services Ltd is an appointed representative of Talbot Underwriting Ltd which is Authorised by the Prudential Regulation Authority and regulated by the Financial Conduct Authority and the Prudential Regulation Authority.",
                Address = new Address
                {
                    AddressLine1 = "60 Threadneedle Street",
                    AddressLine2 = "",
                    City = "London",
                    Country = "England",
                    ZipPostalCode = "EC2R 8HP",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            context.AddOrUpdate<Office>(lon);
            context.AddOrUpdate<Office>(mia);
            context.AddOrUpdate<Office>(lab);
            context.AddOrUpdate<Office>(nyc);
            context.AddOrUpdate<Office>(sng);
            context.AddOrUpdate<Office>(dubai);
            context.AddOrUpdate<Office>(usrl);

            //var userWriter1 = new Underwriter
            //{
            //    Name = "Alex Colquhoun",
            //    Code = "JAC",
            //    CreatedBy = "InitialSetup",
            //    CreatedOn = DateTime.Now,
            //    ModifiedBy = "InitialSetup",
            //    ModifiedOn = DateTime.Now
            //};
            //context.AddOrUpdate<Underwriter>(userWriter1);

            //userWriter2 = new Underwriter
            //{
            //    Name = "Alexandra Davies",
            //    Code = "AED",
            //    CreatedBy = "InitialSetup",
            //    CreatedOn = DateTime.Now,
            //    ModifiedBy = "InitialSetup",
            //    ModifiedOn = DateTime.Now
            //};
            //context.AddOrUpdate<Underwriter>(userWriter2);


            var submissionTypes = new List<SubmissionType>
                {
                    new SubmissionType { Id = "EN", Title = "EN Submission" },
                    new SubmissionType { Id = "PV", Title = "PV Submission" },
                    new SubmissionType { Id = "FI", Title = "FI Submission" }
                };

            foreach (var submissionType in submissionTypes)
            {
                context.AddOrUpdate<SubmissionType>(submissionType);
            }


            var submission = new Submission
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
                Description = "Seed Submission",
                UnderwriterCode = "AED",
                UnderwriterContactCode = "JAC",
                QuotingOfficeId = "LON",
                Leader = "AG",
                Domicile = "AD",
                Title = "Seed Submission",
                SubmissionTypeId = submissionTypes[0].Id,

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
                                            //Energy_QuoteExtraProperty1 = "Seed Val...",

                                            CreatedBy = "InitialSetup",
                                            CreatedOn = DateTime.Now,
                                            ModifiedBy = "InitialSetup",
                                            ModifiedOn = DateTime.Now
                                            }
                                        }
                                }}
                        }}
            };
            context.AddOrUpdate<Submission>(submission);

            var addUser1 = new User { DomainLogon = domainPrefix + @"\DaviesA", IsActive = true, UnderwriterCode = "AED", PrimaryOffice = lon, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };//, FilterCOBs = energyCobs };
            ////developers


            var globalTim = new User
            {
                DomainLogon = domainPrefix + @"\tim",
                IsActive = true,
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                OpenTabs =
                    new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.AddOrUpdate<User>(globalTim);

            var u2 = new User
            {
                DomainLogon = domainPrefix + @"\baillief",
                IsActive = true,
                FilterCOBs = new List<COB> { c, c1 },
                FilterOffices = new List<Office> { lon },
                UnderwriterCode = "AED",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                OpenTabs =
                    new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.AddOrUpdate<User>(u2);


            var u6 = new User
            {
                DomainLogon = @"TALBOTDEV\svcUKDEVTFSBuild",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                UnderwriterCode = "AED",
                IsActive = true,
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            u6.FilterMembers = new List<User> { u6 };
            context.AddOrUpdate<User>(u6);
            var u5 = new User
            {
                DomainLogon = @"GLOBALDEV\anandarr",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                IsActive = true,
                UnderwriterCode = "AED",
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            u5.FilterMembers = new List<User> { u5 };
            context.AddOrUpdate<User>(u5);

            var u4 = new User
            {
                DomainLogon = domainPrefix + @"\SheppaA",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                IsActive = true,
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            u4.FilterMembers = new List<User> { u4 };
            context.AddOrUpdate<User>(u4);

            var u3 = new User
            {
                DomainLogon = domainPrefix + @"\MurrayE",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                IsActive = true,
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var u = new User
            {
                DomainLogon = domainPrefix + @"\seigelj",
                IsActive = true,
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                FilterMembers = new List<User> { u2, u4, u3, u5, u6, globalTim },
                OpenTabs =
                    new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.AddOrUpdate<User>(u);
            u2.AdditionalUsers = new List<User> { u };

            // Energy
            var energyUser1 = new User
            {
                DomainLogon = domainPrefix + @"\McDonaldJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            }; //, FilterCOBs = energyCobs };
            var energyUser2 = new User
            {
                DomainLogon = domainPrefix + @"\sarjeat",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser3 = new User
            {
                DomainLogon = domainPrefix + @"\MassieZ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser4 = new User
            {
                DomainLogon = domainPrefix + @"\CantwellJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser5 = new User
            {
                DomainLogon = domainPrefix + @"\GreenM",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser6 = new User
            {
                DomainLogon = domainPrefix + @"\EwingtonJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser7 = new User
            {
                DomainLogon = domainPrefix + @"\ShilingS",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser8 = new User
            {
                DomainLogon = domainPrefix + @"\GarrettJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser9 = new User
            {
                DomainLogon = domainPrefix + @"\StoopE",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser10 = new User
            {
                DomainLogon = domainPrefix + @"\KeoganA",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser11 = new User
            {
                DomainLogon = domainPrefix + @"\ShawI",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser12 = new User
            {
                DomainLogon = domainPrefix + @"\IsmailR",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser13 = new User
            {
                DomainLogon = domainPrefix + @"\SibleyA",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser14 = new User
            {
                DomainLogon = domainPrefix + @"\DaviesK",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser15 = new User
            {
                DomainLogon = domainPrefix + @"\orsoc",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser16 = new User
            {
                DomainLogon = domainPrefix + @"\dempsef",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };


            var energyTeamUsersList = new List<User>
                    {
                        energyUser1,
                        energyUser2,
                        energyUser3,
                        energyUser4,
                        energyUser5,
                        energyUser6,
                        energyUser7,
                        energyUser8,
                        energyUser9,
                        energyUser10,
                        energyUser11,
                        energyUser12,
                        energyUser13,
                        energyUser14,
                        energyUser15,
                        energyUser16
                    };

            // TeamList and Link List
            var linkList = CreateLinksList();

            var teamList = CreateTeamList(submissionTypes);

            teamList[0].Links = new List<Link>();
            foreach (var link in linkList.GetRange(0, 3))
            {
                teamList[0].Links.Add(link);
            }

            teamList[1].Links = new List<Link>();
            foreach (var link in linkList)
            {
                teamList[1].Links.Add(link);
            }
            var appAccelerators = CreateAppAcceleratorsList();
            teamList[0].AppAccelerators = new List<AppAccelerator>();
            foreach (var appAccelerator in appAccelerators)
            {
                teamList[0].AppAccelerators.Add(appAccelerator);
            }

            teamList[1].AppAccelerators = new List<AppAccelerator>();
            foreach (var appAccelerator in appAccelerators)
            {
                teamList[1].AppAccelerators.Add(appAccelerator);
            }

            teamList[0].RelatedOffices = new List<Office> { lon };
            teamList[1].RelatedOffices = new List<Office> { dubai };
            teamList[0].TeamOfficeSettings = new List<TeamOfficeSetting> { new TeamOfficeSetting { Office = lon, MarketWordingSettings = new List<MarketWordingSetting>(), TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(), SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>() } };
            teamList[1].TeamOfficeSettings = new List<TeamOfficeSetting> { new TeamOfficeSetting { Office = dubai, MarketWordingSettings = new List<MarketWordingSetting>(), TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(), SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>() } };

            var marketWordings = CreateMarketWordingsList();
            foreach (var marketWording in marketWordings.Take(10))
            {
                teamList[0].TeamOfficeSettings.First().MarketWordingSettings.Add(new MarketWordingSetting { MarketWording = marketWording });
            }

            foreach (var marketWording in marketWordings.Skip(10).Take(10))
            {
                teamList[1].TeamOfficeSettings.First().MarketWordingSettings.Add(new MarketWordingSetting { MarketWording = marketWording });
            }

            foreach (var marketWording in marketWordings.Skip(20))
            {
                context.AddOrUpdate(marketWording);
            }

            var termsNConditionWordings = CreateTermsNConditionWordingsList();
            foreach (var termsNConditionWording in termsNConditionWordings.Take(5))
            {
                teamList[0].TeamOfficeSettings.First().TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { TermsNConditionWording = termsNConditionWording });
            }

            foreach (var termsNConditionWording in termsNConditionWordings.Take(5))
            {
                teamList[1].TeamOfficeSettings.First().TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { TermsNConditionWording = termsNConditionWording });
            }

            foreach (var termsNConditionWording in termsNConditionWordings.Take(5))
            {
                context.AddOrUpdate(termsNConditionWording);
            }


            var subjectToClauseWordings = CreateSubjectToClauseWordingsList();
            foreach (var subjectToClauseWording in subjectToClauseWordings.Take(5))
            {
                teamList[0].TeamOfficeSettings.First().SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { SubjectToClauseWording = subjectToClauseWording });
            }

            foreach (var subjectToClauseWording in subjectToClauseWordings.Take(5))
            {
                teamList[1].TeamOfficeSettings.First().SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { SubjectToClauseWording = subjectToClauseWording });
            }

            foreach (var subjectToClauseWording in subjectToClauseWordings.Take(5))
            {
                context.AddOrUpdate(subjectToClauseWording);
            }

            var energyTeamMemberships = energyTeamUsersList.Select(energyTeamUser => new TeamMembership { PrimaryTeamMembership = true, User = energyTeamUser, Team = teamList[1], IsCurrent = true, StartDate = DateTime.Now, EndDate = null, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }).ToList();
            energyTeamMemberships.Add(new TeamMembership { PrimaryTeamMembership = false, Team = teamList[1], User = u5, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now });

            var teamMemberships = new List<TeamMembership> 
                { 
                    new TeamMembership { User = u3, Team = teamList[0], IsCurrent = true, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(4), CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now  },
                };

            var memb1 = new TeamMembership { PrimaryTeamMembership = true, Team = teamList[0], User = u, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb3 = new TeamMembership { PrimaryTeamMembership = true, Team = teamList[0], User = u4, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb2 = new TeamMembership { PrimaryTeamMembership = true, Team = teamList[0], User = u2, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb5 = new TeamMembership { PrimaryTeamMembership = true, Team = teamList[0], User = u5, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb6 = new TeamMembership { PrimaryTeamMembership = true, Team = teamList[0], User = u6, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb4 = new TeamMembership { PrimaryTeamMembership = true, Team = teamList[0], User = globalTim, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            teamList[0].RelatedCOBs = new List<COB> { AR, AG, CC, AT };
            teamList[1].RelatedCOBs = new List<COB> { AR, AG, CC, AT };

            teamList[0].Memberships = new List<TeamMembership> { memb1, memb2, memb4, memb3, memb5, memb6 };

            foreach (var teamM in teamMemberships)
            {
                teamList[0].Memberships.Add(teamM);
            }

            teamList[1].Memberships = new List<TeamMembership>();
            foreach (var energyTeamMembership in energyTeamMemberships)
            {
                teamList[1].Memberships.Add(energyTeamMembership);
            }

            foreach (var team in teamList)
            {
                context.AddOrUpdate<Team>(team);
            }

        }

        public static void NewSeedData(IConsoleRepository context)
        {
            var domainPrefix = global::Validus.Console.Properties.Settings.Default.DomainPrefix;

            var AG = new COB { Id = "AG", Narrative = "Direct - Property - Construction", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.Add<COB>(AG);
            var AR = new COB { Id = "AR", Narrative = "Direct - Property - Rig", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.Add<COB>(AR);
            var AT = new COB { Id = "AT", Narrative = "Direct - Property - Hull", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.Add<COB>(AT);
            var CC = new COB { Id = "CC", Narrative = "Direct - Casualty - Construction", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.Add<COB>(CC);


            var c = new COB { Id = "CA", Narrative = "Cargo", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.Add<COB>(c);

            var c1 = new COB { Id = "AD", Narrative = "Direct - Property - Contingency", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            context.Add<COB>(c1);

            var userWriter1 = new Underwriter
            {
                Name = "Alex Colquhoun",
                Code = "JAC",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.Add<Underwriter>(userWriter1);

            var userWriter2 = new Underwriter
            {
                Name = "Alexandra Davies",
                Code = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.Add<Underwriter>(userWriter2);

            var underWriter = new User
            {
                DomainLogon = domainPrefix + @"\defaultunderwriter",
                IsActive = true,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now,
                UnderwriterCode = "AED"
            };
            context.Add<User>(underWriter);

            var quoteSheetPerson = new User
            {
                DomainLogon = domainPrefix + @"\quoteSheetPerson",
                IsActive = true,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now,
                UnderwriterCode = "AED"
            };
            context.Add<User>(quoteSheetPerson);

            // Note - SubmissionStatus - SUBMITTED, QUOTED, FIRM ORDER, DECLINED, WITHDRAWN, ADDL INFO REQST, FIRM ORDER REQST, OTHER OFFICE
            var masterQuote = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "ADC118440A110006", //"AJH105451C12", // Policy Ref
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 25000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "ADC118440A110006",
                SubmissionStatus = "QUOTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 60000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = false,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q3 = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJE125510B12",
                SubmissionStatus = "DECLINED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 360000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = false,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q4master = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJE125510B12",
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 100000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q5 = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJE125510B12",
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 1000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = false,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                // OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q6master = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJY087986D12",
                SubmissionStatus = "SUBMITTED",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 890000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var q7master = new Quote
            {
                AccountYear = 2013,
                COB = c,
                COBId = c.Id,
                Currency = "USD",
                //<??>
                //QuoteCreated = DateTime.Now,
                //QuoteLastModified = DateTime.Now,
                //QuoteLastModifiedBy = underWriter,
                //QuoteCreatedBy = underWriter,
                SubscribeReference = "AJJ144268A12",
                SubmissionStatus = "ADDL INFO REQST",
                EntryStatus = "PARTIAL",
                PolicyType = "NONMARINE",
                TechnicalPricingMethod = "MODEL",
                QuotedPremium = 39000,
                FacilityRef = "ADC118440A11",
                IsSubscribeMaster = true,
                InceptionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                QuoteExpiryDate = DateTime.Now.AddDays(30),
                //OriginatingOfficeId = "LON",
                MOA = "FA",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };


            var quoteSheet1 = new QuoteSheet { Title = "QS_1", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-1), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet2 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-2), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet3 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-3), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet4 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-4), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet5 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-5), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet6 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-6), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet7 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-7), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var quoteSheet8 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-8), ObjectStore = "Underwriting", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            var ov = new OptionVersion
            {
                Title = "Version 1",
                VersionNumber = 1,
                Quotes = new List<Quote> { q, masterQuote },
                QuoteSheets =
                    new List<QuoteSheet>
                                {
                                    quoteSheet1,
                                    quoteSheet2,
                                    quoteSheet3,
                                    quoteSheet4,
                                    quoteSheet5,
                                    quoteSheet6
                                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var ov2 = new OptionVersion
            {
                Title = "Version 2",
                VersionNumber = 2,
                Quotes = new List<Quote> { q3, q4master, q5 },
                QuoteSheets = new List<QuoteSheet> { quoteSheet7, quoteSheet8 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };


            var ov3 = new OptionVersion
            {
                Title = "Version 3",
                VersionNumber = 1,
                Quotes = new List<Quote> { q6master },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };


            var ov4 = new OptionVersion
            {
                Title = "Version 4",
                VersionNumber = 1,
                Quotes = new List<Quote> { q7master },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var o1 = new Option
            {
                Title = "Option 1",
                Comments =
                    "There is a known issue with this technique where creating databases is not supported on SqlClient provider. Other providers may or may not support this functionality depending on implementation. In general, because of that it is recommended to use unwrapped connections when using DDL APIs (CreateDatabase, DeleteDatabase, DatabaseExists()) as demonstrated in the sample.",
                OptionVersions = new List<OptionVersion> { ov, ov2 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var o2 = new Option
            {
                Title = "Option 2",
                Comments =
                    "In order to efficiently manage tracing for the application we need to create a central factory class which will create ObjectContext instances for us. This is the place where we will create tracing provider connection and use it to instantiate ObjectContext. Assuming our Object Context class is called MyContainer, the factory class will be called MyContainerFactory and will have a method called CreateContext, so the usage becomes:",
                OptionVersions = new List<OptionVersion> { ov3 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var o3 = new Option
            {
                Title = "Option 3",
                Comments =
                    "Entity Framework/Code First feature released as part of Feature CTP 3 can work with any EF-enabled data provider. In addition to regular providers which target databases, it is possible to use wrapping providers which can add interesting functionality, such as caching and tracing. In this post I’m going to explain how to use EFTracingProvider to produce diagnostic trace of all SQL commands executed by EF in Code First.",
                OptionVersions = new List<OptionVersion> { ov4 },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var lon = new Office
            {
                Id = "LON",
                Name = "London",
                Title = "Talbot Underwriting Ltd",
                Footer = "Talbot Underwriting Ltd is Authorised by the Prudential Regulation Authority and regulated by the Financial Conduct Authority and the Prudential Regulation Authority.",
                Address = new Address
                {
                    AddressLine1 = "60 Threadneedle Street",
                    AddressLine2 = "",
                    City = "London",
                    ZipPostalCode = "EC2R 8HP",
                    Country = "England",
                    Phone = "020 7550 3500",
                    Fax = "020 7550 3555",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var mia = new Office
            {
                Id = "MIA",
                Name = "Miami",
                Title = "Validus Reaseguros, Inc.",
                Address = new Address
                {
                    AddressLine1 = "2601 South Bayshore Drive",
                    AddressLine2 = "Suite 1850, Coconut Grove",
                    City = "Miami",
                    StateProvinceRegion = "Florida",
                    ZipPostalCode = "33133",
                    Country = "U.S.A.",
                    Phone = "305 631 7780",
                    Fax = "305 631 7781",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var lab = new Office
            {
                Id = "LAB",
                Name = "Labuan",
                Title = "Talbot Risk Services (Labuan) Pte Ltd",
                Address = new Address
                {
                    AddressLine1 = "Brighton Place, Ground Floor,",
                    AddressLine2 = "No. U0215, Jalan Bahasa,",
                    City = "Labuan FT",
                    Country = "Malaysia",
                    ZipPostalCode = "87000",
                    Phone = "+60 87 442899",
                    Fax = "+60 87 451899",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var nyc = new Office
            {
                Id = "NYC",
                Name = "New York",
                Title = "Talbot Underwriting (US) Ltd. ",
                Address = new Address
                {
                    AddressLine1 = "48 Wall Street 17th Floor",
                    AddressLine2 = "",
                    Country = "USA",
                    StateProvinceRegion = "NY",
                    City = "New York",
                    ZipPostalCode = "10005",
                    Phone = "+1 212 785 2000",
                    Fax = "+1 212 785 2001",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var sng = new Office
            {
                Id = "SNG",
                Name = "Singapore",
                Title = "Talbot Risk Services Pte Ltd",
                Address = new Address
                {
                    AddressLine1 = "8 Marina View #14-01",
                    AddressLine2 = "Asia Square Tower 1",
                    City = "Singapore",
                    ZipPostalCode = "018960",
                    Phone = "+65 6511 1400",
                    Fax = "+65 6511 1401",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var dubai = new Office
            {
                Id = "DUB",
                Name = "Dubai",
                Title = "Talbot Underwriting (MENA) Ltd",
                Footer = "Underwriting Risk Services (Middle East) Ltd ('URSME') is authorised and regulated by the Dubai Financial Services Authority.",
                Address = new Address
                {
                    AddressLine1 = "Dubai International Financial Centre",
                    AddressLine2 = "Gate Village Building 10, Level 5",
                    City = "Dubai",
                    Country = "UAE",
                    ZipPostalCode = "PO Box 506809",
                    Phone = "+971 4 448 7780",
                    Fax = "+971 4 448 7781",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            var usrl = new Office
            {
                Id = "URS",
                Name = "URSL",
                Title = "Talbot Underwriting Risk Services Ltd",
                Footer = "Talbot Underwriting Risk Services Ltd is an appointed representative of Talbot Underwriting Ltd which is Authorised by the Prudential Regulation Authority and regulated by the Financial Conduct Authority and the Prudential Regulation Authority.",
                Address = new Address
                {
                    AddressLine1 = "60 Threadneedle Street",
                    AddressLine2 = "",
                    City = "London",
                    Country = "England",
                    ZipPostalCode = "EC2R 8HP",
                    Phone = "020 7550 3500",
                    Fax = "020 7550 3555",
                    Url = "www.validusholdings.com",
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                },
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            context.Add<Office>(lon);
            context.Add<Office>(mia);
            context.Add<Office>(lab);
            context.Add<Office>(nyc);
            context.Add<Office>(sng);
            context.Add<Office>(dubai);
            context.Add<Office>(usrl);

            //var userWriter1 = new Underwriter
            //{
            //    Name = "Alex Colquhoun",
            //    Code = "JAC",
            //    CreatedBy = "InitialSetup",
            //    CreatedOn = DateTime.Now,
            //    ModifiedBy = "InitialSetup",
            //    ModifiedOn = DateTime.Now
            //};
            //context.Add<Underwriter>(userWriter1);

            //userWriter2 = new Underwriter
            //{
            //    Name = "Alexandra Davies",
            //    Code = "AED",
            //    CreatedBy = "InitialSetup",
            //    CreatedOn = DateTime.Now,
            //    ModifiedBy = "InitialSetup",
            //    ModifiedOn = DateTime.Now
            //};
            //context.Add<Underwriter>(userWriter2);

            var submission = new Submission
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
                Description = "Seed Submission",
                UnderwriterCode = "AED",
                UnderwriterContactCode = "JAC",
                QuotingOfficeId = "LON",
                Leader = "AG",
                Domicile = "AD",
                Title = "Seed Submission",
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
                                            SubscribeReference = "AJE125510B12",
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
            context.Add<Submission>(submission);

            var addUser1 = new User { DomainLogon = domainPrefix + @"\DaviesA", IsActive = true, UnderwriterCode = "AED", PrimaryOffice = lon, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };//, FilterCOBs = energyCobs };
            ////developers


            var globalTim = new User
            {
                DomainLogon = domainPrefix + @"\tim",
                IsActive = true,
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                OpenTabs =
                    new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.Add<User>(globalTim);

            var u2 = new User
            {
                DomainLogon = domainPrefix + @"\baillief",
                IsActive = true,
                FilterCOBs = new List<COB> { c, c1 },
                FilterOffices = new List<Office> { lon },
                UnderwriterCode = "AED",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                OpenTabs =
                    new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.Add<User>(u2);


            var u6 = new User
            {
                DomainLogon = @"TALBOTDEV\svcUKDEVTFSBuild",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                UnderwriterCode = "AED",
                IsActive = true,
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            u6.FilterMembers = new List<User> { u6 };
            context.Add<User>(u6);
            var u5 = new User
            {
                DomainLogon = @"GLOBALDEV\anandarr",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                IsActive = true,
                UnderwriterCode = "AED",
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            u5.FilterMembers = new List<User> { u5 };
            context.Add<User>(u5);

            var u4 = new User
            {
                DomainLogon = domainPrefix + @"\SheppaA",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                IsActive = true,
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            u4.FilterMembers = new List<User> { u4 };
            context.Add<User>(u4);

            var u3 = new User
            {
                DomainLogon = domainPrefix + @"\MurrayE",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                AdditionalUsers = new List<User> { addUser1 },
                IsActive = true,
                OpenTabs = new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };

            var u = new User
            {
                DomainLogon = domainPrefix + @"\seigelj",
                IsActive = true,
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                FilterMembers = new List<User> { u2, u4, u3, u5, u6, globalTim },
                OpenTabs =
                    new List<Tab> { new Tab { Url = "/Submission/_Create", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                UnderwriterCode = "AED",
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };
            context.Add<User>(u);
            u2.AdditionalUsers = new List<User> { u };

            // Energy
            var energyUser1 = new User
            {
                DomainLogon = domainPrefix + @"\McDonaldJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            }; //, FilterCOBs = energyCobs };
            var energyUser2 = new User
            {
                DomainLogon = domainPrefix + @"\sarjeat",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser3 = new User
            {
                DomainLogon = domainPrefix + @"\MassieZ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser4 = new User
            {
                DomainLogon = domainPrefix + @"\CantwellJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser5 = new User
            {
                DomainLogon = domainPrefix + @"\GreenM",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser6 = new User
            {
                DomainLogon = domainPrefix + @"\EwingtonJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser7 = new User
            {
                DomainLogon = domainPrefix + @"\ShilingS",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser8 = new User
            {
                DomainLogon = domainPrefix + @"\GarrettJ",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser9 = new User
            {
                DomainLogon = domainPrefix + @"\StoopE",
                IsActive = true,
                UnderwriterCode = "AED",
                FilterCOBs = new List<COB> { AR, AG, CC, AT },
                FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                PrimaryOffice = lon,
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser10 = new User
            {
                DomainLogon = domainPrefix + @"\KeoganA",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser11 = new User
            {
                DomainLogon = domainPrefix + @"\ShawI",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser12 = new User
            {
                DomainLogon = domainPrefix + @"\IsmailR",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser13 = new User
            {
                DomainLogon = domainPrefix + @"\SibleyA",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser14 = new User
            {
                DomainLogon = domainPrefix + @"\DaviesK",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser15 = new User
            {
                DomainLogon = domainPrefix + @"\orsoc",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };
            var energyUser16 = new User
            {
                DomainLogon = domainPrefix + @"\dempsef",
                AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                IsActive = true,
                PrimaryOffice = lon,
                UnderwriterCode = "AED",
                CreatedBy = "InitialSetup",
                CreatedOn = DateTime.Now,
                ModifiedBy = "InitialSetup",
                ModifiedOn = DateTime.Now
            };//, FilterCOBs = energyCobs };


            var energyTeamUsersList = new List<User>
                    {
                        energyUser1,
                        energyUser2,
                        energyUser3,
                        energyUser4,
                        energyUser5,
                        energyUser6,
                        energyUser7,
                        energyUser8,
                        energyUser9,
                        energyUser10,
                        energyUser11,
                        energyUser12,
                        energyUser13,
                        energyUser14,
                        energyUser15,
                        energyUser16
                    };

            // TeamList and Link List
            var linkList = CreateLinksList();

            var submissionTypes = new List<SubmissionType>
                {
                    new SubmissionType { Id = "EN", Title = "Energy_Submission" },
                    new SubmissionType { Id = "PV", Title = "PoliticalViolence_Submission" },
                    new SubmissionType { Id = "FI", Title = "FinancialInstitutions_Submission" }
                };

            foreach (var submissionType in submissionTypes)
            {
                context.Add<SubmissionType>(submissionType);
            }
            var teamList = CreateTeamList(submissionTypes);

            teamList[0].Links = new List<Link>();
            foreach (var link in linkList.GetRange(0, 3))
            {
                teamList[0].Links.Add(link);
            }

            teamList[1].Links = new List<Link>();
            foreach (var link in linkList)
            {
                teamList[1].Links.Add(link);
            }
            var appAccelerators = CreateAppAcceleratorsList();
            teamList[0].AppAccelerators = new List<AppAccelerator>();
            foreach (var appAccelerator in appAccelerators)
            {
                teamList[0].AppAccelerators.Add(appAccelerator);
            }

            teamList[1].AppAccelerators = new List<AppAccelerator>();
            foreach (var appAccelerator in appAccelerators)
            {
                teamList[1].AppAccelerators.Add(appAccelerator);
            }

            teamList[0].RelatedOffices = new List<Office> { lon };
            teamList[1].RelatedOffices = new List<Office> { dubai };
            teamList[0].TeamOfficeSettings = new List<TeamOfficeSetting> { new TeamOfficeSetting { Office = lon, MarketWordingSettings = new List<MarketWordingSetting>(), TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(), SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>() } };
            teamList[1].TeamOfficeSettings = new List<TeamOfficeSetting> { new TeamOfficeSetting { Office = dubai, MarketWordingSettings = new List<MarketWordingSetting>(), TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(), SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>() } };

            var marketWordings = CreateMarketWordingsList();
            foreach (var marketWording in marketWordings.Take(10))
            {
                teamList[0].TeamOfficeSettings.First().MarketWordingSettings.Add(new MarketWordingSetting { MarketWording = marketWording });
            }

            foreach (var marketWording in marketWordings.Skip(10).Take(10))
            {
                teamList[1].TeamOfficeSettings.First().MarketWordingSettings.Add(new MarketWordingSetting { MarketWording = marketWording });
            }

            foreach (var marketWording in marketWordings.Skip(20))
            {
                context.Add(marketWording);
            }

            var termsNConditionWordings = CreateTermsNConditionWordingsList();
            foreach (var termsNConditionWording in termsNConditionWordings.Take(5))
            {
                teamList[0].TeamOfficeSettings.First().TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { TermsNConditionWording = termsNConditionWording });
            }

            foreach (var termsNConditionWording in termsNConditionWordings.Take(5))
            {
                teamList[1].TeamOfficeSettings.First().TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { TermsNConditionWording = termsNConditionWording });
            }

            foreach (var termsNConditionWording in termsNConditionWordings.Take(5))
            {
                context.Add(termsNConditionWording);
            }


            var subjectToClauseWordings = CreateSubjectToClauseWordingsList();
            foreach (var subjectToClauseWording in subjectToClauseWordings.Take(5))
            {
                teamList[0].TeamOfficeSettings.First().SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { SubjectToClauseWording = subjectToClauseWording });
            }

            foreach (var subjectToClauseWording in subjectToClauseWordings.Take(5))
            {
                teamList[1].TeamOfficeSettings.First().SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { SubjectToClauseWording = subjectToClauseWording });
            }

            foreach (var subjectToClauseWording in subjectToClauseWordings.Take(5))
            {
                context.Add(subjectToClauseWording);
            }
            var energyTeamMemberships = energyTeamUsersList.Select(energyTeamUser => new TeamMembership { User = energyTeamUser, Team = teamList[1], IsCurrent = true, StartDate = DateTime.Now, EndDate = null, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }).ToList();
            energyTeamMemberships.Add(new TeamMembership { PrimaryTeamMembership = false, Team = teamList[1], User = u5, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now });
            var teamMemberships = new List<TeamMembership> 
                { 
                    new TeamMembership { User = u3, Team = teamList[0], IsCurrent = true, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(4), CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now  },
                };

            var memb1 = new TeamMembership { Team = teamList[0], User = u, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb3 = new TeamMembership { Team = teamList[0], User = u4, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb2 = new TeamMembership { Team = teamList[0], User = u2, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb5 = new TeamMembership { Team = teamList[0], User = u5, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb6 = new TeamMembership { Team = teamList[0], User = u6, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            var memb4 = new TeamMembership { Team = teamList[0], User = globalTim, StartDate = DateTime.Now, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            teamList[0].RelatedCOBs = new List<COB> { AR, AG, CC, AT };
            teamList[1].RelatedCOBs = new List<COB> { AR, AG, CC, AT };

            teamList[0].Memberships = new List<TeamMembership> { memb1, memb2, memb4, memb3, memb5, memb6 };

            foreach (var teamM in teamMemberships)
            {
                teamList[0].Memberships.Add(teamM);
            }

            teamList[1].Memberships = new List<TeamMembership>();
            foreach (var energyTeamMembership in energyTeamMemberships)
            {
                teamList[1].Memberships.Add(energyTeamMembership);
            }

            foreach (var team in teamList)
            {
                context.Add<Team>(team);
            }
        }

        private static List<MarketWording> CreateMarketWordingsList()
        {
            return new List<MarketWording>{
                new MarketWording   {WordingRefNumber = "LMA3030",Title = "LMA3030"},
                new MarketWording   {WordingRefNumber = "LMA5039",Title = "LMA5039"},
                new MarketWording   {WordingRefNumber = "T3",Title = "T3"},
                new MarketWording   {WordingRefNumber = "T3A",Title = "T3A"},
                new MarketWording   {WordingRefNumber = "T3L",Title = "T3L"},
                new MarketWording   {WordingRefNumber = "SRCCMD",Title = "SRCCMD"},
                new MarketWording   {WordingRefNumber = "LP0437",Title = "LP0437"},
                new MarketWording   {WordingRefNumber = "LSW667",Title = "LSW667"},
                new MarketWording   {WordingRefNumber = "LMA3092",Title = "LMA3092"},
                new MarketWording   {WordingRefNumber = "FULL PV",Title = "FULL PV"},
                new MarketWording   {WordingRefNumber = "KFA 81",Title = "Based on the KFA 81 wording"},
                new MarketWording   {WordingRefNumber = "LSW 283",Title = "Based on the LSW 983"},
                new MarketWording   {WordingRefNumber = "LSW 283",Title = "Based on the LSW 283"},
                new MarketWording   {WordingRefNumber = "DHP 84",Title = "Based on the DHP 84 wording"},
                new MarketWording   {WordingRefNumber = "Form 14",Title = "Based on the Form 14 wording"},
                new MarketWording   {WordingRefNumber = "Form 24",Title = "Based on the Form 24 wording"},
                new MarketWording   {WordingRefNumber = "LPO218",Title = "Based on the LPO218 wording"},
                new MarketWording   {WordingRefNumber = "NMA2626",Title = "Based on the NMA2626 wording"},
                new MarketWording   {WordingRefNumber = "RAGJ",Title = "Based on the RAGJ wording"},
                new MarketWording   {WordingRefNumber = "Stockbroker (FSIP)",Title = "Based on the Stockbroker wording (FSIP)"},
                new MarketWording   {WordingRefNumber = "Crimeguard",Title = "Based on the Crimeguard wording"},
                new MarketWording   {WordingRefNumber = "NMA 2273",Title = "Based on the NMA 2273"},
                new MarketWording   {WordingRefNumber = "AIG Civil Liability (CHT00019 1211)",Title = "Based on the AIG Civil Liability (CHT00019 1211)"},
                new MarketWording   {WordingRefNumber = "AIG Investment Management Insurance (CHARTISMLFIM 28/04/10)",Title = "Based on the AIG Investment Management Insurance (CHARTISMLFIM 28/04/10)"},
                new MarketWording   {WordingRefNumber = "Chubb VCAP Form (04/06)",Title = "Based on the Chubb VCAP Form (04/06)"},
                new MarketWording   {WordingRefNumber = "NMA 2273",Title = "Based on the NMA 2273"},
                new MarketWording   {WordingRefNumber = "NMA 2273 + Extensionas",Title = "Based on the NMA 2273 + Extensionas"},
                new MarketWording   {WordingRefNumber = "NMA 3000",Title = "Based on the NMA 3000"},
                new MarketWording   {WordingRefNumber = "Financial Services Industry Policy 2003",Title = "Based on the Financial Services Industry Policy 2003"},
                new MarketWording   {WordingRefNumber = "SVB 2002",Title = "Based on the SVB 2002 wording"},
                new MarketWording   {WordingRefNumber = "AIG Corporate Guard 2006",Title = "Based on the AIG Corporate Guard 2006"},
                new MarketWording   {WordingRefNumber = "AIG IMI",Title = "Based on the AIG IMI Wording"},
                new MarketWording   {WordingRefNumber = "Beazley Trust",Title = "Based on the Beazley Trust wording"},
                new MarketWording   {WordingRefNumber = "Full Securities Entities Coverage",Title = "Based on the Full Securities Entities Coverage wording"},
                new MarketWording   {WordingRefNumber = "ODL Extension",Title = "Based on the ODL Extension wording"},
                new MarketWording   {WordingRefNumber = "Chubb Form",Title = "Based on the Chubb Form"}};




        }

        private static List<TermsNConditionWording> CreateTermsNConditionWordingsList()
        {
            return new List<TermsNConditionWording>{
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Non-Binding terms open for 7 days"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "LSW 3000(45/60 days)"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Subject full/complete schedule of locations (including 9 digit post-code, zip code or latitude and longitude grid reference) emailed to Terrorism@talbotuw.com prior to inception"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Excluding Chemical/Biological/Cyber Terrorism as per CL370/380, or as per T3"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Sanctions Clause 3100"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Subject to the Law of England and Wales and the exclusive jurisdiction of the court of England and Wales"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "72 Hours Occurrence Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Subject to no losses or threats in the past 5 years"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Claims Control Clause for Reinsurances"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Excluding T&D/Pipelines"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Policies to comply with Lloyds 2010 claims scheme"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "LSW3001 /3002 (as applicable) 60/15"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Institute Radioactive Contamination, Chemical, Biological, Biochemical and Electromagnetic Weapons Exclusion Clause (Cl.370)"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Institute Cyber Attack Exclusion Clause (Cl.380)"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "No Cover Given"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Slip terms and conditions to be agreed"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Wording to be presented for agreement not less than 24 hours prior to attachment"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Sanction Limitation and Exclusion Clause JC2010/014"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "NMA 2920 Terrorism Exclusion for onshore"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Computer Virus Exclusion NMA 2914/5"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "EDRC NMA 2800/2"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "MGW Extn Wording"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Inc USA Wording"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "England & Wales Law & Jurisdiction Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "War & Terrorism Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "International Trade Sanctions Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Absolute Plastic Card Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Absolute Cheque Kiting Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Absolute Bills of Lading Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Money Laundering Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Discovery Limitation Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "ATM Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Aggregate Policy Limit Endorsement"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Claims Control Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Automatic Reinstatement Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "No Account Taken Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Interlocking Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Deliberate Corporate Acts Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Cut Through Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Alternative Dispute Resolution Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Anti-Trust Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Computer Mis-use/Manipulation Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Criminal/Disciplinary Offences Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Excessive Fees Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Limited Subsidiary Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Parent Company Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Fraudulent Claims Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "30 Day Premium Payment Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "45 day Premium Payment Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "60 Day Premium Payment Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "90 Day Premium Payment Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Currency Conversion Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Fines & Penalties Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Mechanical Breakdown Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Retroactive Date Inception"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Mis-selling Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Mutual Fund Trading Practices Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Investment Managers Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Property Management Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Reservation of Rights Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Including Voice Initiated Transfer"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Excluding Voice Initiated Transfer Coverage Absolutely"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "PINA Exclusion"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "Automatic Acquisition Clause"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "5% NCB"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "7.5% NCB"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "10% NCB"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "12.5% NCB"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "15% NCB"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "2.5% LTA"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "5% LTA"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "7.5% LTA"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "10% LTA"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "12.5% LTA"},
                new TermsNConditionWording  {WordingRefNumber = "",Title = "15% LTA"}};

        }

        private static List<SubjectToClauseWording> CreateSubjectToClauseWordingsList()
        {
            return new List<SubjectToClauseWording>{
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Confirmation no known or reported losses in the past 5 years"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Satisfactory Signed and Dated BB Proposal Form"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Satisfactory Signed and Dated Electronic & Computer Crime Proposal Form"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Satisfactory Signed and Dated PI Proposal Form"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Satisfactory Signed and Dated D&O Proposal Form"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Satisfactory Signed and Dated Internet Banking Questionaire"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "No Claims Declaration"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "No Material Changes Declaration"},
                new SubjectToClauseWording   {WordingRefNumber = "",Title = "Latest Report & Accounts"}};

        }

        private static List<AppAccelerator> CreateAppAcceleratorsList()
        {
            return new List<AppAccelerator>
                {
                    new AppAccelerator
                        {
                            Id="WorldCheck",
                            HomepageUrl =
                                "localhost" + ":" +
                                "52377",
                            DisplayName = "WorldCheck",
                            DisplayIcon =
                                "localhost" + ":" +
                                "52377" + @"/favicon.ico",
                            ActivityCategory = "Validus",
                            ActivityActionPreview =
                                "localhost" + ":" +
                                "52377" +
                                "/WorldCheck/_WorldCheckSearchMatches?insuredName={selection}",
                            ActivityActionExecute =
                                "localhost" + ":" +
                                "52377" +
                                "/WorldCheck/_WorldCheckSearchMatches?insuredName={selection}",
                        }
                };
        }

        private static List<Link> CreateLinksList()
        {
            return new List<Link> { 
                new Link { Url = "http://docs.talbotuw.com/default.aspx", Title="DMS", Category = "Doc Management"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "https://acord.validusholdings.com/MMT/MessageSummary.aspx", Title="MMT - ebusiness", Category = "ebusiness"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "https://www.ri3k.com/marketplace/login/index.html", Title="MMT - Qatarlyst", Category = "Qatarlyst"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "http://intranet.validusholdings.com/BI/SitePages/Regular%20Talbot%20Reports.aspx", Title="Weekly management reports", Category = "Reports"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "http://ireport:8700/talbot/getfolderitems.do?folder=/UW/Monthly%20UW%20Figures", Title="Class summaries & triangles", Category = "Reports"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do", Title="Market wordings database - Info", Category = "Information"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "http://apps.talbotuw.com/sites/workflow/pages/underwritingworkflow.aspx", Title="Market wordings database - Workflow", Category = "Workflow"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "http://www.lloyds.com/The-Market/Tools-and-Resources/Tools-E-Services/Crystal", Title="Crystal - Compliance Tools", Category = "Compliance Tools"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now},
                new Link { Url = "http://uktrms01.talbotuw.com/Wiki/EM_Wiki/Aggregate_Reports/Summary/ExposureDashboard.html", Title="WOL/Terror hotspot report", Category = "Reports"  , CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now}

            };
        }

        private static List<Team> CreateTeamList(List<SubmissionType> submissionTypes)
        {
            var quoteSheetTemplates = CreateQuoteSheets();

            return new List<Team> {
                new Team { Title = "Developers", QuoteExpiryDaysDefault = 30 ,  CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now, SubmissionTypeId = null, DefaultPolicyType = "MARINE"    },
                new Team { Title = "Energy", QuoteExpiryDaysDefault = 30 , RelatedQuoteTemplates = quoteSheetTemplates.Where(t => t.Name.Contains("Energy")).ToList(), CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now, SubmissionTypeId = submissionTypes[0].Id, DefaultPolicyType = "MARINE"   },
                new Team { Title = "Political Violence", QuoteExpiryDaysDefault = 30 , RelatedQuoteTemplates = quoteSheetTemplates.Where(t => t.Name.Contains("PV")).ToList(), CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now, SubmissionTypeId = submissionTypes[1].Id, DefaultPolicyType = "NONMARINE"   },
                new Team { Title = "Financial Institutions", QuoteExpiryDaysDefault = 30 , RelatedQuoteTemplates = quoteSheetTemplates.Where(t => t.Name.Contains("FI")).ToList(), CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now, SubmissionTypeId = submissionTypes[2].Id, DefaultPolicyType = "NONMARINE", RelatedRisks = CreateRisksList()   }
            };
        }

        private static List<QuoteTemplate> CreateQuoteSheets()
        {
            return new List<QuoteTemplate>
                {
                    new QuoteTemplate { Name = "Energy 1", RdlPath = "/Underwriting/Console/QuoteSheet" },
                    new QuoteTemplate { Name = "Energy 2", RdlPath = "/Underwriting/Console/QuoteSheet_EN2" },
                    new QuoteTemplate { Name = "PV 1", RdlPath = "/Underwriting/Console/QuoteSheet_PV" },
                    new QuoteTemplate { Name = "FI 1", RdlPath = "/Underwriting/Console/QuoteSheet_FI" }
                };
        }

        private static List<RiskCode> CreateRisksList()
        {
            return new List<RiskCode>
            {
                new RiskCode { Code= "DO", Name = "Directors and officers liability" },
                new RiskCode { Code= "BB", Name = "Fidelity, comp crime and bankers' policies" },
                new RiskCode { Code= "PI", Name = "Errors and omissions/professional indemnity" },
            };
        }
    }
}