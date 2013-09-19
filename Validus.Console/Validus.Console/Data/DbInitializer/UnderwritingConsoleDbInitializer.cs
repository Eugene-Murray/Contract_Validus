using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Validus.Models;

namespace Validus.Console.Data.DbInitializer
{
    public class UnderwritingConsoleDbInitializer : DropCreateDatabaseIfModelChanges<ConsoleRepository>
    {
	    protected override void Seed(ConsoleRepository context)
	    {
	    }

	    /*protected override void Seed(ConsoleRepository context)
        {
            try
            {
                string domainPrefix = global::Validus.Console.Properties.Settings.Default.DomainPrefix;

                var AG = new COB { Id = "AG", Narrative = "Direct - Property - Construction" };
                context.Query<COB>().Add(AG);
                var AR = new COB { Id = "AR", Narrative = "Direct - Property - Rig" };
                context.Query<COB>().Add(AR);
                var AT = new COB { Id = "AT", Narrative = "Direct - Property - Hull" };
                context.Query<COB>().Add(AT);
                var CC = new COB { Id = "CC", Narrative = "Direct - Casualty - Construction" };
                context.Query<COB>().Add(CC);


                var c = new COB {Id = "CA", Narrative = "Cargo"};
                context.Query<COB>().Add(c);

                var c1 = new COB {Id = "AD", Narrative = "Direct - Property - Contingency"};
                context.Query<COB>().Add(c1);
                var underWriter = new User
                {
                    DomainLogon = domainPrefix + @"\defaultunderwriter",
                    IsActive = true
                };

                var quoteSheetPerson = new User
                {
                    DomainLogon = domainPrefix + @"\quoteSheetPerson",
                    IsActive = true
                };
                context.Users.Add(quoteSheetPerson);

                // Note - SubmissionStatus - SUBMITTED, QUOTED, FIRM ORDER, DECLINED, WITHDRAWN, ADDL INFO REQST, FIRM ORDER REQST, OTHER OFFICE
                var masterQuote = new Quote{ 
                    AccountYear = 2013,
					COB = c,
					COBId = c.Id,
                    Currency = "USD",
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LastModifiedBy = underWriter,
                    CreatedBy = underWriter,
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
                    OriginatingOfficeId = "LON",
					MOA = "FA"
                };

                var q = new Quote
                    {
                        AccountYear = 2013,
						COB = c,
						COBId = c.Id,
                        Currency = "USD",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        LastModifiedBy = underWriter,
                        CreatedBy = underWriter,
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
						OriginatingOfficeId = "LON",
						MOA = "FA"
                    };

                var q3 = new Quote
                    {
                        AccountYear = 2013,
						COB = c,
						COBId = c.Id,
                        Currency = "USD",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        LastModifiedBy = underWriter,
                        CreatedBy = underWriter,
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
						OriginatingOfficeId = "LON",
						MOA = "FA"
                    };

                var q4master = new Quote
                    {
                        AccountYear = 2013,
						COB = c,
						COBId = c.Id,
                        Currency = "USD",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        LastModifiedBy = underWriter,
                        CreatedBy = underWriter,
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
						OriginatingOfficeId = "LON",
						MOA = "FA"
                    };

                var q5 = new Quote
                    {
                        AccountYear = 2013,
						COB = c,
						COBId = c.Id,
                        Currency = "USD",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        LastModifiedBy = underWriter,
                        CreatedBy = underWriter,
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
						OriginatingOfficeId = "LON",
						MOA = "FA"
                    };

                var q6master = new Quote
                    {
                        AccountYear = 2013,
                        COB = c,
						COBId = c.Id,
                        Currency = "USD",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        LastModifiedBy = underWriter,
                        CreatedBy = underWriter,
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
						OriginatingOfficeId = "LON",
						MOA = "FA"
                    };

                var q7master = new Quote
                    {
                        AccountYear = 2013,
						COB = c,
						COBId = c.Id,
                        Currency = "USD",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        LastModifiedBy = underWriter,
                        CreatedBy = underWriter,
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
						OriginatingOfficeId = "LON",
						MOA = "FA"
                    };


                var quoteSheet1 = new QuoteSheet { Title = "QS_1", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-1), ObjectStore = "Underwriting" };
                var quoteSheet2 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-2), ObjectStore = "Underwriting" };
                var quoteSheet3 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-3), ObjectStore = "Underwriting" };
                var quoteSheet4 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-4), ObjectStore = "Underwriting" };
                var quoteSheet5 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-5), ObjectStore = "Underwriting" };
                var quoteSheet6 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-6), ObjectStore = "Underwriting" };
                var quoteSheet7 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-7), ObjectStore = "Underwriting" };
                var quoteSheet8 = new QuoteSheet { Title = "QS_2", Guid = Guid.NewGuid(), IssuedBy = quoteSheetPerson, IssuedDate = DateTime.Now.AddDays(-8), ObjectStore = "Underwriting" };

                var ov = new OptionVersion
                    {
                        Title = "Version 1",
                        VersionNumber = 1,
                        Quotes = new List<Quote> {q, masterQuote},
                        QuoteSheets =
                            new List<QuoteSheet>
                                {
                                    quoteSheet1,
                                    quoteSheet2,
                                    quoteSheet3,
                                    quoteSheet4,
                                    quoteSheet5,
                                    quoteSheet6
                                }
                    };

                var ov2 = new OptionVersion
                    {
                        Title = "Version 2",
                        VersionNumber = 2,
                        Quotes = new List<Quote> {q3, q4master, q5},
                        QuoteSheets = new List<QuoteSheet> {quoteSheet7, quoteSheet8}
                    };


                var ov3 = new OptionVersion
                    {
                        Title = "Version 3",
                        VersionNumber = 1,
                        Quotes = new List<Quote> {q6master}
                    };


                var ov4 = new OptionVersion
                    {
                        Title = "Version 4",
                        VersionNumber = 1,
                        Quotes = new List<Quote> {q7master}
                    };

                var o1 = new Option
                    {
                        Title = "Option 1",
                        Comments =
                            "There is a known issue with this technique where creating databases is not supported on SqlClient provider. Other providers may or may not support this functionality depending on implementation. In general, because of that it is recommended to use unwrapped connections when using DDL APIs (CreateDatabase, DeleteDatabase, DatabaseExists()) as demonstrated in the sample.",
                        OptionVersions = new List<OptionVersion> {ov, ov2}
                    };

                var o2 = new Option
                    {
                        Title = "Option 2",
                        Comments =
                            "In order to efficiently manage tracing for the application we need to create a central factory class which will create ObjectContext instances for us. This is the place where we will create tracing provider connection and use it to instantiate ObjectContext. Assuming our Object Context class is called MyContainer, the factory class will be called MyContainerFactory and will have a method called CreateContext, so the usage becomes:",
                        OptionVersions = new List<OptionVersion> {ov3}
                    };

                var o3 = new Option
                    {
                        Title = "Option 3",
                        Comments =
                            "Entity Framework/Code First feature released as part of Feature CTP 3 can work with any EF-enabled data provider. In addition to regular providers which target databases, it is possible to use wrapping providers which can add interesting functionality, such as caching and tracing. In this post I’m going to explain how to use EFTracingProvider to produce diagnostic trace of all SQL commands executed by EF in Code First.",
                        OptionVersions = new List<OptionVersion> {ov4}
                    };

                var s = new Submission
                    {
                        Title = "Shell Policy",
                        InsuredName = "RED ELECTRICA DE ESPANA SAU",
                        BrokerCode = "0775",
                        BrokerPseudonym = "BRE",
                        BrokerContact = "Ramnik Rajguru",
                        QuoteSheetNotes = 
                            "While most of the operators in LINQ let you get one output element for each element you have in your sequence, or they let you filter out elements, but SelectMany is the only operator that lets you produce n output elements for each element in your input sequence. This fact opens up all kinds of possibilities with LINQ that otherwise wouldn’t be available to you. I hope this helps you out!",
                        Options = new List<Option> {o1, o2},
                        Underwriter = "AED",
						Domicile = "US",
						Leader = "TAL",
                        Brokerage = 5.6m  
                    };
                context.Submissions.Add(s);

                var s1 = new Submission
                    {
                        Title = "SOCIEDAD ELECTRICA",
                        InsuredName = "SOCIEDAD ELECTRICA DEL SUR OESTE SA",
                        BrokerCode = "1130",
                        BrokerPseudonym = "CMT",
                        BrokerContact = "Alan Stewart",
						QuoteSheetNotes =
                            "LINQ SelectMany operator. The LINQ SelectMany operator is one of the most useful, misunderstood, and underused operators in your LINQ repertoire. In my previous post I gave you a decent idea of what you can do with the LINQ SelectMany operator, but I’m not quite sure that I did a very good job at really showing you how it works. In this post, I want to give you a more visual explanation of the LINQ SelectMany operator, and what it can do for you.",
                        Options = new List<Option> {o3},
                        Underwriter = "DGB",
                        Domicile = "US",
						Leader = "TAL",
                        Brokerage = 5.6m  
                    };
                context.Submissions.Add(s1);

                var lon = new Office { Id = "LON", Title = "London", Footer = "Talbot Underwriting Ltd is Authorised by the Prudential Regulation Authority and regulated by the Financial Conduct Authority and the Prudential Regulation Authority.", Address = new Address { AddressLine1 = "LondonLine1", AddressLine2 = "LondonLine2", City = "London", ZipPostalCode = "EC2R 8HP" } };
                var mia = new Office { Id = "MIA", Title = "Miami", Address = new Address { AddressLine1 = "MiamiLine1", AddressLine2 = "MiamiLine2", City = "Miami", ZipPostalCode = "MMMMMMM" } };
                var lab = new Office { Id = "LAB", Title = "Labuan", Address = new Address { AddressLine1 = "LabuanLine1", AddressLine2 = "LabuanLine2", City = "Labuan", ZipPostalCode = "LLLLLL" } };
                var nyc = new Office { Id = "NYC", Title = "New York", Address = new Address { AddressLine1 = "NewYorkLine1", AddressLine2 = "NewYorkLine2", City = "NewYork", ZipPostalCode = "NYYYYYY" } };
                var sng = new Office { Id = "SNG", Title = "Singapore", Address = new Address { AddressLine1 = "SingaporeLine1", AddressLine2 = "SingaporeLine2", City = "Singapore", ZipPostalCode = "SNGSNG" } };
                context.Offices.Add(lon);
                context.Offices.Add(mia);
                context.Offices.Add(lab);
                context.Offices.Add(nyc);
                context.Offices.Add(sng);

                // Users

                var addUser1 = new User { DomainLogon = domainPrefix + @"\DaviesA", IsActive = true, UnderwriterId = "AED", PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                //developers
                

                var globalTim = new User
                {
                    DomainLogon = domainPrefix + @"\tim",
                    IsActive = true,
                    AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                    AdditionalUsers = new List<User> { addUser1 },
                    OpenTabs =
                        new List<Tab> { new Tab() { Url = "/Submission/_Create" }, new Tab() { Url = "/Submission/_Edit/1" } },
                    UnderwriterId = "DGB",
                    PrimaryOffice = lon
                };
                context.Users.Add(globalTim);

                var u2 = new User
                    {
                        DomainLogon = domainPrefix + @"\baillief",
                        IsActive = true,
                        FilterCOBs = new List<COB> {c, c1},
                        FilterOffices = new List<Office> { lon },
                        AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                        AdditionalUsers = new List<User> { addUser1 },
                        OpenTabs =
                            new List<Tab> { new Tab() { Url = "/Submission/_Create" }, new Tab() { Url = "/Submission/_Edit/1" } },
                        PrimaryOffice = lon
                    };
                context.Users.Add(u2);

				var u4 = new User
				    {
                        DomainLogon = domainPrefix + @"\SheppaA",
                        AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                        AdditionalUsers = new List<User> { addUser1 },
				        IsActive = true,
				        OpenTabs = new List<Tab> {new Tab() {Url = "/Submission/_Create"}, new Tab() {Url = "/Submission/_Edit/1"}},
                        UnderwriterId = "MM",
                        PrimaryOffice = lon
				    };
                u4.FilterMembers = new List<User> {u4};
                context.Users.Add(u4);

                var u3 = new User
                {
                    DomainLogon = domainPrefix + @"\MurrayE",
                    AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc },
                    AdditionalUsers = new List<User> { addUser1 },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab() { Url = "/Submission/_Create" }, new Tab() { Url = "/Submission/_Edit/1" } },
                    UnderwriterId = "CDC",
                    PrimaryOffice = lon
                };

                var u = new User
                {
                    DomainLogon = domainPrefix + @"\seigelj",
                    IsActive = true,
                    FilterCOBs = new List<COB> {AR,AG,CC,AT},
                    FilterOffices = new List<Office> { lon, mia, sng, lab, nyc },
                    FilterMembers = new List<User> {u2,u4,u3,globalTim},
                    OpenTabs =
                        new List<Tab> { new Tab() { Url = "/Submission/_Create" }, new Tab() { Url = "/Submission/_Edit/1" } },
                    UnderwriterId = "DGB",
                    PrimaryOffice = lon
                };
                context.Users.Add(u);
                u2.AdditionalUsers = new List<User> {u};

                // Energy
                var energyUser1 = new User { DomainLogon = domainPrefix + @"\McDonaldJ", IsActive = true, UnderwriterId = "JMC", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon }; //, FilterCOBs = energyCobs };
                var energyUser2 = new User { DomainLogon = domainPrefix + @"\sarjeat", IsActive = true, UnderwriterId = "AJS", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser3 = new User { DomainLogon = domainPrefix + @"\MassieZ", IsActive = true, UnderwriterId = "ZAM", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser4 = new User { DomainLogon = domainPrefix + @"\CantwellJ", IsActive = true, UnderwriterId = "JBC", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser5 = new User { DomainLogon = domainPrefix + @"\GreenM", IsActive = true, UnderwriterId = "MRG", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser6 = new User { DomainLogon = domainPrefix + @"\EwingtonJ", IsActive = true, UnderwriterId = "JDE", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser7 = new User { DomainLogon = domainPrefix + @"\ShilingS", IsActive = true, UnderwriterId = "SFS", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser8 = new User { DomainLogon = domainPrefix + @"\GarrettJ", IsActive = true, UnderwriterId = "JCG", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser9 = new User { DomainLogon = domainPrefix + @"\StoopE", IsActive = true, UnderwriterId = "ERS", FilterCOBs = new List<COB> { AR, AG, CC, AT }, FilterOffices = new List<Office> { lon, mia, sng, lab, nyc }, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser10 = new User { DomainLogon = domainPrefix + @"\KeoganA", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser11 = new User { DomainLogon = domainPrefix + @"\ShawI", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser12 = new User { DomainLogon = domainPrefix + @"\IsmailR", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser13 = new User { DomainLogon = domainPrefix + @"\SibleyA", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser14 = new User { DomainLogon = domainPrefix + @"\DaviesK", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser15 = new User { DomainLogon = domainPrefix + @"\orsoc", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };
                var energyUser16 = new User { DomainLogon = domainPrefix + @"\dempsef", AdditionalOffices = new List<Office> { lon, mia, sng, lab, nyc }, IsActive = true, PrimaryOffice = lon };//, FilterCOBs = energyCobs };


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
                var teamList = CreateTeamList();

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

                var energyTeamMemberships = energyTeamUsersList.Select(energyTeamUser => new TeamMembership {User = energyTeamUser, Team = teamList[1], IsCurrent = true, StartDate = DateTime.Now, EndDate = null}).ToList();

                var teamMemberships = new List<TeamMembership> 
                { 
                    new TeamMembership { User = u3, Team = teamList[0], IsCurrent = true, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(4)  },
                };

				var memb1 = new TeamMembership { Team = teamList[0], User = u, StartDate = DateTime.Now };
				var memb3 = new TeamMembership { Team = teamList[0], User = u4, StartDate = DateTime.Now };
                var memb2 = new TeamMembership { Team = teamList[0], User = u2, StartDate = DateTime.Now };
                var memb4 = new TeamMembership { Team = teamList[0], User = globalTim, StartDate = DateTime.Now };

                teamList[0].RelatedCOBs = new List<COB> {AR, AG, CC, AT};
                teamList[1].RelatedCOBs = new List<COB> { AR, AG, CC, AT };

                teamList[0].Memberships = new List<TeamMembership> {memb1, memb2, memb4, memb3};

                foreach(var teamM in teamMemberships)
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
                    context.Teams.Add(team);
                }

                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
            }
        }
		*/
        private static List<Link> CreateLinksList()
        {
            return new List<Link> { 
                new Link { Url = "http://docs.talbotuw.com/default.aspx", Title="DMS", Category = "Doc Management"  },
                new Link { Url = "https://acord.validusholdings.com/MMT/MessageSummary.aspx", Title="MMT - e-endorsements", Category = "e-business"  },
                new Link { Url = "https://www.ri3k.com/marketplace/login/index.html", Title="Qatarlyst", Category = "e-business"  },
                new Link { Url = "http://intranet.validusholdings.com/BI/SitePages/Regular%20Talbot%20Reports.aspx", Title="Weekly management reports", Category = "Reports"  },
                new Link { Url = "http://ireport:8700/talbot/getfolderitems.do?folder=/UW/Monthly%20UW%20Figures", Title="Class summaries & triangles", Category = "Reports"  },
                new Link { Url = "https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do", Title="Market wordings database - Info", Category = "Information"  },
                new Link { Url = "http://apps.talbotuw.com/sites/workflow/pages/underwritingworkflow.aspx", Title="Market wordings database - Workflow", Category = "Workflow"  },
                new Link { Url = "http://www.lloyds.com/The-Market/Tools-and-Resources/Tools-E-Services/Crystal", Title="Crystal - Compliance Tools", Category = "Compliance Tools"  },
                new Link { Url = "http://uktrms01.talbotuw.com/Wiki/EM_Wiki/Aggregate_Reports/Summary/ExposureDashboard.html", Title="WOL/Terror hotspot report", Category = "Reports"  },
                new Link { Url = "http://intranet.validusholdings.com/BI/_layouts/ReportServer/RSViewerPage.aspx?rv:RelativeReportUrl=/bi/talbotreports/renewal%20monitor.rdl", Title="Renewal Report", Category = "Reports"  }
            };
        }

        private static List<Team> CreateTeamList()
        {
            
            return new List<Team> {
                new Team { Title = "Developers", QuoteExpiryDaysDefault = 30 },
                new Team { Title = "Energy", QuoteExpiryDaysDefault = 30 }
            };
        }
    }
}