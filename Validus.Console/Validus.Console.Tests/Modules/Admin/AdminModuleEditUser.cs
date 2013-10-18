extern alias globalVM;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.BusinessLogic;
using Validus.Console.Data;
using Validus.Console.DTO;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using globalVM::Validus.Models;

namespace Validus.Console.Tests.Modules.Admin
{
    [TestClass]
    public class AdminModuleEditUser
    {
        static Mock<ICurrentHttpContext> context;
        static Int32 newUserId;
        static Int32 TeamAId;
        static Int32 TeamBId;

        [ClassInitialize]
        public static void Setup(TestContext t)
        {            
            using (IConsoleRepository _rep = new ConsoleRepository())
            {
                Team teamA = new Team() { Title = "Team A", DefaultPolicyType = "MARINE", QuoteExpiryDaysDefault = 30 };
                _rep.Add<Team>(teamA);
                Team teamB = new Team() { Title = "Team B", DefaultPolicyType = "MARINE", QuoteExpiryDaysDefault = 30 };
                _rep.Add<Team>(teamB);
                
                COB aa = new COB() { Id = "AA", Narrative = "AA" };
                COB bb = new COB() { Id = "BB", Narrative = "BB" };
                COB xx = new COB() { Id = "XX", Narrative = "XX" };
                COB yy = new COB() { Id = "YY", Narrative = "YY" };
                COB zz = new COB() { Id = "ZZ", Narrative = "ZZ" };
                _rep.Add<COB>(aa);
                _rep.Add<COB>(bb);
                _rep.Add<COB>(xx);
                _rep.Add<COB>(yy);
                _rep.Add<COB>(zz);

                teamA.RelatedCOBs = new List<COB>();
                teamA.RelatedCOBs.Add(aa);
                teamA.RelatedCOBs.Add(bb);
                teamA.RelatedCOBs.Add(xx);

                teamB.RelatedCOBs = new List<COB>();
                teamB.RelatedCOBs.Add(xx);
                teamB.RelatedCOBs.Add(yy);
                teamB.RelatedCOBs.Add(zz);

                User user1 = new User() { DomainLogon = @"talbot\user1", UnderwriterCode = "AED" };
                _rep.Add<User>(user1);

                _rep.SaveChanges();
                newUserId = user1.Id;
                TeamAId = teamA.Id;
                TeamBId = teamB.Id;
            }
            
            context = new Mock<ICurrentHttpContext>();
            var user = @"talbotdev\MurrayE";
            
            context.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(user), null));
            context.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());            
        }

        [TestMethod]
        public void Edit_User_Add_Membership_User_Gets_Filters()
        {
            // Assign
            ConsoleRepository c1 = new ConsoleRepository();
            AdminModuleManager a1 = new AdminModuleManager(c1, new LogHandler(), context.Object, new WebSiteModuleManager(c1, context.Object));

            var dto = a1.GetUser(newUserId);
            dto.TeamMemberships = new List<TeamMembershipDto>();
            dto.TeamMemberships.Add(new DTO.TeamMembershipDto()
            {
                Team = new TeamDto() { Id = TeamAId },
                UserId = newUserId,
                PrimaryTeamMembership = true
            });
            
            // Act
            ConsoleRepository c = new ConsoleRepository();
            AdminModuleManager a = new AdminModuleManager(c, new LogHandler(), context.Object, new WebSiteModuleManager(c, context.Object));
            var result = a.EditUser(dto);

            ConsoleRepository c2 = new ConsoleRepository();
            AdminModuleManager a2 = new AdminModuleManager(c2, new LogHandler(), context.Object, new WebSiteModuleManager(c2, context.Object));
            var dtoafter = a2.GetUser(newUserId);

            // Assert
            Assert.AreEqual(dtoafter.FilterCOBs.Count, 3);
        }        

        [TestMethod]
        public void Edit_User_Add_2nd_Membership_User_Gets_Filters()
        {
            // Assign
            ConsoleRepository c1 = new ConsoleRepository();
            AdminModuleManager a1 = new AdminModuleManager(c1, new LogHandler(), context.Object, new WebSiteModuleManager(c1, context.Object));

            var dto = a1.GetUser(newUserId);
            dto.TeamMemberships = new List<TeamMembershipDto>();
            dto.TeamMemberships.Add(new DTO.TeamMembershipDto()
            {
                Team = new TeamDto() { Id = TeamBId },
                UserId = newUserId,
                PrimaryTeamMembership = true
            });
            
            // Act
            ConsoleRepository c = new ConsoleRepository();
            AdminModuleManager a = new AdminModuleManager(c, new LogHandler(), context.Object, new WebSiteModuleManager(c, context.Object));
            var result = a.EditUser(dto);

            ConsoleRepository c2 = new ConsoleRepository();
            AdminModuleManager a2 = new AdminModuleManager(c2, new LogHandler(), context.Object, new WebSiteModuleManager(c2, context.Object));
            var dtoafter = a2.GetUser(newUserId);

            // Assert
            Assert.AreEqual(dtoafter.FilterCOBs.Count, 5);
        }

        //[TestMethod]
        //public void Edit_User_Remove_1st_Membership_User_Keep_2nds_Filters()
        //{
        //    // Assign
        //    ConsoleRepository c1 = new ConsoleRepository();
        //    AdminModuleManager a1 = new AdminModuleManager(c1, new LogHandler(), context.Object);

        //    var dto = a1.GetUser(newUserId);
        //    //dto.TeamMemberships.
        //    dto.AllTeamMemberships.Add(new DTO.TeamMembershipDto()
        //    {
        //        Team = new TeamDto() { Id = TeamAId },
        //        UserId = newUserId,
        //        PrimaryTeamMembership = false
        //    });

        //    // Act
        //    ConsoleRepository c = new ConsoleRepository();
        //    AdminModuleManager a = new AdminModuleManager(c, new LogHandler(), context.Object);
        //    var result = a.EditUser(dto);

        //    ConsoleRepository c2 = new ConsoleRepository();
        //    AdminModuleManager a2 = new AdminModuleManager(c2, new LogHandler(), context.Object);
        //    var dtoafter = a2.GetUser(newUserId);

        //    // Assert
        //    Assert.AreEqual(dtoafter.FilterCOBs.Count, 3);
        //}
    }
}
