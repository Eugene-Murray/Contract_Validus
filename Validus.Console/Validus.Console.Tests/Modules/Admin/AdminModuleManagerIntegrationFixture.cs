extern alias globalVM;
using System;
using System.Collections.ObjectModel;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Moq;
using Validus.Console.Data;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using globalVM::Validus.Models;
using System.Collections.Generic;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using System.Linq;


namespace Validus.Console.Tests.Modules.Admin
{
    [TestClass]
    public class AdminModuleManagerIntegrationFixture : IntegrationFixtureBase
    {
        public static IUnityContainer _container;
        public static IAdminModuleManager _adminModuleManager;
        public static TeamDto _actualTestTeam;
        public static UserDto _editUserDto;
        public static TeamDto _testTeam;
        public static LogHandler _logHandler;
        public static Mock<CurrentHttpContext> _mockCurrentHttpContext;



        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _container = new UnityContainer();
            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            using (IConsoleRepository repository = _container.Resolve<ConsoleRepository>())
            {
                var team = repository.Query<Team>(t=>t.RelatedOffices).FirstOrDefault(t => t.Title == "New Test Team");
                if (team != null)
                {
                    repository.Attach<Team>(team);
                    repository.Delete<Team>(team);
                    repository.SaveChanges();
                }
            }

            using (IConsoleRepository repository = _container.Resolve<ConsoleRepository>())
            {
                var team = repository.Query<Team>(t => t.RelatedOffices).FirstOrDefault(t => t.Title == "Test Team");
                if (team != null)
                {
                    repository.Attach<Team>(team);
                    repository.Delete<Team>(team);
                    repository.SaveChanges();
                }
            }

            _logHandler = new LogHandler();
            var consoleRepository = new ConsoleRepository();
            _mockCurrentHttpContext = new Mock<CurrentHttpContext>();
            _mockCurrentHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(@"talbotdev\MurrayE"), null));
            _mockCurrentHttpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());

            var adminModuleManager = new AdminModuleManager(consoleRepository, _logHandler, _mockCurrentHttpContext.Object, new WebSiteModuleManager(consoleRepository, _mockCurrentHttpContext.Object));
            _editUserDto = adminModuleManager.GetUser(4);

            var consoleRepository2 = new ConsoleRepository();
            var adminModuleManager2 = new AdminModuleManager(consoleRepository2, _logHandler, _mockCurrentHttpContext.Object, new WebSiteModuleManager(consoleRepository2, _mockCurrentHttpContext.Object));
            var teamDto = new TeamDto { Title = "Test Team", QuoteExpiryDaysDefault = 2, DefaultPolicyType = "MARINE" };
            _testTeam = adminModuleManager2.CreateTeam(teamDto);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_actualTestTeam != null)
            {
                var consoleRepository1 = new ConsoleRepository();
                var adminModuleManager1 = new AdminModuleManager(consoleRepository1, _logHandler, _mockCurrentHttpContext.Object, new WebSiteModuleManager(consoleRepository1, _mockCurrentHttpContext.Object));
                var teamToDelete = adminModuleManager1.GetTeam(_actualTestTeam.Id);

                if (teamToDelete != null)
                {
                    var consoleRepository2 = new ConsoleRepository();
                    var adminModuleManager2 = new AdminModuleManager(consoleRepository2, _logHandler, _mockCurrentHttpContext.Object, new WebSiteModuleManager(consoleRepository2, _mockCurrentHttpContext.Object));
                    adminModuleManager2.DeleteTeam(teamToDelete);
                }
            }

            if (_testTeam != null)
            {
                var consoleRepository1 = new ConsoleRepository();
                var adminModuleManager3 = new AdminModuleManager(consoleRepository1, _logHandler, _mockCurrentHttpContext.Object, new WebSiteModuleManager(consoleRepository1, _mockCurrentHttpContext.Object));
                var teamToDelete = adminModuleManager3.GetTeam(_testTeam.Id);

                if (teamToDelete != null)
                {
                    var consoleRepository2 = new ConsoleRepository();
                    var adminModuleManager4 = new AdminModuleManager(consoleRepository2, _logHandler, _mockCurrentHttpContext.Object, new WebSiteModuleManager(consoleRepository2, _mockCurrentHttpContext.Object));
                    adminModuleManager4.DeleteTeam(teamToDelete);
                }
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            var logHandler = new LogHandler();
            var consoleRepository = new ConsoleRepository();
            var mockHttpContext = new Mock<CurrentHttpContext>();
            mockHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(@"talbotdev\MurrayE"), null));
            mockHttpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());

            _adminModuleManager = new AdminModuleManager(consoleRepository, logHandler, mockHttpContext.Object, new WebSiteModuleManager(consoleRepository, _mockCurrentHttpContext.Object));
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [Ignore]
        [TestMethod]
        public void GetTeamsAndLinks_Success()
        {
            // Assign
            var repository = _container.Resolve<IConsoleRepository>();

            // Act
            var actualExpectedTeam = repository.Query<Team>(t => t.Links);
            // Assert
            Assert.IsNotNull(actualExpectedTeam);
        }

        [TestMethod]
        public void SaveLinksForTeam_LinksSame_Success()
        {
            // Assign
            var teamLinksIdList = new List<int> { 7, 8 };
            var teamLinksDto = new TeamLinksDto { TeamId = _testTeam.Id, TeamLinksIdList = teamLinksIdList };

            var expectedResultMessage = "Save Links - Team does not Exist";

            // Act
            var actualResultMessage = _adminModuleManager.SaveLinksForTeam(teamLinksDto);

            // Assert
            Assert.AreNotEqual(expectedResultMessage, actualResultMessage);
        }

        [TestMethod]
        public void GetLinksForTeam_Success()
        {
            // Assign
            int teamId = 1;
            int expectedLinkCount = 3;

            // Act
            var actualLinkCount = _adminModuleManager.GetLinksForTeam(teamId).Count;

            // Assert
            Assert.IsTrue(actualLinkCount > 0);
        }

        [TestMethod]
        public void SearchForUserByName_NoUserFound_Success()
        {
            // Assign
            string expectedUserName = @"talbotdev\InvisibleUser";
            int expectedUserCount = 0;

            // Act
            var actualUserCount = _adminModuleManager.SearchForUserByName(expectedUserName).Count;

            // Assert
            Assert.AreEqual(expectedUserCount, actualUserCount);

        }

        // TODO: not sure what the setup of this test fails when the rest work
        [TestMethod]
        public void GetTeam_ReturnResult_Success()
        {
            // Assign
            var teamId = 1;
            var expectedTeam = new Team { Title = "Developers" };

            // Act
            var actualTeam = _adminModuleManager.GetTeam(teamId);

            // Assert
            Assert.AreEqual(expectedTeam.Title, actualTeam.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetTeam_Exception()
        {
            // Assign
            int? teamId = null;

            // Act
            var actualTestTeam = _adminModuleManager.GetTeam(teamId);

            // Assert
        }

        [TestMethod]
        public void GetUsersInTeam_Success()
        {
            // Assign
            var teamId = 1;
            var expectedUsersInTeamCount = 3;

            // Act
            var actualUsersInTeamCount = _adminModuleManager.GetUsersInTeam(teamId).Count;

            // Assert
            Assert.IsTrue(expectedUsersInTeamCount <= actualUsersInTeamCount);
        }

        [TestMethod]
        public void CreateSimpleTeam_Success()
        {
            // Assign
            var expectedTeam = new TeamDto { Title = "New Test Team", QuoteExpiryDaysDefault = 1 };

            // Act
            _actualTestTeam = _adminModuleManager.CreateTeam(expectedTeam);

            // Assert
            Assert.IsNotNull(_actualTestTeam);

        }

        [TestMethod]
        public void EditTeam_Success()
        {
            // Assign
            var logHandler = new LogHandler();
            var consoleRepository = new ConsoleRepository();
            var mockHttpContext = new Mock<ICurrentHttpContext>();
            mockHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(@"talbotdev\MurrayE"), null));

            var adminModuleManager = new AdminModuleManager(consoleRepository, logHandler, mockHttpContext.Object, new WebSiteModuleManager(consoleRepository, mockHttpContext.Object));

            // Act
            _testTeam.Title = "Edit Team - new name...";
            var updatedTeam = adminModuleManager.EditTeam(_testTeam);

            // Assert
            Assert.AreEqual("Edit Team - new name...", updatedTeam.Title);
        }

        [TestMethod]
        public void GetUsersById_Success()
        {
            // Assign
            var userId = 1;

            // Act
            var userResult = _adminModuleManager.GetUser(userId);

            // Assert
            Assert.IsNotNull(userResult);
        }

        [TestMethod]
        public void SearchForUserByName_Success()
        {
            // Assign
            string userName = @"talbotdev\MurrayE";

            // Act
            var actualUsers = _adminModuleManager.SearchForUserByName(userName);

            // Assert
            Assert.AreEqual(userName, actualUsers[0].DomainLogon);
        }

        [TestMethod]
        public void GetRequiredDataCreateUser_Success()
        {
            // Assign

            // Act
            var dto = _adminModuleManager.GetRequiredDataCreateUser();

            // Assert
            Assert.IsNotNull(dto);
        }

        [TestMethod]
        public void AddTeamFilters_CreateOrEditTeam_Success()
        {
            // Assign
            var user = new User
            {
                FilterCOBs = new Collection<COB> { new COB { Id = "3", Narrative = "Cob 3" } },
                FilterOffices = new Collection<Office> { new Office { Id = "3", Title = "Office 3" }, new Office { Id = "4", Title = "Office 4" } }
            };
            var team = new Team
                {
                    Id = 1,
                    RelatedCOBs = new Collection<COB> { new COB { Id = "1", Narrative = "Cob 1" }, new COB { Id = "2", Narrative = "Cob 2" } },
                    RelatedOffices = new Collection<Office> { new Office { Id = "1", Title = "Office 1" }, new Office { Id = "2", Title = "Office 2" } }
                };
            var teamIds = new List<int> { 1 };

            var expectedCOBCount = 3;
            var expectedOfficeCount = 4;

            // Act
            _adminModuleManager.AddTeamFilters(teamIds, user, team);

            // Assert
            Assert.AreEqual(expectedCOBCount, user.FilterCOBs.Count());
            Assert.AreEqual(expectedOfficeCount, user.FilterOffices.Count());
        }

        [TestMethod]
        public void AddTeamFilters_CreateOrEditUser_Success()
        {
            // Assign
            var teamIds = new List<int> { 1, 2 };
            var user = new User
            {
                FilterCOBs = new Collection<COB> { new COB { Id = "3", Narrative = "Cob 3" } },
                FilterOffices = new Collection<Office> { new Office { Id = "3", Title = "Office 3" }, new Office { Id = "4", Title = "Office 4" } }
            };

            var expectedCOBCount = 5;
            var expectedOfficeCount = 2+2;//2 offices are set in the seed data

            // Act
            _adminModuleManager.AddTeamFilters(teamIds, user, null);

            // Assert
            Assert.AreEqual(expectedCOBCount, user.FilterCOBs.Count());
            Assert.AreEqual(expectedOfficeCount, user.FilterOffices.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void EditUser_Exception()
        {
            // Assign
            var userDto = new UserDto { Id = 0 };

            // Act
            var actualResult = _adminModuleManager.EditUser(userDto);

            // Assert
        }

        [TestMethod]
        public void EditUser_Success()
        {
            // Assign
            var expectedResult = "User successfully updated";
 
            // Act
            var actualResult = _adminModuleManager.EditUser(_editUserDto);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void EditUser_SetPrimaryTeamMembership_Success()
        {
            // Assign
            var expectedResult = "User successfully updated";
            var teamMembershipDto = _editUserDto.TeamMemberships.FirstOrDefault();
            if (teamMembershipDto != null)
                teamMembershipDto.PrimaryTeamMembership = (teamMembershipDto.PrimaryTeamMembership == true) ? false : true;

            // Act
            var actualResult = _adminModuleManager.EditUser(_editUserDto);

            using (IConsoleRepository repository = _container.Resolve<ConsoleRepository>())
            {
                var actualTeamMembership = repository.Query<TeamMembership>(tm => tm.UserId == _editUserDto.Id).FirstOrDefault();
                
                // Assert
                if (actualTeamMembership != null && teamMembershipDto != null)
                    Assert.AreEqual(teamMembershipDto.PrimaryTeamMembership, actualTeamMembership.PrimaryTeamMembership);
            }


            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void EditUser_SetPrimaryTeamMembership_True_Success()
        {
            // Assign
            var expectedTeamMembership = true;
            var expectedResult = "User successfully updated";
            var teamMembershipDto = _editUserDto.TeamMemberships.FirstOrDefault();
            if (teamMembershipDto != null)
                teamMembershipDto.PrimaryTeamMembership = expectedTeamMembership;
            _editUserDto.UnderwriterId = "JAC";

            // Act
            var actualResult = _adminModuleManager.EditUser(_editUserDto);

            using (IConsoleRepository repository = _container.Resolve<ConsoleRepository>())
            {
                var actualTeamMembership = repository.Query<TeamMembership>(tm => tm.UserId == _editUserDto.Id).FirstOrDefault();

                // Assert
                Assert.AreEqual(expectedTeamMembership, actualTeamMembership.PrimaryTeamMembership);
            }


            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void EditUser_SetPrimaryTeamMembership_False_Success()
        {
            // Assign
            var expectedTeamMembership = false;
            var expectedResult = "User successfully updated";
            var teamMembershipDto = _editUserDto.TeamMemberships.FirstOrDefault(tm => tm.PrimaryTeamMembership == true);
            if (teamMembershipDto != null)
                teamMembershipDto.PrimaryTeamMembership = expectedTeamMembership;
            _editUserDto.UnderwriterId = "JAC";

            // Act
            var actualResult = _adminModuleManager.EditUser(_editUserDto);

            using (IConsoleRepository repository = _container.Resolve<ConsoleRepository>())
            {
                var actualTeamMembership = repository.Query<TeamMembership>(tm => tm.UserId == _editUserDto.Id).FirstOrDefault();

                // Assert
                Assert.AreEqual(expectedTeamMembership, actualTeamMembership.PrimaryTeamMembership);
            }


            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetSelectedUserByName_Success()
        {
            // Assign
            var userName = @"talbotdev\MurrayE";
            var expectedUnderwriterId = "AED";

            // Act
            var actualUserDto = _adminModuleManager.GetSelectedUserByName(userName);

            // Assert
            Assert.IsNotNull(actualUserDto);
            Assert.AreEqual(expectedUnderwriterId, actualUserDto.UnderwriterId);
        }

    }
}
