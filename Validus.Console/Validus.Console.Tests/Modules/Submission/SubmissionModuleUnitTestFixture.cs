using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.BusinessLogic;
using Validus.Console.Data;
using Validus.Models;
using Validus.Core.HttpContext;
using Validus.Console.Fakes;
using System.Security.Principal;
using Validus.Console.Tests.Helpers;
using Validus.Console.SubscribeService;
using Validus.Core.LogHandling;
using System.Reflection;

namespace Validus.Console.Tests.Modules.Submission
{
    [TestClass]
    public class SubmissionModuleUnitTestFixture
    {
        FakeConsoleRepository _rep;
        ISubmissionModule _submissionModule;
        private IWebSiteModuleManager _webSiteModuleManager;

        [TestInitialize]
        public void Setup()
        {
            _rep = new FakeConsoleRepository();

            COB c = new COB() { Id = "CA", Narrative = "Cargo" };
            Office off = new Office() { Id = "LON", Title = "London" };
            List<User> users = new List<User>();
            User u = new User() { DomainLogon = @"talbotdev\murraye" };
            u.FilterCOBs = new List<COB>();
            u.AdditionalCOBs = new List<COB>();
            u.FilterOffices = new List<Office>();
            u.AdditionalOffices = new List<Office>();
            u.FilterMembers = new List<User>();
            u.AdditionalUsers = new List<User>();

            u.AdditionalCOBs.Add(c);

            users.Add(u);
            _rep.Users = users.AsQueryable();

            List<Validus.Models.Submission> subs = new List<Validus.Models.Submission>();

            for (int i = 0; i < 15; i++)
			{
                subs.Add(new Validus.Models.Submission()
                {
                    Id = i,
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now,
                    InsuredName = (i == 3 || i == 6 ? "Fergus Baillie" : "ALLAN MURRAY"),
                    BrokerCode = String.Format("11{0}", i),
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
                    Options = new List<Option>()                    
                });
            }

            List<Option> options = new List<Option>();
            Option o = new Option() { Title = "Test", SubmissionId = 3, Submission = subs[3] };
            options.Add(o);

            List<OptionVersion> optionVersions = new List<OptionVersion>();
            OptionVersion ov = new OptionVersion() { Title = "Test", Option = o };
            optionVersions.Add(ov);

            List<Quote> quotes = new List<Quote>();
            Quote q = new Quote() { 
                COB = c, COBId = c.Id, 
                OriginatingOffice = off, 
                OriginatingOfficeId = off.Id, 
                EntryStatus = "PARTIAL",
                OptionVersion = ov,
                SubscribeReference = "BAN169784A13"
            };
            
            quotes.Add(q);

            ov.Quotes = quotes;
            o.OptionVersions = optionVersions;
            subs[3].Options = options;

            
            _rep.Submissions = subs.AsQueryable();            

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
                                            new StandardOutput {OutputXml = MvcMockHelpers.CreateGetReferenceResponseXml()}
                                    });
            var mockPolicyData = new Mock<IPolicyData>();
            var context = new Mock<ICurrentHttpContext>();
            var user = @"talbotdev\murraye";
            //user = user.Replace(@"\\", @"\");
            context.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            context.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());

            _webSiteModuleManager = new WebSiteModuleManager(_rep, context.Object);
            _submissionModule = new SubmissionModule(_rep, mockSubscribeService.Object, new LogHandler(), context.Object, _webSiteModuleManager, mockPolicyData.Object);
            //_submissionModule = new SubmissionModule(rep, null, null, cont);
        }

        [TestMethod]
        public void Can_Get_Submissions()
        {
            Int32 total;
            Int32 totalDisplay;
            Object[] subs = _submissionModule.GetSubmissions(null, 0, 10, "InsuredName", "ASC", false, out totalDisplay, out total);

            Assert.IsTrue(totalDisplay == 15);
            Assert.IsTrue(subs.Length > 0);
        }

        [TestMethod]
        public void Can_Get_Page_Two_Of_Submissions()
        {
            //  Assign
            Int32 skip = 10;
            Int32 take = 10;
            Int32 expectedResultLength = 5;

            //  Act
            Int32 total;
            Int32 todaldisplay;
            Object[] results = _submissionModule.GetSubmissions(null, skip, take, "InsuredName", "ASC", false, out todaldisplay, out total);

            // Assert
            Assert.IsTrue(results.Length == expectedResultLength);
        }

        [TestMethod]
        public void Can_Sort_Submissions()
        {
            //  Assign
            Int32 skip = 0;
            Int32 take = 10;
            String sortOrder = "DESC";
            String sortCol = "Id";
            
            //  Act
            Int32 total;
            Int32 todaldisplay;
            Object[] results = _submissionModule.GetSubmissions(null, skip, take, sortCol, sortOrder, false, out todaldisplay, out total);
            Type t = results[0].GetType();
            PropertyInfo p = t.GetProperty("Id");
            Int32 v = Int32.Parse(p.GetValue(results[0], null).ToString());
            
            // Assert
            Assert.IsTrue(v == 14);
        }

        [TestMethod]
        public void Can_Filter_Submissions_By_Search_Term()
        {
            //  Assign
            Int32 skip = 0;
            Int32 take = 10;            
            String searchTerm = "ergus";

            //  Act
            Int32 total;
            Int32 todaldisplay;
            Object[] results = _submissionModule.GetSubmissions(searchTerm, skip, take, "InsuredName", "ASC", false, out todaldisplay, out total);
            Type t = results[0].GetType();
            PropertyInfo p = t.GetProperty("Id");
            Int32 v1 = Int32.Parse(p.GetValue(results[0], null).ToString());
            Int32 v2 = Int32.Parse(p.GetValue(results[1], null).ToString());

            // Assert
            Assert.IsTrue(results.Count() == 2);
            Assert.IsTrue(v1 == 3);
            Assert.IsTrue(v2 == 6);
        }

        [TestMethod]
        public void Can_Filter_Submissions_By_Search_Term_With_Profile_Filters()
        {
            //  Assign
            Int32 skip = 0;
            Int32 take = 10;
            String searchTerm = "ergus";

            //  Act
            Int32 total;
            Int32 todaldisplay;
            Object[] results = _submissionModule.GetSubmissions(searchTerm, skip, take, "Id", "ASC", true, out todaldisplay, out total);
            Type t = results[0].GetType();
            PropertyInfo p = t.GetProperty("Id");
            Int32 v1 = Int32.Parse(p.GetValue(results[0], null).ToString());

            // Assert
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(v1 == 3);
        }

        [TestMethod]
        public void Can_Filter_Submissions_By_Search_Term_PolId()
        {
            //  Assign
            Int32 skip = 0;
            Int32 take = 10;
            String searchTerm = "BAN169784A13";

            //  Act
            Int32 total;
            Int32 todaldisplay;
            Object[] results = _submissionModule.GetSubmissions(searchTerm, skip, take, "InsuredName", "ASC", false, out todaldisplay, out total);
            Type t = results[0].GetType();
            PropertyInfo p = t.GetProperty("Id");
            Int32 v1 = Int32.Parse(p.GetValue(results[0], null).ToString());

            // Assert
            Assert.IsTrue(v1 == 3);
        }
    }
}
