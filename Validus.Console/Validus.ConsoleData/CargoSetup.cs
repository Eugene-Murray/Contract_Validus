using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.Data;
using Validus.Models;

namespace Validus.ConsoleData
{
    public class CargoSetup:TeamSetup
    {
        public CargoSetup(IConsoleRepository consoleRepository)
            : base(consoleRepository)
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
        // ReSharper disable InconsistentNaming
        private TermsNConditionWording TnC01;   //•Non-Binding terms open for 30 days.
        private TermsNConditionWording TnC02;   //•Excluding Chemical/Biological/Cyber Terrorism as per CL370/380.
        private TermsNConditionWording TnC03;   //•Subject to the Law of England and Wales and the exclusive jurisdiction of the court of England and Wales.
        private TermsNConditionWording TnC04;   //•No Cover Given.
        private TermsNConditionWording TnC05;   //•JLC2010 005.
        // ReSharper restore InconsistentNaming
        public override void SetUpTermsNCondition()
        {
            TnC01 = _consoleRepository.Query<TermsNConditionWording>(tnc => tnc.Title == "Non-Binding terms open for 30 days").SingleOrDefault();
            TnC01 = TnC01 ?? new TermsNConditionWording { WordingRefNumber = "", Title = "Non-Binding terms open for 30 days" };

            TnC02 = _consoleRepository.Query<TermsNConditionWording>(tnc => tnc.Title == "Excluding Chemical/Biological/Cyber Terrorism as per CL370/380").SingleOrDefault();
            TnC02 = TnC02 ?? new TermsNConditionWording { WordingRefNumber = "", Title = "Excluding Chemical/Biological/Cyber Terrorism as per CL370/380" };

            TnC03 = _consoleRepository.Query<TermsNConditionWording>(tnc => tnc.Title == "Subject to the Law of England and Wales and the exclusive jurisdiction of the court of England and Wales").SingleOrDefault();
            TnC03 = TnC03 ?? new TermsNConditionWording { WordingRefNumber = "", Title = "Subject to the Law of England and Wales and the exclusive jurisdiction of the court of England and Wales" };

            TnC04 = _consoleRepository.Query<TermsNConditionWording>(tnc => tnc.Title == "No Cover Given").SingleOrDefault();
            TnC04 = TnC04 ?? new TermsNConditionWording { WordingRefNumber = "", Title = "No Cover Given" };

            TnC05 = _consoleRepository.Query<TermsNConditionWording>(tnc => tnc.Title == "JLC2010 005").SingleOrDefault();
            TnC05 = TnC05 ?? new TermsNConditionWording { WordingRefNumber = "", Title = "JLC2010 005" };
        } 
        #endregion

        public override void SetUpLinks()
        {
            //throw new NotImplementedException();//Product Backlog Item 889: Insufficient  Requirement
        }

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
        private User User10;
        private User User11;
        private User User12;
        private User User13;
        private User User14;
        // ReSharper restore InconsistentNaming
        public override void SetUpUser()
        {
            //London	David Silk  DZS
            User01 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\SilkD").SingleOrDefault();
            if (User01 == null)
            {
                User01 = new User
                {
                    DomainLogon = DomainPrefix + @"\SilkD", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "DZS",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw=>uw.Code=="DZS").SingleOrDefault()?? new Underwriter
                    {
                        Name = "David Silk",
                        Code = "DZS",
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
            //London	Emma Thornton-Jones ETJ
            User02 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\ThornE").SingleOrDefault();
            if (User02 == null)
            {
                User02 = new User
                {
                    DomainLogon = DomainPrefix + @"\ThornE", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "ETJ",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "ETJ").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Emma Thornton-Jones",
                        Code = "ETJ",
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
            //London	Alfie Hibbard   
            User03 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\HibbarA").SingleOrDefault();
            if (User03 == null)
            {
                User03 = new User
                {
                    DomainLogon = DomainPrefix + @"\HibbarA", //<??>
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
                _consoleRepository.Add(User03);
            }
            //London	Brian James BZJ
            User04 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\JamesBr").SingleOrDefault();
            if (User04 == null)
            {
                User04 = new User
                {
                    DomainLogon = DomainPrefix + @"\JamesBr", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "BZJ",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "BZJ").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Brian James",
                        Code = "BZJ",
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
                _consoleRepository.Add(User04);
            }
            //London	Joanne Fennell JZF
            User05 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\FennellJ").SingleOrDefault();
            if (User05 == null)
            {
                User05 = new User
                {
                    DomainLogon = DomainPrefix + @"\FennellJ", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "AYD",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "JZF").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Joanne Fennell",
                        Code = "JZF",
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
                _consoleRepository.Add(User05);
            }
            //London	Ed Colclough    EWC
            User06 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\ColclouC").SingleOrDefault();
            if (User06 == null)
            {
                User06 = new User
                {
                    DomainLogon = DomainPrefix + @"\ColclouC", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "AYD",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "EWC").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Ed Colclough",
                        Code = "EWC",
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
            //London	Graham McManus  GTM
            User07 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\McManuG").SingleOrDefault();
            if (User07 == null)
            {
                User07 = new User
                {
                    DomainLogon = DomainPrefix + @"\McManuG", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "AYD",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "GTM").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Graham McManus",
                        Code = "GTM",
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
            //Singapore	Janita Leu  JTL
            User08 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\LeuJa").SingleOrDefault();
            if (User08 == null)
            {
                User08 = new User
                {
                    DomainLogon = DomainPrefix + @"\LeuJa", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "JTL",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "SNG").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "JTL").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Janita Leu",
                        Code = "JTL",
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
            //Singapore	Li Lin Kea
            User09 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\keall").SingleOrDefault();
            if (User09 == null)
            {
                User09 = new User
                {
                    DomainLogon = DomainPrefix + @"\keall", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "SNG").Single(),
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                };
                _consoleRepository.Add(User09);
            }
            //Singapore	Sharon Seah SHS
            User10 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\SeahS").SingleOrDefault();
            if (User10 == null)
            {
                User10 = new User
                {
                    DomainLogon = DomainPrefix + @"\SeahS", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "SHS",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "SNG").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "SHS").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Sharon Seah",
                        Code = "SHS",
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
                _consoleRepository.Add(User10);
            }
            //Singapore	Simon Wilmot-Smith  SWS
            User11 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\wilmots").SingleOrDefault();
            if (User11 == null)
            {
                User11 = new User
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
                _consoleRepository.Add(User11);
            }
            //New York	Ella Stoop  ERS
            User12 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\stoope").SingleOrDefault();
            if (User12 == null)
            {
                User12 = new User
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
                _consoleRepository.Add(User12);
            }
            //Miami	Luis Sonville   LAS
            User13 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\Luis.Sonville").SingleOrDefault();
            if (User13 == null)
            {
                User13 = new User
                {
                    DomainLogon = DomainPrefix + @"\Luis.Sonville", //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    UnderwriterCode = "LAS",//<??>
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == "MIA").Single(),
                    Underwriter = _consoleRepository.Query<Underwriter>(uw => uw.Code == "LAS").SingleOrDefault() ?? new Underwriter
                    {
                        Name = "Luis Sonville",
                        Code = "LAS",
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
                _consoleRepository.Add(User13);
            }
            //New York 	Michelle Gatti
            User14 = _consoleRepository.Query<User>(user => user.DomainLogon == DomainPrefix + @"\GattiMi").SingleOrDefault();
            if (User14 == null)
            {
                User14 = new User
                {
                    DomainLogon = DomainPrefix + @"\GattiMi", //<??>
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
                _consoleRepository.Add(User14);
            }
        } 
        #endregion

        #region Team
        // ReSharper disable InconsistentNaming
        private QuoteTemplate QuoteTemplate01;
        private QuoteTemplate QuoteTemplate02;
        private QuoteTemplate QuoteTemplate03;
        private SubmissionType SubmissionType01;
        private Team Team01;
        // ReSharper restore InconsistentNaming

        public override void SetupTeam()
        {
            this.SetUpCob();
            this.SetUpTermsNCondition();
            this.SetUpLinks();
            this.SetUpUser();

            QuoteTemplate01 = _consoleRepository.Query<QuoteTemplate>(qtmpl => qtmpl.Name == "Cargo").SingleOrDefault();
            QuoteTemplate01 = QuoteTemplate01 ?? new QuoteTemplate { Name = "Cargo", RdlPath = "/Underwriting/Console/QuoteSheet_CA" };

            QuoteTemplate02 = _consoleRepository.Query<QuoteTemplate>(qtmpl => qtmpl.Name == "Specie").SingleOrDefault();
            QuoteTemplate02 = QuoteTemplate02 ?? new QuoteTemplate { Name = "Specie", RdlPath = "/Underwriting/Console/QuoteSheet_CA" };

            QuoteTemplate03 = _consoleRepository.Query<QuoteTemplate>(qtmpl => qtmpl.Name == "Fine Art").SingleOrDefault();
            QuoteTemplate03 = QuoteTemplate03 ?? new QuoteTemplate { Name = "Fine Art", RdlPath = "/Underwriting/Console/QuoteSheet_CA" };

            SubmissionType01 = _consoleRepository.Query<SubmissionType>(styp => styp.Id == "CA").SingleOrDefault();
            SubmissionType01 = SubmissionType01 ?? new SubmissionType { Id = "CA", Title = "CA Submission" };



            Team01 = _consoleRepository.Query<Team>(team => team.Title == "Cargo").SingleOrDefault();
            if (Team01 == null)
            {
                Team01 = new Team
                {
                    Title = "Cargo",
                    QuoteExpiryDaysDefault = 30,
                    RelatedCOBs = new List<COB> { COB_AY, COB_AT, COB_AW, COB_CL },
                    Links = new List<Link> {  },
                    RelatedQuoteTemplates = new List<QuoteTemplate> { QuoteTemplate01, QuoteTemplate02, QuoteTemplate03 },
                    Memberships = new Collection<TeamMembership> { new TeamMembership { StartDate = DateTime.Now, User = User01 }, new TeamMembership { StartDate = DateTime.Now, User = User02 }, new TeamMembership { StartDate = DateTime.Now, User = User03 }, new TeamMembership { StartDate = DateTime.Now, User = User04 }, new TeamMembership { StartDate = DateTime.Now, User = User05 }, new TeamMembership { StartDate = DateTime.Now, User = User06 }, new TeamMembership { StartDate = DateTime.Now, User = User07 }, new TeamMembership { StartDate = DateTime.Now, User = User08 }, new TeamMembership { StartDate = DateTime.Now, User = User09 }, new TeamMembership { StartDate = DateTime.Now, User = User10 }, new TeamMembership { StartDate = DateTime.Now, User = User11 }, new TeamMembership { StartDate = DateTime.Now, User = User12 }, new TeamMembership { StartDate = DateTime.Now, User = User13 }, new TeamMembership { StartDate = DateTime.Now, User = User14 } },
                    RelatedOffices = new List<Office>
                        {
                            _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                            _consoleRepository.Query<Office>(off => off.Id == "MIA").Single(),
                            _consoleRepository.Query<Office>(off => off.Id == "LAB").Single(),
                            _consoleRepository.Query<Office>(off => off.Id == "NYC").Single(),
                            _consoleRepository.Query<Office>(off => off.Id == "SNG").Single(),
                            _consoleRepository.Query<Office>(off => off.Id == "DUB").Single(),
                        },
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now,
                    SubmissionTypeId = SubmissionType01.Id,
                    DefaultPolicyType = "MARINE"
                };

                var teamOfficeSetting01 = new TeamOfficeSetting
                    {
                        Team = Team01,
                        Office = _consoleRepository.Query<Office>(off => off.Id == "LON").Single(),
                        TermsNConditionWordingSettings =
                            new Collection<TermsNConditionWordingSetting>
                                {
                                    new TermsNConditionWordingSetting {DisplayOrder = 1, TermsNConditionWording = TnC01},
                                    new TermsNConditionWordingSetting {DisplayOrder = 2, TermsNConditionWording = TnC02},
                                    new TermsNConditionWordingSetting {DisplayOrder = 3, TermsNConditionWording = TnC03},
                                    new TermsNConditionWordingSetting {DisplayOrder = 4, TermsNConditionWording = TnC04},
                                    new TermsNConditionWordingSetting {DisplayOrder = 5, TermsNConditionWording = TnC05}
                                }
                    };
                var teamOfficeSetting02 = new TeamOfficeSetting
                {
                    Team = Team01,
                    Office = _consoleRepository.Query<Office>(off => off.Id == "MIA").Single(),
                    TermsNConditionWordingSettings =
                        new Collection<TermsNConditionWordingSetting>
                                {
                                    new TermsNConditionWordingSetting {DisplayOrder = 1, TermsNConditionWording = TnC01},
                                    new TermsNConditionWordingSetting {DisplayOrder = 2, TermsNConditionWording = TnC02},
                                    new TermsNConditionWordingSetting {DisplayOrder = 3, TermsNConditionWording = TnC03},
                                    new TermsNConditionWordingSetting {DisplayOrder = 4, TermsNConditionWording = TnC04},
                                    new TermsNConditionWordingSetting {DisplayOrder = 5, TermsNConditionWording = TnC05}
                                }
                };
                var teamOfficeSetting03 = new TeamOfficeSetting
                {
                    Team = Team01,
                    Office = _consoleRepository.Query<Office>(off => off.Id == "LAB").Single(),
                    TermsNConditionWordingSettings =
                        new Collection<TermsNConditionWordingSetting>
                                {
                                    new TermsNConditionWordingSetting {DisplayOrder = 1, TermsNConditionWording = TnC01},
                                    new TermsNConditionWordingSetting {DisplayOrder = 2, TermsNConditionWording = TnC02},
                                    new TermsNConditionWordingSetting {DisplayOrder = 3, TermsNConditionWording = TnC03},
                                    new TermsNConditionWordingSetting {DisplayOrder = 4, TermsNConditionWording = TnC04},
                                    new TermsNConditionWordingSetting {DisplayOrder = 5, TermsNConditionWording = TnC05}
                                }
                };
                var teamOfficeSetting04 = new TeamOfficeSetting
                {
                    Team = Team01,
                    Office = _consoleRepository.Query<Office>(off => off.Id == "NYC").Single(),
                    TermsNConditionWordingSettings =
                        new Collection<TermsNConditionWordingSetting>
                                {
                                    new TermsNConditionWordingSetting {DisplayOrder = 1, TermsNConditionWording = TnC01},
                                    new TermsNConditionWordingSetting {DisplayOrder = 2, TermsNConditionWording = TnC02},
                                    new TermsNConditionWordingSetting {DisplayOrder = 3, TermsNConditionWording = TnC03},
                                    new TermsNConditionWordingSetting {DisplayOrder = 4, TermsNConditionWording = TnC04},
                                    new TermsNConditionWordingSetting {DisplayOrder = 5, TermsNConditionWording = TnC05}
                                }
                };
                var teamOfficeSetting05 = new TeamOfficeSetting
                {
                    Team = Team01,
                    Office = _consoleRepository.Query<Office>(off => off.Id == "SNG").Single(),
                    TermsNConditionWordingSettings =
                        new Collection<TermsNConditionWordingSetting>
                                {
                                    new TermsNConditionWordingSetting {DisplayOrder = 1, TermsNConditionWording = TnC01},
                                    new TermsNConditionWordingSetting {DisplayOrder = 2, TermsNConditionWording = TnC02},
                                    new TermsNConditionWordingSetting {DisplayOrder = 3, TermsNConditionWording = TnC03},
                                    new TermsNConditionWordingSetting {DisplayOrder = 4, TermsNConditionWording = TnC04},
                                    new TermsNConditionWordingSetting {DisplayOrder = 5, TermsNConditionWording = TnC05}
                                }
                };
                var teamOfficeSetting06 = new TeamOfficeSetting
                {
                    Team = Team01,
                    Office = _consoleRepository.Query<Office>(off => off.Id == "DUB").Single(),
                    TermsNConditionWordingSettings =
                        new Collection<TermsNConditionWordingSetting>
                                {
                                    new TermsNConditionWordingSetting {DisplayOrder = 1, TermsNConditionWording = TnC01},
                                    new TermsNConditionWordingSetting {DisplayOrder = 2, TermsNConditionWording = TnC02},
                                    new TermsNConditionWordingSetting {DisplayOrder = 3, TermsNConditionWording = TnC03},
                                    new TermsNConditionWordingSetting {DisplayOrder = 4, TermsNConditionWording = TnC04},
                                    new TermsNConditionWordingSetting {DisplayOrder = 5, TermsNConditionWording = TnC05}
                                }
                };

                Team01.TeamOfficeSettings = new Collection<TeamOfficeSetting> { teamOfficeSetting01, 
                    teamOfficeSetting02, 
                    teamOfficeSetting03, 
                    teamOfficeSetting04, 
                    teamOfficeSetting05, 
                    teamOfficeSetting06};

                _consoleRepository.Add(Team01);
            }

            _consoleRepository.SaveChanges();

        }
        #endregion
    }
}
