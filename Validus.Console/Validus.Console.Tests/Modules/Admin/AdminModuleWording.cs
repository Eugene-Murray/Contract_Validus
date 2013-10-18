extern alias globalVM;

using System;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Console.Data;
using Validus.Console.Tests.Helpers;
using Validus.Core.LogHandling;
using globalVM::Validus.Models;
using Validus.Core.HttpContext;
using Moq;
using System.Collections.Generic;
using System.Linq;


namespace Validus.Console.Tests.Modules.Admin
{
    [TestClass]
    public class AdminModuleWording
    {
        static Mock<ICurrentHttpContext> _httpContext;
        static int _totalMarketWording = 0;
        static int _totalTermsNConditionWording = 0;
        static int _totalSubjectToClauseWording = 0;

        [ClassInitialize]
        public static void Setup(TestContext t)
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                _totalMarketWording = rep.Query<MarketWording>(mw => true).Count();
                _totalTermsNConditionWording = rep.Query<TermsNConditionWording>(mw => true).Count();
                _totalSubjectToClauseWording = rep.Query<SubjectToClauseWording>(mw => true).Count();
                var lon = rep.Query<Office>(off => off.Id == "LON").First();
                var team = new Team
                    {
                        Title = "TestTeam",
                        QuoteExpiryDaysDefault = 30,
                        CreatedBy = "InitialSetup",
                        CreatedOn = DateTime.Now,
                        ModifiedBy = "InitialSetup",
                        ModifiedOn = DateTime.Now,
                        SubmissionTypeId = null,
                        DefaultPolicyType = "MARINE",
                        RelatedOffices = new List<Office> { lon },
                        TeamOfficeSettings =
                            new List<TeamOfficeSetting>
                                {
                                    new TeamOfficeSetting
                                        {
                                            Office = lon,
                                            MarketWordingSettings = new List<MarketWordingSetting>(),
                                            TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>(),
                                            SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>()
                                        }
                                }
                    };
                rep.Add(team);
                rep.SaveChanges();

            }
            _httpContext = new Mock<ICurrentHttpContext>();
            const string user = @"talbotdev\MurrayE";

            _httpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            _httpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());
        }

        [TestMethod]
        public void CheckSeedData()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalMarketWording = rep.Query<MarketWording>(mw => true).Count();
                Assert.AreEqual(_totalMarketWording, tempTotalMarketWording);

                var temptermsNConditionWording = rep.Query<TermsNConditionWording>(mw => true).Count();
                Assert.AreEqual(_totalTermsNConditionWording, temptermsNConditionWording);

                var tempsubjectToClauseWording = rep.Query<SubjectToClauseWording>(mw => true).Count();
                Assert.AreEqual(_totalSubjectToClauseWording, tempsubjectToClauseWording);


            }
        }

        #region Market Wording

        [TestMethod]
        public void AddMarketWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateMarketWording(new MarketWording
                    {
                        WordingRefNumber = "MTEST01",
                        Title = "MTEST01"
                    });

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalMarketWording = rep.Query<MarketWording>(mw => true).Count();
                Assert.AreEqual(_totalMarketWording + 1, tempTotalMarketWording);
                _totalMarketWording = tempTotalMarketWording;
            }
        }

        [TestMethod]
        public void EditMarketWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateMarketWording(new MarketWording
                {
                    WordingRefNumber = "MTEST02",
                    Title = "MTEST02"
                });

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempMarketWording = rep.Query<MarketWording>(mw => mw.Title == "MTEST02").First();
                tempMarketWording.Title = "MTEST02_Changed";

                adminModuleManager.EditMarketWording(tempMarketWording);

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalMarketWording = rep.Query<MarketWording>(mw => true).Count();
                var tempMarketWording = rep.Query<MarketWording>(mw => mw.Title == "MTEST02_Changed").First();
                var tempOldMarketWording = rep.Query<MarketWording>(mw => mw.Title == "MTEST02").FirstOrDefault();
                Assert.AreEqual(_totalMarketWording + 1, tempTotalMarketWording);
                Assert.IsNotNull(tempMarketWording);
                Assert.IsNull(tempOldMarketWording);
                _totalMarketWording = tempTotalMarketWording;
            }
        }

        [TestMethod]
        public void DeleteMarketWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateMarketWording(new MarketWording
                {
                    WordingRefNumber = "MTEST03",
                    Title = "MTEST03"
                });
            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempMarketWording = rep.Query<MarketWording>(mw => mw.Title == "MTEST03").First();
                adminModuleManager.DeleteMarketWording(tempMarketWording);
            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalMarketWording = rep.Query<MarketWording>(mw => true).Count();
                var tempMarketWording = rep.Query<MarketWording>(mw => mw.Title == "MTEST03").FirstOrDefault();
                Assert.AreEqual(_totalMarketWording, tempTotalMarketWording);
                Assert.IsNull(tempMarketWording);
            }
        }

        [TestMethod]
        public void SaveMarketWordingsForTeamOffice()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateMarketWording(new MarketWording
                {
                    WordingRefNumber = "MTEST04",
                    Title = "MTEST04"
                });
            }

            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempMarketWording = rep.Query<MarketWording>(mw => mw.Title == "MTEST04").First();
                var marketWordingSettingDto = new MarketWordingSettingDto
                    {
                        Id = tempMarketWording.Id,
                        DisplayOrder = 1,
                        WordingRefNumber = "MTEST04",
                        Title = "MTEST04"
                    };


                var teamMarketWordingsDto = new TeamMarketWordingsDto
                    {
                        TeamId = rep.Query<Team>(t => t.Title == "TestTeam").First().Id,
                        OfficeId = "LON",
                        MarketWordingSettingDtoList = new List<MarketWordingSettingDto> { marketWordingSettingDto }
                    };
                adminModuleManager.SaveMarketWordingsForTeamOffice(teamMarketWordingsDto);
            }

            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var team = rep.Query<Team>(t => t.Title == "TestTeam",
                                t => t.TeamOfficeSettings.Select(tos => tos.MarketWordingSettings.Select(mw => mw.MarketWording))).First();
                Assert.AreEqual("MTEST04",
                                team.TeamOfficeSettings.First().MarketWordingSettings.First().MarketWording.Title);
                _totalMarketWording = _totalMarketWording + 1;
            }


        }

        #endregion

        #region Terms and Condition Wording

        [TestMethod]
        public void AddTermsNConditionWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateTermsNConditionWording(new TermsNConditionWording
                {
                    WordingRefNumber = "TTEST01",
                    Title = "TTEST01"
                });

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => true).Count();
                Assert.AreEqual(_totalTermsNConditionWording + 1, tempTotalTermsNConditionWording);
                _totalTermsNConditionWording = tempTotalTermsNConditionWording;
            }
        }

        [TestMethod]
        public void EditTermsNConditionWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateTermsNConditionWording(new TermsNConditionWording
                {
                    WordingRefNumber = "TTEST02",
                    Title = "TTEST02"
                });

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => tnc.Title == "TTEST02").First();
                tempTermsNConditionWording.Title = "TTEST02_Changed";

                adminModuleManager.EditTermsNConditionWording(tempTermsNConditionWording);

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => true).Count();
                var tempTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => tnc.Title == "TTEST02_Changed").First();
                var tempOldTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => tnc.Title == "TTEST02").FirstOrDefault();
                Assert.AreEqual(_totalTermsNConditionWording + 1, tempTotalTermsNConditionWording);
                Assert.IsNotNull(tempTermsNConditionWording);
                Assert.IsNull(tempOldTermsNConditionWording);
                _totalTermsNConditionWording = tempTotalTermsNConditionWording;
            }
        }

        [TestMethod]
        public void DeleteTermsNConditionWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateTermsNConditionWording(new TermsNConditionWording
                {
                    WordingRefNumber = "TTEST03",
                    Title = "TTEST03"
                });
            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => tnc.Title == "TTEST03").First();
                adminModuleManager.DeleteTermsNConditionWording(tempTermsNConditionWording);
            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => true).Count();
                var tempTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => tnc.Title == "TTEST03").FirstOrDefault();
                Assert.AreEqual(_totalTermsNConditionWording, tempTotalTermsNConditionWording);
                Assert.IsNull(tempTermsNConditionWording);
            }
        }

        [TestMethod]
        public void SaveTermsNConditionWordingsForTeamOffice()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateTermsNConditionWording(new TermsNConditionWording
                {
                    WordingRefNumber = "TTEST04",
                    Title = "TTEST04"
                });
            }

            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempTermsNConditionWording = rep.Query<TermsNConditionWording>(tnc => tnc.Title == "TTEST04").First();
                var termsNConditionWordingSettingDto = new TermsNConditionWordingSettingDto
                {
                    Id = tempTermsNConditionWording.Id,
                    DisplayOrder = 1,
                    WordingRefNumber = "TTEST04",
                    Title = "TTEST04"
                };


                var teamTermsNConditionWordingsDto = new TeamTermsNConditionWordingsDto
                {
                    TeamId = rep.Query<Team>(t => t.Title == "TestTeam").First().Id,
                    OfficeId = "LON",
                    TermsNConditionWordingSettingDtoList = new List<TermsNConditionWordingSettingDto> { termsNConditionWordingSettingDto }
                };
                adminModuleManager.SaveTermsNConditionWordingsForTeamOffice(teamTermsNConditionWordingsDto);
            }

            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var team = rep.Query<Team>(t => t.Title == "TestTeam",
                                t => t.TeamOfficeSettings.Select(tos => tos.TermsNConditionWordingSettings.Select(tnc => tnc.TermsNConditionWording))).First();
                Assert.AreEqual("TTEST04",
                                team.TeamOfficeSettings.First().TermsNConditionWordingSettings.First().TermsNConditionWording.Title);
                _totalTermsNConditionWording = _totalTermsNConditionWording + 1;
            }


        }

        #endregion

        #region Subject To Clause Wording

        [TestMethod]
        public void AddSubjectToClauseWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateSubjectToClauseWording(new SubjectToClauseWording
                {
                    WordingRefNumber = "STEST01",
                    Title = "STEST01"
                });

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => true).Count();
                Assert.AreEqual(_totalSubjectToClauseWording + 1, tempTotalSubjectToClauseWording);
                _totalSubjectToClauseWording = tempTotalSubjectToClauseWording;
            }
        }

        [TestMethod]
        public void EditSubjectToClauseWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateSubjectToClauseWording(new SubjectToClauseWording
                {
                    WordingRefNumber = "STEST02",
                    Title = "STEST02"
                });

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => sw.Title == "STEST02").First();
                tempSubjectToClauseWording.Title = "STEST02_Changed";

                adminModuleManager.EditSubjectToClauseWording(tempSubjectToClauseWording);

            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => true).Count();
                var tempSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => sw.Title == "STEST02_Changed").First();
                var tempOldSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => sw.Title == "STEST02").FirstOrDefault();
                Assert.AreEqual(_totalSubjectToClauseWording + 1, tempTotalSubjectToClauseWording);
                Assert.IsNotNull(tempSubjectToClauseWording);
                Assert.IsNull(tempOldSubjectToClauseWording);
                _totalSubjectToClauseWording = tempTotalSubjectToClauseWording;
            }
        }

        [TestMethod]
        public void DeleteSubjectToClauseWording()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateSubjectToClauseWording(new SubjectToClauseWording
                {
                    WordingRefNumber = "STEST03",
                    Title = "STEST03"
                });
            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => sw.Title == "STEST03").First();
                adminModuleManager.DeleteSubjectToClauseWording(tempSubjectToClauseWording);
            }
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var tempTotalSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => true).Count();
                var tempSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => sw.Title == "STEST03").FirstOrDefault();
                Assert.AreEqual(_totalSubjectToClauseWording, tempTotalSubjectToClauseWording);
                Assert.IsNull(tempSubjectToClauseWording);
            }
        }

        [TestMethod]
        public void SaveSubjectToClauseWordingsForTeamOffice()
        {
            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                adminModuleManager.CreateSubjectToClauseWording(new SubjectToClauseWording
                {
                    WordingRefNumber = "STEST04",
                    Title = "STEST04"
                });
            }

            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var adminModuleManager = new AdminModuleManager(rep, new LogHandler(), _httpContext.Object, new WebSiteModuleManager(rep, _httpContext.Object));
                var tempSubjectToClauseWording = rep.Query<SubjectToClauseWording>(sw => sw.Title == "STEST04").First();
                var subjectToClauseWordingSettingDto = new SubjectToClauseWordingSettingDto
                {
                    Id = tempSubjectToClauseWording.Id,
                    DisplayOrder = 1,
                    Title = "STEST04"
                };


                var teamSubjectToClauseWordingsDto = new TeamSubjectToClauseWordingsDto
                {
                    TeamId = rep.Query<Team>(t => t.Title == "TestTeam").First().Id,
                    OfficeId = "LON",
                    SubjectToClauseWordingSettingDtoList = new List<SubjectToClauseWordingSettingDto> { subjectToClauseWordingSettingDto }
                };
                adminModuleManager.SaveSubjectToClauseWordingsForTeamOffice(teamSubjectToClauseWordingsDto);
            }

            using (IConsoleRepository rep = new ConsoleRepository())
            {
                var team = rep.Query<Team>(t => t.Title == "TestTeam",
                                t => t.TeamOfficeSettings.Select(tos => tos.SubjectToClauseWordingSettings.Select(sw => sw.SubjectToClauseWording))).First();
                Assert.AreEqual("STEST04",
                                team.TeamOfficeSettings.First().SubjectToClauseWordingSettings.First().SubjectToClauseWording.Title);
                _totalSubjectToClauseWording = _totalSubjectToClauseWording + 1;
            }


        }

        #endregion


    }
}
