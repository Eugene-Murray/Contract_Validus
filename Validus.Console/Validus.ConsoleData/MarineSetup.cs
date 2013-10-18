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
    public class MarineSetup:TeamSetup
    {

        public MarineSetup(IConsoleRepository consoleRepository)
            : base(consoleRepository)
        {
        
        }
        #region COB

        // ReSharper disable InconsistentNaming
        private COB COB_CL; //CL:  Direct - Casualty - Marine Liability 
        private COB COB_CR; //CR:  Direct Casualty Energy Liability

        private COB COB_AT; //AT:  Direct - Property - Hull
        private COB COB_AV; //AV:  Direct Property Cargo
        private COB COB_AW; //AW:  Direct - Property - War
        private COB COB_AR; //AR:  Direct Energy Rig
        
        // ReSharper restore InconsistentNaming
        public override void SetUpCob()
        {
            COB_CL = ResolveCob("CL", "Direct - Casualty - Marine Liability");
            COB_CR = ResolveCob("CR", "Direct Casualty Energy Liability");
            COB_AT = ResolveCob("AT", "Direct - Property - Hul");
            COB_AV = ResolveCob("AV", "Direct Property Cargo");
            COB_AW = ResolveCob("AW", "Direct - Property - War");
            COB_AR = ResolveCob("AR", "Direct Energy Rig");
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

            TnC01 = ResolveTermsNConditionWording("Non-Binding terms open for 30 days");
            TnC02 = ResolveTermsNConditionWording("Excluding Chemical/Biological/Cyber Terrorism as per CL370/380");
            TnC03 = ResolveTermsNConditionWording("Subject to the Law of England and Wales and the exclusive jurisdiction of the court of England and Wales");
            TnC04 = ResolveTermsNConditionWording("No Cover Given");
            TnC05 = ResolveTermsNConditionWording("JLC2010 005");

        }
        #endregion

        #region Link
        // ReSharper disable InconsistentNaming
        private Link Link_01;
        private Link Link_02;
        private Link Link_03;
        private Link Link_04;
        private Link Link_05;
        private Link Link_06;
        private Link Link_07;
        private Link Link_08;
        private Link Link_09;
        private Link Link_10;
        private Link Link_11;
        private Link Link_12;
        private Link Link_13;
        private Link Link_14;
        private Link Link_15;

        // ReSharper restore InconsistentNaming
        public override void SetUpLinks()
        {
            Link_01	=	ResolveLink("DMS","Document Management","http://docs.talbotuw.com/default.aspx");
            Link_02	=	ResolveLink("MMT","ebusiness","https://acord.validusholdings.com/MMT/MessageSummary.aspx");
            Link_03	=	ResolveLink("Qatarlyst","eBusiness","https://www.ri3k.com/marketplace/login/index.html");
            Link_04	=	ResolveLink("Open Xposure","Exposure Management","Current open Xposure management link as per desktop setup");
            Link_05	=	ResolveLink("Subscribe","Policy Admin","As per standard desktop installation");
            Link_06	=	ResolveLink("Weekly Management Reports","Reports","http://intranet.validusholdings.com/BI/SitePages/Regular%20Talbot%20Reports.aspx");
            Link_07	=	ResolveLink("Class Summaries & Triangles","Reports","http://ireport:8700/talbot/getfolderitems.do?folder=/UW/Monthly%20UW%20Figures");
            Link_08	=	ResolveLink("Market wordings database","Information","https://www.lloydswordings.com/lma/auth/login.do?req_url=https://www.lloydswordings.com/lma/app/start.do");
            Link_09	=	ResolveLink("Workflow Dashboard","Workflow","http://apps.talbotuw.com/sites/workflow/pages/underwritingworkflow.aspx");
            Link_10	=	ResolveLink("Crystal","Compliance Tools","http://www.lloyds.com/The-Market/Tools-and-Resources/Tools-E-Services/Crystal");
            Link_11	=	ResolveLink("XE Live Exchange Rates","Information","www.xe.com");
            Link_12	=	ResolveLink("Weekly Underwriter Pack","Information","http://intranet.validusholdings.com/Underwriting/Talbot/SitePages/UWPack.aspx");
            Link_13	=	ResolveLink("Trade Winds","Information","http://www.tradewindsnews.com/");
            Link_14	=	ResolveLink("MEL Pricing Models","Pricing Models",@"G:\Underwriting\Liability\New Liabs Shared Folder\Rating\2013");
            Link_15	=	ResolveLink("Rig Zone","Information","http://www.rigzone.com/");
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
        private User User10;
        private User User11;
        private User User12;
        private User User13;
        private User User14;
        private User User15;
        private User User16;
        private User User17;
        // ReSharper restore InconsistentNaming
        public override void SetUpUser()
        {
            User01 = ResolveUser("LON", "Ian Peterson", "petersi", "IRP");
            User02 = ResolveUser("LON", "Holly Kadwill", "KadwillH", "");
            User03 = ResolveUser("LON", "Alex MacLennan", "MacLenna", "AWM");
            User04 = ResolveUser("LON", "Jeannie Schreiner", "SchreiJ", "");
            User05 = ResolveUser("NYC", "Ella Stoop", "stoope", "ERS");
            User06 = ResolveUser("NYC", "Michelle Gatti", "GattiMi", "");
            User07 = ResolveUser("SNG", "John Ewington", "EwingJ", "JDE");
            User08 = ResolveUser("SNG", "Simon Wilmot-Smith", "wilmots", "SWS");
            User09 = ResolveUser("SNG", "Janita Leu", "LeuJa", "JTL");
            User10 = ResolveUser("SNG", "David Morris", "MorrisD", "DM ");
            User11 = ResolveUser("SNG", "Li Lin Kea", "keall", "");
            User12 = ResolveUser("SNG", "Shiling Su", "SuS", "");
            User13 = ResolveUser("DUB", "James Garratt", "GarrattJ", "JCG");
            User14 = ResolveUser("DUB", "Samuel Raj", "rajsamu", "SQR");
            User15 = ResolveUser("DUB", "Raghd Coussa", "CoussaR", "");
            User16 = ResolveUser("MIA", "Luis Sonville", "Luis.Sonville", "LAS");
            User17 = ResolveUser("MIA", "Diego I. San Martin", "SanmarD", "");
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

            QuoteTemplate01 = _consoleRepository.Query<QuoteTemplate>(qtmpl => qtmpl.Name == "Marine&Energy").SingleOrDefault();
            QuoteTemplate01 = QuoteTemplate01 ?? new QuoteTemplate { Name = "Marine&Energy", RdlPath = "/Underwriting/Console/QuoteSheet_ME" };

            SubmissionType01 = _consoleRepository.Query<SubmissionType>(styp => styp.Id == "ME").SingleOrDefault();
            SubmissionType01 = SubmissionType01 ?? new SubmissionType { Id = "ME", Title = "Marine&EnergyLiabilities_Submission" };

            Team01 = _consoleRepository.Query<Team>(team => team.Title == "Marine&Energy").SingleOrDefault();
            if (Team01 == null)
            {
                Team01 = new Team
                {
                    Title = "Marine&Energy",
                    QuoteExpiryDaysDefault = 30,
                    RelatedCOBs = new List<COB> { COB_CL, COB_CR},
                    Links = new List<Link> { Link_01, Link_02, Link_03, Link_04, Link_05, Link_06, Link_07, Link_08, Link_09, Link_10, Link_11, Link_12, Link_13, Link_14, Link_15 },
                    RelatedQuoteTemplates = new List<QuoteTemplate> { QuoteTemplate01 },
                    Memberships = new Collection<TeamMembership> { new TeamMembership { StartDate = DateTime.Now, User = User01 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User02 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User03 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User04 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User05 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User06 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User07 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User08 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User09 }, 
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User10 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User11 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User12 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User13 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User14 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User15 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User16 },
                                                                   new TeamMembership { StartDate = DateTime.Now, User = User17 },
                    },
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
