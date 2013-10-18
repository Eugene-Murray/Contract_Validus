using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Validus.Console.Data;
using Validus.Models;


namespace Validus.ConsoleData
{
    public class HullSetup : TeamSetup
    {
       
        public HullSetup(IConsoleRepository consoleRepository): base(consoleRepository)
        {

        }

        #region COB
        // ReSharper disable InconsistentNaming
        private COB COB_AY; //AY:   Direct - Property - Yachts
        private COB COB_AT; //AT:   Direct - Property - Hull
        private COB COB_AW; //AW:  Direct - Property - War
        private COB COB_CL; //CL:   Direct - Casualty - Marine Liability 
        // ReSharper restore InconsistentNaming
        public override void SetUpCob()
        {
            COB_AY = _consoleRepository.Query<COB>(cob => cob.Id == "AY").SingleOrDefault();
            COB_AY = COB_AY ?? new COB { Id = "AY", Narrative = "Direct - Property - Yachts", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            COB_AT = _consoleRepository.Query<COB>(cob => cob.Id == "AT").SingleOrDefault();
            COB_AT = COB_AT ?? new COB { Id = "AT", Narrative = "Direct - Property - Hull", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            COB_AW = _consoleRepository.Query<COB>(cob => cob.Id == "AW").SingleOrDefault();
            COB_AW = COB_AW ?? new COB { Id = "AW", Narrative = "Direct - Property - War", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

            COB_CL = _consoleRepository.Query<COB>(cob => cob.Id == "CL").SingleOrDefault();
            COB_CL = COB_CL ?? new COB { Id = "CL", Narrative = "Direct - Casualty - Marine Liability", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
        } 
        #endregion

        #region Wording
        public override void SetUpTermsNCondition()
        {


        } 
        #endregion

        #region Link
        // ReSharper disable InconsistentNaming
        private Link Link_01; //DMS http://docs.talbotuw.com/default.aspx
        private Link Link_02; //MMT https://acord.validusholdings.com/MMT/MessageSummary.aspx
        private Link Link_03; //Qatarlyst https://www.ri3k.com/marketplace/login/index.html
        private Link Link_04; //Tamarine G:\Underwriting\Marine\TAMARINE\Tamarine v102.01.xlsm
        private Link Link_05; //Marine War Model G:\Underwriting\Marine\TAMARINE\Marine War\Marine War_v102.02.xlsm
        private Link Link_06; //Hull Pricing Pro Forma G:\Underwriting\Marine\TAMARINE\Pro Forma 2013 Risks.xlsx
        private Link Link_07; //Weekly Management Reports http://intranet.validusholdings.com/BI/SitePages/Regular%20Talbot%20Reports.aspx
        private Link Link_08; //Class Summaries & Triangles http://ireport:8700/talbot/getfolderitems.do?folder=/UW/Monthly%20UW%20Figures
        private Link Link_09; //Market wordings database https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do
        private Link Link_10; //Market wordings database - Workflow https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do
        private Link Link_11; //Workflow Dashboard http://apps.talbotuw.com/sites/workflow/pages/underwritingworkflow.aspx
        private Link Link_12; //Crystal http://www.lloyds.com/The-Market/Tools-and-Resources/Tools-E-Services/Crystal
        private Link Link_13; //XE Live Exchange Rates www.xe.com
        private Link Link_14; //Weekly Underwriter Pack http://intranet.validusholdings.com/Underwriting/Talbot/SitePages/UWPack.aspx
        private Link Link_15; //Proclarity 
        private Link Link_16; //Worldcheck http://worldcheck.talbotuw.com/Default.aspx
        private Link Link_17; //Renewals http://intranet.validusholdings.com/BI/_layouts/ReportServer/RSViewerPage.aspx?rv:RelativeReportUrl=/bi/talbotreports/renewal%20monitor.rdl
        private Link Link_18; //Sea-Web http://www.sea-web.com/authenticated/seaweb_subscriber_welcome.aspx
        private Link Link_19; //Trade Winds http://www.tradewindsnews.com/
        private Link Link_20; //World Port Source http://www.worldportsource.com/
        // ReSharper restore InconsistentNaming
        public override void SetUpLinks()
        {

            Link_01 = _consoleRepository.Query<Link>(lnk => lnk.Title == "DMS").SingleOrDefault();
            Link_01 = Link_01 ?? new Link { Category = "Document Management", Title = "DMS", Url = "http://docs.talbotuw.com/default.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_02 = _consoleRepository.Query<Link>(lnk => lnk.Title == "MMT").SingleOrDefault();
            Link_02 = Link_02 ?? new Link { Category = "ebusiness", Title = "MMT", Url = "https://acord.validusholdings.com/MMT/MessageSummary.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_03 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Qatarlyst").SingleOrDefault();
            Link_03 = Link_03 ?? new Link { Category = "eBusiness", Title = "Qatarlyst", Url = "https://www.ri3k.com/marketplace/login/index.html", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_04 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Tamarine").SingleOrDefault();
            Link_04 = Link_04 ?? new Link { Category = "Pricing Models", Title = "Tamarine", Url = @"G:\Underwriting\Marine\TAMARINE\Tamarine v102.01.xlsm", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_05 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Marine War Model").SingleOrDefault();
            Link_05 = Link_05 ?? new Link { Category = "Pricing Models", Title = "Marine War Model", Url = @"G:\Underwriting\Marine\TAMARINE\Marine War\Marine War_v102.02.xlsm", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_06 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Hull Pricing Pro Forma").SingleOrDefault();
            Link_06 = Link_06 ?? new Link { Category = "Pricing Models", Title = "Hull Pricing Pro Forma", Url = @"G:\Underwriting\Marine\TAMARINE\Pro Forma 2013 Risks.xlsx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_07 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Weekly Management Reports").SingleOrDefault();
            Link_07 = Link_07 ?? new Link { Category = "Reports", Title = "Weekly Management Reports", Url = "http://intranet.validusholdings.com/BI/SitePages/Regular%20Talbot%20Reports.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_08 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Class Summaries & Triangles").SingleOrDefault();
            Link_08 = Link_08 ?? new Link { Category = "Reports", Title = "Class Summaries & Triangles", Url = "http://ireport:8700/talbot/getfolderitems.do?folder=/UW/Monthly%20UW%20Figures", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_09 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Market wordings database").SingleOrDefault();
            Link_09 = Link_09 ?? new Link { Category = "Information", Title = "Market wordings database", Url = "https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_10 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Market wordings database - Workflow").SingleOrDefault();
            Link_10 = Link_10 ?? new Link { Category = "Information", Title = "Market wordings database - Workflow", Url = "https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_11 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Workflow Dashboard").SingleOrDefault();
            Link_11 = Link_11 ?? new Link { Category = "Workflow", Title = "Workflow Dashboard", Url = "http://apps.talbotuw.com/sites/workflow/pages/underwritingworkflow.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_12 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Crystal").SingleOrDefault();
            Link_12 = Link_12 ?? new Link { Category = "Compliance Tools", Title = "Crystal", Url = "http://www.lloyds.com/The-Market/Tools-and-Resources/Tools-E-Services/Crystal", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_13 = _consoleRepository.Query<Link>(lnk => lnk.Title == "XE Live Exchange Rates").SingleOrDefault();
            Link_13 = Link_13 ?? new Link { Category = "Information", Title = "XE Live Exchange Rates", Url = "www.xe.com", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_14 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Weekly Underwriter Pack").SingleOrDefault();
            Link_14 = Link_14 ?? new Link { Category = "Information", Title = "Weekly Underwriter Pack", Url = "http://intranet.validusholdings.com/Underwriting/Talbot/SitePages/UWPack.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_15 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Proclarity").SingleOrDefault();
            Link_15 = Link_15 ?? new Link { Category = "Reports", Title = "Proclarity", Url = "NA", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_16 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Worldcheck").SingleOrDefault();
            Link_16 = Link_16 ?? new Link { Category = "Compliance Tools", Title = "Worldcheck", Url = "http://worldcheck.talbotuw.com/Default.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_17 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Renewals").SingleOrDefault();
            Link_17 = Link_17 ?? new Link { Category = "Reports", Title = "Renewals", Url = "http://intranet.validusholdings.com/BI/_layouts/ReportServer/RSViewerPage.aspx?rv:RelativeReportUrl=/bi/talbotreports/renewal%20monitor.rdl", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_18 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Sea-Web").SingleOrDefault();
            Link_18 = Link_18 ?? new Link { Category = "Information", Title = "Sea-Web", Url = "http://www.sea-web.com/authenticated/seaweb_subscriber_welcome.aspx", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_19 = _consoleRepository.Query<Link>(lnk => lnk.Title == "Trade Winds").SingleOrDefault();
            Link_19 = Link_19 ?? new Link { Category = "Information", Title = "Trade Winds", Url = "http://www.tradewindsnews.com/", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
            Link_20 = _consoleRepository.Query<Link>(lnk => lnk.Title == "World Port Source").SingleOrDefault();
            Link_20 = Link_20 ?? new Link { Category = "Information", Title = "World Port Source", Url = "http://www.worldportsource.com/", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };

        }
        
        #endregion

        #region User
        // ReSharper disable InconsistentNaming
        private User User01;
        private User User02;
        private User User03;
        private User User04;
        private User User05;
        private User User06;
        private User User07;
        private User User08;
        private User User09;
        // ReSharper restore InconsistentNaming
        public override void SetUpUser()
        {
            User01 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\DaviAnn").SingleOrDefault();
            if (User01 == null)
            {
                User01 = new User
                {
                    DomainLogon = DomainPrefix + @"\DaviAnn", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "AYD",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "AYD").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Alex Colquhoun",
                        Code = "AYD",
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
                _consoleRepository.Add(User01);
            }

            User02 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\maccolm").SingleOrDefault();
            if (User02 == null)
            {
                User02 = new User
                {
                    DomainLogon = DomainPrefix + @"\maccolm", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "MDM",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "MDM").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Mike MacColl",
                        Code = "MDM",
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
                _consoleRepository.Add(User02);
            }

            User03 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\carpene").SingleOrDefault();
            if (User03 == null)
            {
                User03 = new User
                {
                    DomainLogon = DomainPrefix + @"\carpene", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "EJC",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "EJC").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Edward Carpenter",
                        Code = "EJC",
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
                _consoleRepository.Add(User03);
            }

            User04 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\ewenb").SingleOrDefault();
            if (User04 == null)
            {
                User04 = new User
                {
                    DomainLogon = DomainPrefix + @"\ewenb", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                };
                _consoleRepository.Add(User04);
            }

            /* This user is not showong in ADS*/
            User05 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\PalmerEl").SingleOrDefault();
            if (User05 == null)
            {
                User05 = new User
                {
                    DomainLogon = DomainPrefix + @"\PalmerEl", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                };
                _consoleRepository.Add(User05);
            }

            User06 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\stoope").SingleOrDefault();
            if (User06 == null)
            {
                User06 = new User
                {
                    DomainLogon = DomainPrefix + @"\stoope", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "ERS",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "NYC").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "ERS").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Ella Stoop",
                        Code = "ERS",
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
                _consoleRepository.Add(User06);
            }

            User07 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\wilmots").SingleOrDefault();
            if (User07 == null)
            {
                User07 = new User
                {
                    DomainLogon = DomainPrefix + @"\wilmots", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "SWS",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "SNG").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "SWS").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Simon Wilmot-Smith",
                        Code = "SWS",
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
                _consoleRepository.Add(User07);
            }

            User08 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\GarrattJ").SingleOrDefault();
            if (User08 == null)
            {
                User08 = new User
                {
                    DomainLogon = DomainPrefix + @"\GarrattJ", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "JCG",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "DUB").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "JCG").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "James Garratt",
                        Code = "JCG",
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
                _consoleRepository.Add(User08);
            }

            User09 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\GattiMi").SingleOrDefault();
            if (User09 == null)
            {
                User09 = new User
                {
                    DomainLogon = DomainPrefix + @"\GattiMi", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "NYC").Single(),
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                };
                _consoleRepository.Add(User09);
            }

        } 
        #endregion

        #region Team
        // ReSharper disable InconsistentNaming
        private QuoteTemplate QuoteTemplate01;
        private SubmissionType SubmissionType01;
        private Team Team01;
        // ReSharper restore InconsistentNaming
        public override void SetupTeam()
        {
            this.SetUpCob();
            this.SetUpTermsNCondition();
            this.SetUpLinks();
            this.SetUpUser();

            QuoteTemplate01 = _consoleRepository.Query<QuoteTemplate>(qtmpl => qtmpl.Name == "Hull&Marine").SingleOrDefault();
            QuoteTemplate01 = QuoteTemplate01 ?? new QuoteTemplate { Name = "Hull&Marine", RdlPath = "/Underwriting/Console/QuoteSheet_HM" };

            SubmissionType01 = _consoleRepository.Query<SubmissionType>(styp => styp.Id == "HM").SingleOrDefault();
            SubmissionType01 = SubmissionType01 ?? new SubmissionType { Id = "HM", Title = "HM Submission" };

            Team01 = _consoleRepository.Query<Team>(team => team.Title == "Hull&Marine").SingleOrDefault();
            if (Team01 == null)
            {
                Team01 = new Team
                {
                    Title = "Hull&Marine",
                    QuoteExpiryDaysDefault = 14,
                    RelatedCOBs = new List<COB> { COB_AY, COB_AT, COB_AW, COB_CL },
                    Links = new List<Link> { Link_01, Link_02, Link_03, Link_04, Link_05, Link_06, Link_07, Link_08, Link_09, Link_10, Link_11, Link_12, Link_13, Link_14, Link_15, Link_16, Link_17, Link_18, Link_19, Link_20 },
                    RelatedQuoteTemplates = new List<QuoteTemplate> { QuoteTemplate01 },
                    Memberships = new Collection<TeamMembership> { new TeamMembership { StartDate = DateTime.Now, User = User01 }, new TeamMembership { StartDate = DateTime.Now, User = User02 }, new TeamMembership { StartDate = DateTime.Now, User = User03 }, new TeamMembership { StartDate = DateTime.Now, User = User04 }, new TeamMembership { StartDate = DateTime.Now, User = User05 }, new TeamMembership { StartDate = DateTime.Now, User = User06 }, new TeamMembership { StartDate = DateTime.Now, User = User07 }, new TeamMembership { StartDate = DateTime.Now, User = User08 }, new TeamMembership { StartDate = DateTime.Now, User = User09 } },
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now,
                    SubmissionTypeId = SubmissionType01.Id,
                    DefaultPolicyType = "MARINE"
                };
                _consoleRepository.Add(Team01);
            }

            _consoleRepository.SaveChanges();

        }
        #endregion
    }
}
