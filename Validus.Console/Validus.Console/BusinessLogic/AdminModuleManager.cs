using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Xml;
using RazorEngine;
using Validus.Console.Data;
using Validus.Console.DTO;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public class AdminModuleManager : IAdminModuleManager
    {
        public readonly IConsoleRepository ConsoleRepository;
        public readonly ILogHandler LogHandler;
        public readonly ICurrentHttpContext CurrentHttpContext;
        public readonly IWebSiteModuleManager WebSiteModuleManager;

        public AdminModuleManager(IConsoleRepository consoleRepository, ILogHandler logHandler,
                                  ICurrentHttpContext currentHttpContext, IWebSiteModuleManager webSiteModuleManager)
        {
            this.LogHandler = logHandler;
            this.ConsoleRepository = consoleRepository;
            this.CurrentHttpContext = currentHttpContext;
            this.WebSiteModuleManager = webSiteModuleManager;
        }

        #region User Team Link

        public Team GetTeam(int? teamId)
        {
            if (teamId == null) // TODO: Not needed if parameter is not nullable ?
                throw new Exception("teamId is empty"); // TODO: Throw new ArgumentNullException(teamId)

            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<Team>().FirstOrDefault(t => t.Id == teamId.Value);
            }
        }

        public List<TeamDto> GetTeamsBasicData()
        {
            var teamList = new List<TeamDto>();
            using (ConsoleRepository)
            {
                var teams = ConsoleRepository.Query<Team>().ToList();

                teams.ForEach(t =>
                    {
                        teamList.Add(new TeamDto
                            {
                                Id = t.Id,
                                Title = t.Title,
                                DefaultDomicile = t.DefaultDomicile,
                                DefaultMOA = t.DefaultDomicile,
                                DefaultPolicyType = t.DefaultPolicyType
                            });
                    });

            }

            return teamList;
        }

        public List<TeamDto> GetTeamsFullData()
        {
            var teamList = new List<TeamDto>();
            using (ConsoleRepository)
            {
                var teams =
                    ConsoleRepository.Query<Team>(t => t.Memberships.Select(mu => mu.User), 
                                                    t => t.RelatedCOBs,
                                                    t => t.RelatedOffices, 
                                                    t => t.RelatedOffices, t => t.Links).ToList();

                teams.ForEach(team =>
                    {
                        var users = new List<BasicUserDto>();
                        var links = new List<LinkDto>();
                        var relatedCobs = new List<COBDto>();
                        var relatedOffices = new List<OfficeDto>();
                        var allUsers = new List<BasicUserDto>();
                        var allLinks = new List<LinkDto>();
                        var allRelatedCobs = new List<COBDto>();
                        var allRelatedOffices = new List<OfficeDto>();

                        if (team.Memberships != null)
                        {
                            foreach (var mem in team.Memberships.Where(m => m.IsCurrent).ToList())
                            {
                                users.Add(new BasicUserDto { Id = mem.User.Id, DomainLogon = mem.User.DomainLogon });
                            }
                        }

                        if (team.Links != null)
                        {
                            foreach (var link in team.Links.ToList())
                            {
                                links.Add(new LinkDto
                                    {
                                        Id = link.Id,
                                        Category = link.Category,
                                        Url = link.Url,
                                        Title = link.Title
                                    });
                            }
                        }

                        if (team.RelatedCOBs != null)
                        {
                            foreach (var cob in team.RelatedCOBs.ToList())
                            {
                                relatedCobs.Add(new COBDto { Id = cob.Id, Narrative = cob.Narrative });
                            }
                        }

                        if (team.RelatedOffices != null)
                        {
                            foreach (var office in team.RelatedOffices.ToList())
                            {
                                relatedOffices.Add(new OfficeDto { Id = office.Id, Title = office.Name });
                            }
                        }

                        // Get the List values not already added to the team
                        var currentUsers = team.Memberships.Select(m => m.User).ToList();
                        allUsers = ConsoleRepository.Query<User>().ToList().Except(currentUsers).Select(u =>
                            {
                                return new BasicUserDto { Id = u.Id, DomainLogon = u.DomainLogon };

                            }).ToList();

                        allLinks = ConsoleRepository.Query<Link>().ToList().Except(team.Links.ToList()).Select(l =>
                            {
                                return new LinkDto { Id = l.Id, Category = l.Category, Url = l.Url, Title = l.Title };

                            }).ToList();

                        allRelatedCobs =
                            ConsoleRepository.Query<COB>().ToList().Except(team.RelatedCOBs.ToList()).Select(c =>
                                {
                                    return new COBDto { Id = c.Id, Narrative = c.Narrative };

                                }).ToList();

                        allRelatedOffices =
                            ConsoleRepository.Query<Office>().ToList().Except(team.RelatedOffices.ToList()).Select(o =>
                                {
                                    return new OfficeDto { Id = o.Id, Title = o.Name };

                                }).ToList();

                        teamList.Add(new TeamDto
                            {
                                Id = team.Id,
                                Title = team.Title,
                                DefaultDomicile = team.DefaultDomicile,
                                DefaultMOA = team.DefaultMOA,
                                QuoteExpiryDaysDefault = team.QuoteExpiryDaysDefault,
                                DefaultPolicyType = team.DefaultPolicyType,
                                Users = users,
                                Links = links,
                                RelatedCOBs = relatedCobs,
                                RelatedOffices = relatedOffices,
                                AllLinks = allLinks,
                                AllRelatedCOBs = allRelatedCobs,
                                AllRelatedOffices = allRelatedOffices,
                                AllUsers = allUsers
                            });
                    });

            }
            return teamList;
        }

        public List<User> GetUsers()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<User>().ToList();
            }
        }

        public List<string> GetUsersInTeam(int? teamId)
        {
            using (ConsoleRepository)
            {
                var teamMemberships =
                    ConsoleRepository.Query<TeamMembership>().Where(tm => tm.TeamId == teamId && tm.IsCurrent);

                return teamMemberships.Select(tm => tm.User.DomainLogon).ToList();
            }
        }

        public TeamDto CreateTeam(TeamDto teamDto)
        {
            using (ConsoleRepository)
            {
                var existingTeam = ConsoleRepository.Query<Team>().FirstOrDefault(t => t.Title == teamDto.Title);

                if (existingTeam != null)
                    throw new ArgumentException("Team already exists, please use another Title.");

                var team = new Team
                    {
                        Title = teamDto.Title,
                        DefaultDomicile = teamDto.DefaultDomicile,
                        DefaultMOA = teamDto.DefaultMOA,
                        QuoteExpiryDaysDefault = teamDto.QuoteExpiryDaysDefault,
                        DefaultPolicyType = (string.IsNullOrEmpty(teamDto.DefaultPolicyType)) ? "MARINE" : teamDto.DefaultPolicyType
                    };

                if (teamDto.Links != null)
                {
                    var linkList = new List<Link>();
                    foreach (var newLink in teamDto.Links)
                    {
                        var link = ConsoleRepository.Query<Link>().FirstOrDefault(l => l.Id == newLink.Id);
                        linkList.Add(link);
                    }
                    team.Links = linkList;
                }

                if (teamDto.RelatedCOBs != null)
                {
                    var cobList = new List<COB>();
                    foreach (var newCOB in teamDto.RelatedCOBs)
                    {
                        var cob = ConsoleRepository.Query<COB>().FirstOrDefault(c => c.Id == newCOB.Id);
                        cobList.Add(cob);
                    }
                    team.RelatedCOBs = cobList;
                }

                if (teamDto.RelatedOffices != null)
                {
                    var officeList = new List<Office>();
                    foreach (var newOffice in teamDto.RelatedOffices)
                    {
                        var office = ConsoleRepository.Query<Office>().FirstOrDefault(c => c.Id == newOffice.Id);
                        officeList.Add(office);
                    }
                    team.RelatedOffices = officeList;
                }

                if (teamDto.Users != null)
                {
                    var teamMembershipList = new List<TeamMembership>();
                    var userList = new List<User>();
                    foreach (var newUser in teamDto.Users)
                    {
                        var user = ConsoleRepository.Query<User>().FirstOrDefault(u => u.Id == newUser.Id);
                        userList.Add(user);
                        teamMembershipList.Add(new TeamMembership
                            {
                                User = user,
                                Team = team,
                                StartDate = DateTime.Now,
                                EndDate = null
                            });
                    }
                    team.Memberships = teamMembershipList;

                    // Add Team FilterCOBs and FilterOffices to added users
                    foreach (var user in userList)
                    {
                        AddTeamFilters(teamMembershipList.Select(tm => tm.TeamId).Distinct(), user, team);
                    }
                }

                ConsoleRepository.Add<Team>(team);
                ConsoleRepository.SaveChanges();

                teamDto.Id = team.Id;
                return teamDto;
            }
        }

        public TeamDto EditTeam(TeamDto teamDto)
        {
            using (ConsoleRepository)
            {
                var team =
                    ConsoleRepository.Query<Team>(t => t.Memberships.Select(mu => mu.User.FilterCOBs),
                                                    t => t.Memberships.Select(mu => mu.User.FilterOffices), 
                                                    t => t.RelatedCOBs,
                                                    t => t.RelatedOffices, 
                                                    t => t.Links).FirstOrDefault(t => t.Id == teamDto.Id);

                if (team == null)
                    throw new Exception("Team to Edit does not exist"); // TODO: throw new NullReferenceException(team);

                ConsoleRepository.Attach<Team>(team);

                team.Title = teamDto.Title;
                team.DefaultDomicile = teamDto.DefaultDomicile;
                team.DefaultMOA = teamDto.DefaultMOA;
                team.QuoteExpiryDaysDefault = teamDto.QuoteExpiryDaysDefault;
                team.DefaultPolicyType = string.IsNullOrEmpty(teamDto.DefaultPolicyType)
                                             ? "MARINE"
                                             : teamDto.DefaultPolicyType;

                if (teamDto.Links != null)
                {
                    if (team.Links != null)
                        team.Links.Clear();

                    var linkList = new List<Link>();
                    foreach (var newLink in teamDto.Links)
                    {
                        var link = ConsoleRepository.Query<Link>().FirstOrDefault(l => l.Id == newLink.Id);
                        if (link != null)
                            linkList.Add(link);
                    }
                    team.Links = linkList;
                }

                if (teamDto.RelatedCOBs != null)
                {
                    if (team.RelatedCOBs != null)
                        team.RelatedCOBs.Clear();

                    var cobList = new List<COB>();
                    foreach (var newCOB in teamDto.RelatedCOBs)
                    {
                        var cob = ConsoleRepository.Query<COB>().FirstOrDefault(c => c.Id == newCOB.Id);
                        if (cob != null)
                            cobList.Add(cob);
                    }
                    team.RelatedCOBs = cobList;
                }

                if (teamDto.RelatedOffices != null)
                {
                    if (team.RelatedOffices != null)
                        team.RelatedOffices.Clear();

                    var officeList = new List<Office>();
                    foreach (var newOffice in teamDto.RelatedOffices)
                    {
                        var office = ConsoleRepository.Query<Office>().FirstOrDefault(c => c.Id == newOffice.Id);
                        if (office != null)
                            officeList.Add(office);
                    }
                    team.RelatedOffices = officeList;
                }

                //  TeamMembership
                if (teamDto.AllUsers != null)
                {
                    foreach (var membership in teamDto.AllUsers)
                    {
                        var teamMembership = team.Memberships.FirstOrDefault(tm => tm.UserId == membership.Id);
                        if (teamMembership != null)
                        {
                            if (!membership.IsCurrentMembership)
                            {
                                teamMembership.IsCurrent = false;
                                teamMembership.EndDate = DateTime.Now;

                                // Remove team filters from user
                                var user =
                                    team.Memberships.Where(
                                        tm => tm.UserId == teamMembership.UserId && tm.IsCurrent == false)
                                        .Select(t => t.User)
                                        .FirstOrDefault();

                                if (user != null)
                                {
                                    foreach (var cob in team.RelatedCOBs)
                                    {
                                        user.FilterCOBs.Remove(cob);
                                    }

                                    foreach (var office in team.RelatedOffices)
                                    {
                                        user.FilterOffices.Remove(office);
                                    }
                                }
                            }
                            else
                            {
                                teamMembership.IsCurrent = true;
                                teamMembership.EndDate = null;
                            }
                        }
                    }
                }

                if (teamDto.Users != null)
                {
                    var userList = new List<User>();
                    foreach (var newMem in teamDto.Users)
                    {
                        var currentMem = team.Memberships.FirstOrDefault(m => m.UserId == newMem.Id);
                        var user = ConsoleRepository.Query<User>(u => u.FilterOffices, u => u.FilterCOBs).FirstOrDefault(u => u.Id == newMem.Id);

                        if (currentMem == null)
                        {
                            team.Memberships.Add(new TeamMembership
                                {
                                    TeamId = team.Id,
                                    UserId = newMem.Id,
                                    StartDate = DateTime.Now,
                                    EndDate = null
                                });
                        }
                        else
                        {
                            userList.Add(user);

                            currentMem.IsCurrent = true;
                            currentMem.EndDate = null;
                        }
                    }

                    // Add Team FilterCOBs and FilterOffices to added users
                    foreach (var user in userList)
                    {
                        AddTeamFilters(team.Memberships.Select(tm => tm.TeamId).Distinct(), user, team);
                    }
                }

                ConsoleRepository.SaveChanges();

                return teamDto;
            }
        }

        public string DeleteTeam(Team team)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<Team>(team);
                ConsoleRepository.Delete<Team>(team);
                ConsoleRepository.SaveChanges();
                return "Successfully Deleted Team";
            }
        }

        public int CreateUser(UserDto userDto)
        {
            using (ConsoleRepository)
            {
                var existingUser =
                    ConsoleRepository.Query<User>().FirstOrDefault(u => u.DomainLogon == userDto.DomainLogon);

                if (existingUser != null)
                    throw new ApplicationException("User already exists");

                var user = new User();
                user.DomainLogon = userDto.DomainLogon;
                user.IsActive = userDto.IsActive;
                user.UnderwriterCode = userDto.UnderwriterId;
                user.PrimaryOffice =
                    ConsoleRepository.Query<Office>().FirstOrDefault(o => o.Id == userDto.PrimaryOffice.Id);
                user.DefaultOrigOffice =
                    ConsoleRepository.Query<Office>().FirstOrDefault(o => o.Id == userDto.DefaultOrigOffice.Id);
                user.DefaultUW = ConsoleRepository.Query<User>().FirstOrDefault(d => d.Id == userDto.DefaultUW.Id);

                if (userDto.TeamMemberships != null)
                {
                    var teamMembershipList = new List<TeamMembership>();
                    foreach (var newTeamM in userDto.TeamMemberships)
                    {
                        var team = ConsoleRepository.Query<Team>().FirstOrDefault(t => t.Id == newTeamM.Team.Id);
                        teamMembershipList.Add(new TeamMembership
                            {
                                User = user,
                                Team = team,
                                StartDate = DateTime.Now,
                                EndDate = null,
                                PrimaryTeamMembership = newTeamM.PrimaryTeamMembership
                            });
                    }
                    user.TeamMemberships = teamMembershipList;
                }

                if (userDto.FilterCOBs != null)
                {
                    var filterCobList = new List<COB>();
                    foreach (var newFilterC in userDto.FilterCOBs)
                    {
                        var cob = ConsoleRepository.Query<COB>().FirstOrDefault(c => c.Id == newFilterC.Id);
                        filterCobList.Add(cob);
                    }
                    user.FilterCOBs = filterCobList;
                }

                if (userDto.FilterOffices != null)
                {
                    var filterOfficeList = new List<Office>();
                    foreach (var newFilterO in userDto.FilterOffices)
                    {
                        var office = ConsoleRepository.Query<Office>().FirstOrDefault(c => c.Id == newFilterO.Id);
                        filterOfficeList.Add(office);
                    }
                    user.FilterOffices = filterOfficeList;
                }

                if (userDto.FilterMembers != null)
                {
                    var filterMembersList = new List<User>();
                    foreach (var newFilterM in userDto.FilterMembers)
                    {
                        var member = ConsoleRepository.Query<User>().FirstOrDefault(c => c.Id == newFilterM.Id);
                        filterMembersList.Add(member);
                    }
                    user.FilterMembers = filterMembersList;
                }

                if (userDto.AdditionalCOBs != null)
                {
                    var additionalCobList = new List<COB>();
                    foreach (var newAdditionalC in userDto.AdditionalCOBs)
                    {
                        var cob = ConsoleRepository.Query<COB>().FirstOrDefault(c => c.Id == newAdditionalC.Id);
                        additionalCobList.Add(cob);
                    }
                    user.AdditionalCOBs = additionalCobList;
                }

                if (userDto.AdditionalOffices != null)
                {
                    var additionalOffices = new List<Office>();
                    foreach (var newAdditionalO in userDto.AdditionalOffices)
                    {
                        var office = ConsoleRepository.Query<Office>().FirstOrDefault(c => c.Id == newAdditionalO.Id);
                        additionalOffices.Add(office);
                    }
                    user.AdditionalOffices = additionalOffices;
                }

                if (userDto.AdditionalUsers != null)
                {
                    var additionalUserList = new List<User>();
                    foreach (var newAdditionalU in userDto.AdditionalUsers)
                    {
                        var additionalUser =
                            ConsoleRepository.Query<User>().FirstOrDefault(c => c.Id == newAdditionalU.Id);
                        additionalUserList.Add(additionalUser);
                    }
                    user.AdditionalUsers = additionalUserList;
                }

                // Add Team FilterCOBs and FilterOffices
                AddTeamFilters(userDto.TeamMemberships.Select(tm => tm.Team.Id), user, null);

                ConsoleRepository.Add<User>(user);
                ConsoleRepository.SaveChanges();
                return user.Id;
            }
        }

        public string EditUser(UserDto userDto)
        {
            using (ConsoleRepository)
            {
                var user =
                    ConsoleRepository.Query<User>(u => u.TeamMemberships.Select(tm => tm.Team.RelatedCOBs),
                                                  u => u.TeamMemberships.Select(tm => tm.Team.RelatedOffices),
                                                  u => u.FilterCOBs, u => u.FilterOffices, u => u.FilterMembers,
                                                  u => u.AdditionalCOBs, u => u.AdditionalOffices,
                                                  u => u.AdditionalUsers).FirstOrDefault(u => u.Id == userDto.Id);

                if (user == null)
                    throw new ApplicationException("User to edit not found");
                // TODO: Throw new NullReferenceException(user)

                if (string.IsNullOrEmpty(userDto.UnderwriterId))
                    throw new ApplicationException("Underwriter required");

                if (!ConsoleRepository.Query<Underwriter>().Any(u => u.Code == userDto.UnderwriterId))
                    throw new ApplicationException("Underwriter not recognised");

                ConsoleRepository.Attach<User>(user);

                user.DomainLogon = userDto.DomainLogon;
                user.IsActive = userDto.IsActive;
                user.UnderwriterCode = userDto.UnderwriterId;
                user.PrimaryOffice = (userDto.PrimaryOffice != null)
                                         ? ConsoleRepository.Query<Office>()
                                                            .FirstOrDefault(o => o.Id == userDto.PrimaryOffice.Id)
                                         : null;
                user.DefaultOrigOffice = (userDto.DefaultOrigOffice != null)
                                             ? ConsoleRepository.Query<Office>()
                                                                .FirstOrDefault(
                                                                    o => o.Id == userDto.DefaultOrigOffice.Id)
                                             : null;
                user.DefaultUW = (userDto.DefaultUW != null)
                                     ? ConsoleRepository.Query<User>().FirstOrDefault(d => d.Id == userDto.DefaultUW.Id)
                                     : null;


                //  Must do these bits first because they clear e.g. FilterCOBs
                if (userDto.FilterCOBs != null && userDto.FilterCOBs.Count > 0)
                {
                    if (user.FilterCOBs != null)
                        user.FilterCOBs.Clear();

                    var filterCOBIds = userDto.FilterCOBs.Select(fc => fc.Id);
                    var newFilterCOBs = ConsoleRepository.Query<COB>().Where(fc => filterCOBIds.Contains(fc.Id));
                    foreach (var newFilterC in newFilterCOBs)
                        user.FilterCOBs.Add(newFilterC);
                }
                else
                {
                    user.FilterCOBs.Clear();
                }

                if (userDto.FilterOffices != null && userDto.FilterOffices.Count > 0)
                {
                    if (user.FilterOffices != null)
                        user.FilterOffices.Clear();

                    var filterOfficeIds = userDto.FilterOffices.Select(fo => fo.Id);
                    var newFilterOffices = ConsoleRepository.Query<Office>()
                                                            .Where(fo => filterOfficeIds.Contains(fo.Id));
                    foreach (var newFilterO in newFilterOffices)
                        user.FilterOffices.Add(newFilterO);
                }
                else
                {
                    user.FilterOffices.Clear();
                }

                if (userDto.TeamMemberships != null)
                {
                    foreach (var newMem in userDto.TeamMemberships)
                    {
                        var currentMem = user.TeamMemberships.FirstOrDefault(t => t.TeamId == newMem.Team.Id && t.IsCurrent == true);

                        if (currentMem == null)
                        {
                            user.TeamMemberships.Add(new TeamMembership
                                {
                                    PrimaryTeamMembership = newMem.PrimaryTeamMembership,
                                    TeamId = newMem.Team.Id,
                                    UserId = user.Id,
                                    StartDate = DateTime.Now,
                                    EndDate = null,
                                    IsCurrent = true
                                });
                            
                            //  Only add team filters for newly added memberships.
                            AddTeamFilters(new List<Int32>() { newMem.Team.Id }, user, null);
                        }
                        else
                        {
                            currentMem.PrimaryTeamMembership = newMem.PrimaryTeamMembership;
                            currentMem.IsCurrent = true;
                            currentMem.EndDate = null;
                        }
                    }
                }

                //List<COB> memberFilterCobs = ConsoleRepository.Query<Team>(t => t.Contains(), t => t.RelatedCOBs) 

                if (userDto.FilterMembers != null && userDto.FilterMembers.Count > 0)
                {
                    if (user.FilterMembers != null)
                        user.FilterMembers.Clear();

                    var filterMemberIds = userDto.FilterMembers.Select(fm => fm.Id);
                    var newFilterMembers = ConsoleRepository.Query<User>().Where(fm => filterMemberIds.Contains(fm.Id));
                    foreach (var newFilterM in newFilterMembers)
                        user.FilterMembers.Add(newFilterM);
                }
                else
                {
                    user.FilterMembers.Clear();
                }

                if (userDto.AdditionalCOBs != null && userDto.AdditionalCOBs.Count > 0)
                {
                    if (user.AdditionalCOBs != null)
                        user.AdditionalCOBs.Clear();

                    var additionalCOBIds = userDto.AdditionalCOBs.Select(ac => ac.Id);
                    var newAdditionalCOBs = ConsoleRepository.Query<COB>().Where(ac => additionalCOBIds.Contains(ac.Id));
                    foreach (var newAdditionalC in newAdditionalCOBs)
                        user.AdditionalCOBs.Add(newAdditionalC);
                }
                else
                {
                    user.AdditionalCOBs.Clear();
                }

                if (userDto.AdditionalOffices != null && userDto.AdditionalOffices.Count > 0)
                {
                    if (user.AdditionalOffices != null)
                        user.AdditionalOffices.Clear();

                    var additionalOfficeIds = userDto.AdditionalOffices.Select(ao => ao.Id);
                    var newAdditionalOffices =
                        ConsoleRepository.Query<Office>().Where(ao => additionalOfficeIds.Contains(ao.Id));
                    foreach (var newAdditionalO in newAdditionalOffices)
                        user.AdditionalOffices.Add(newAdditionalO);
                }
                else
                {
                    user.AdditionalOffices.Clear();
                }

                if (userDto.AdditionalUsers != null && userDto.AdditionalUsers.Count > 0)
                {
                    if (user.AdditionalUsers != null)
                        user.AdditionalUsers.Clear();

                    var additionalUserIds = userDto.AdditionalUsers.Select(ao => ao.Id);
                    var newAdditionalUsers =
                        ConsoleRepository.Query<User>().Where(ao => additionalUserIds.Contains(ao.Id));
                    foreach (var newAdditionalU in newAdditionalUsers)
                        user.AdditionalUsers.Add(newAdditionalU);
                }
                else
                {
                    user.AdditionalUsers.Clear();
                }

                if (userDto.AllTeamMemberships != null)
                {
                    //  The membership could be in this list because it was removed from the user in the UI
                    //  Or because it was not added in the UI
                    foreach (var membership in userDto.AllTeamMemberships)
                    {
                        //  Does the user have a current membership for this team?
                        var teamMembership = user.TeamMemberships.FirstOrDefault(tm => tm.TeamId == membership.Team.Id && tm.IsCurrent == true);
                        if (teamMembership != null)
                        {
                            
                            //if (!membership.IsCurrent)
                            //{
                                //  Yes, need to  stop it.
                                teamMembership.IsCurrent = false;
                                teamMembership.EndDate = DateTime.Now;

                                // Remove team filters from user
                                var team = teamMembership.Team;// user.TeamMemberships.Where(tm => tm.TeamId == membership.Team.Id && tm.IsCurrent == false).Select(t => t.Team);

                                if (team != null)
                                {
                                    foreach (var cob in team.RelatedCOBs)
                                    {
                                        //  Should only remove if not in another current membership's team related cobs!                                        
                                        user.FilterCOBs.Remove(cob);
                                    }

                                    foreach (var office in team.RelatedOffices)
                                    {
                                        //  Should only remove if not in another current membership's team related offices!
                                        user.FilterOffices.Remove(office);
                                    }
                                }
                            //}
                            //else
                            //{
                            //    teamMembership.IsCurrent = true;
                            //    teamMembership.EndDate = null;
                            //}
                        }
                    }
                }

                // Add Team FilterCOBs and FilterOffices
                //if (userDto.TeamMemberships != null)
                //    AddTeamFilters(userDto.TeamMemberships.Select(tm => tm.Team.Id), user, null);

                ConsoleRepository.SaveChanges();

                if (CurrentHttpContext.CurrentUser.Identity.Name.ToUpper() == user.DomainLogon.ToUpper())
                    CurrentHttpContext.Context.Session["User"] = null;

                return "User successfully updated";
            }
        }

        public string DeleteUser(User user)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<User>(user);
                ConsoleRepository.Delete<User>(user);
                ConsoleRepository.SaveChanges();
                return "Successfully Deleted User";
            }
        }

        public Link GetLink(int? linkId)
        {
            if (linkId == null) // TODO: Remove nullable from linkId ?
                throw new Exception("linkId is empty"); // TODO: Throw new ArgumentNullException(linkId)

            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<Link>().FirstOrDefault(t => t.Id == linkId.Value);
            }
        }

        public List<Link> GetLinks()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<Link>().ToList();
            }
        }

        public Link CreateLink(Link link)
        {
            // TODO: sort out http etc on url string
            //if(link.Url.sub

            using (ConsoleRepository)
            {
                ConsoleRepository.Add<Link>(link);
                ConsoleRepository.SaveChanges();
                return link;
            }
        }

        public Link EditLink(Link link)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<Link>(link);
                ConsoleRepository.SaveChanges();
                return link;
            }
        }

        public string DeleteLink(Link link)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<Link>(link);
                ConsoleRepository.Delete<Link>(link);
                ConsoleRepository.SaveChanges();
                return "Successfully Deleted Link";
            }
        }

        public List<UserDto> SearchForUserByName(string userName)
        {
            if (userName == null)
                throw new Exception("userName is empty"); // TODO: Throw new ArgumentNullException(userName)

            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<User>().Where(u => u.DomainLogon.Contains(userName)).ToList().Select(u =>
                                                                                                                    new UserDto
                                                                                                                        {
                                                                                                                            Id
                                                                                                                                =
                                                                                                                                u
                                                                                                                        .Id,
                                                                                                                            DomainLogon
                                                                                                                                =
                                                                                                                                u
                                                                                                                        .DomainLogon
                                                                                                                        })
                                        .ToList();
            }

        }

        public UserDto GetUserPersonalSettings()
        {
            User user = WebSiteModuleManager.EnsureCurrentUser();
            var userDto = new UserDto();
            SetUserDto(userDto, user, ConsoleRepository);
            return userDto;
        }

        public UserDto GetSelectedUserByName(string userName)
        {
            if (userName == null)
                throw new Exception("userName is empty"); // TODO: Throw new ArgumentNullException(userName)

            var userDto = new UserDto();

            using (ConsoleRepository)
            {
                
                var user = 
                    ConsoleRepository.Query<User>(t => t.DomainLogon == userName, 
                                                t => t.FilterCOBs,
                                                t => t.FilterOffices,
                                                t => t.AdditionalCOBs,
                                                t => t.AdditionalOffices,
                                                t => t.DefaultOrigOffice,
                                                t => t.PrimaryOffice, 
                                                t => t.FilterMembers, 
                                                t => t.AdditionalUsers, 
                                                t => t.DefaultUW, t => t.TeamMemberships.Select(tm => tm.Team.PricingActuary))
                                     .SingleOrDefault();

                if (user != null)
                {
                    SetUserDto(userDto, user, ConsoleRepository);
                }
            }

            return userDto;
        }

        public UserDto GetUser(int? userId)
        {
            if (userId == null) // TODO: Remove nullable from userId
                throw new Exception("userId is empty"); // TODO: throw new ArgumentNullException(userId)

            var userDto = new UserDto();
            using (ConsoleRepository)
            {
                var user =
                    ConsoleRepository.Query<User>(t => t.Id == userId.Value, 
                                                    t => t.PrimaryOffice, 
                                                    t => t.FilterMembers,
                                                    t => t.FilterCOBs, 
                                                    t => t.AdditionalUsers,
                                                    t => t.TeamMemberships.Select(tm => tm.Team), 
                                                    t => t.DefaultUW).FirstOrDefault();

                if (user != null)
                {
                    SetUserDto(userDto, user, ConsoleRepository);
                }
            }

            return userDto;
        }

        public List<Link> GetLinksForTeam(int? teamId)
        {
            // TODO: Remove nullable from teamId ?
            // TODO: Where is the argument null reference check ?

            using (ConsoleRepository)
            {
                var firstOrDefault =
                    ConsoleRepository.Query<Team>(t => t.Id == teamId.Value, t => t.Links).FirstOrDefault();
                if (firstOrDefault != null)
                    return firstOrDefault.Links.ToList();
            }

            return null;
        }

        public string SaveLinksForTeam(TeamLinksDto teamLinksDto)
        {
            using (ConsoleRepository)
            {
                // Check does Team Links exist
                var team = ConsoleRepository.Query<Team>(t => t.Links).FirstOrDefault(t => t.Id == teamLinksDto.TeamId);

                bool linksChanged = false;
                if (team != null)
                {
                    // Remove links that need to be removed
                    var currentTeamLinks = team.Links.Select(t => t.Id).ToList();
                    var linksToRemove = currentTeamLinks.Except(teamLinksDto.TeamLinksIdList).ToList();

                    foreach (var linkId in linksToRemove)
                    {
                        var linkToDelete = ConsoleRepository.Query<Link>().FirstOrDefault(l => l.Id == linkId);

                        if (linkToDelete != null && team.Links.Any(l => l.Equals(linkToDelete)))
                        {
                            team.Links.Remove(linkToDelete);
                            linksChanged = true;
                        }
                    }

                    // Add Links that need to be added
                    foreach (var linkId in teamLinksDto.TeamLinksIdList)
                    {
                        if (team.Links.Any(l => l.Id == linkId)) continue;
                        var linkToAdd = ConsoleRepository.Query<Link>().FirstOrDefault(l => l.Id == linkId);

                        team.Links.Add(linkToAdd);
                        linksChanged = true;
                    }

                    ConsoleRepository.SaveChanges();

                    if (linksChanged)
                        return "Saved Link(s) for Team";
                    else
                        return "Link(s) for Team have not changed";
                }
                else
                {
                    return "Save Links - Team does not Exist";
                }
            }
        }

        public static RequiredDataUserDto GetRequiredDataEditUser(string userName, IConsoleRepository _consoleRepository)
        {
            var requiredDataDto = new RequiredDataUserDto();
            using (_consoleRepository)
            {
                var user = _consoleRepository.Query<User>(u => u.FilterCOBs,
                                            u => u.FilterOffices,
                                            u => u.FilterMembers,
                                            u => u.AdditionalCOBs,
                                            u => u.AdditionalOffices,
                                            u => u.AdditionalUsers,
                                            u => u.OpenTabs,
                                            u => u.TeamMemberships.Select(t => t.Team),
                                            u => u.DefaultOrigOffice,
                                            u => u.DefaultUW,
                                            u => u.Underwriter,
                                            u => u.DefaultUW.Underwriter).FirstOrDefault(u => u.DomainLogon == userName);

                if (user == null)
                    throw new Exception("User not Found"); // TODO: throw new NullReferenceException(user)

                var allTeams = _consoleRepository.Query<Team>().Select(t => t).ToList();

                var currentUserTeamMemberships =
                    _consoleRepository.Query<TeamMembership>()
                                      .Where(tm => tm.User.DomainLogon.Contains(userName) && tm.IsCurrent)
                                      .Select(t => t.Team)
                                      .ToList();

                var allTeamMemberships = allTeams.Except(currentUserTeamMemberships).Select(t =>
                    {
                        // Create a new empty team Membership
                        return new TeamMembershipDto
                            {
                                Id = 0,
                                Team = new TeamDto { Id = t.Id, Title = t.Title },
                                EndDate = null,
                                StartDate = new DateTime()
                            };

                    }).ToList();

                // 2. Get AllFilterCOBs Except the ones that the user is already in
                var currentFilterCOBs = user.FilterCOBs.Select(fc => fc);

                var allFilterCOBs = _consoleRepository.Query<COB>().ToList().Except(currentFilterCOBs).Select(c =>
                    {
                        return new COBDto { Id = c.Id, Narrative = c.Narrative };

                    }).ToList();

                // 3. AllFilterOffices
                var currentFilterOffices = user.FilterOffices.Select(fo => fo);

                var allFilterOffices =
                    _consoleRepository.Query<Office>().ToList().Except(currentFilterOffices).Select(o =>
                        {
                            return new OfficeDto { Id = o.Id, Title = o.Name };

                        }).ToList();

                // 4. AllFilterMembers
                var currentFilterMembers = user.FilterMembers.Select(fm => fm);

                var allFilterMembers =
                    _consoleRepository.Query<User>().ToList().Except(currentFilterMembers).Select(u =>
                        {
                            return new UserDto { Id = u.Id, DomainLogon = u.DomainLogon };

                        }).ToList();

                // 5. AllAdditionalCOBs
                var currentAdditionalCOBs = user.AdditionalCOBs.Select(ac => ac);

                var allAdditionalCOBs =
                    _consoleRepository.Query<COB>().ToList().Except(currentAdditionalCOBs).Select(c =>
                        {
                            return new COBDto { Id = c.Id, Narrative = c.Narrative };

                        }).ToList();

                // 6. AllAdditionalOffices
                var currentAdditionalOffices = user.AdditionalOffices.Select(ao => ao);

                var allAdditionalOffices =
                    _consoleRepository.Query<Office>().ToList().Except(currentAdditionalOffices).Select(o =>
                        {
                            return new OfficeDto { Id = o.Id, Title = o.Name };

                        }).ToList();

                // 7. AllAdditionalUsers 
                var currentAdditionalUsers = user.AdditionalUsers.Select(au => au);

                var allAdditionalUsers =
                    _consoleRepository.Query<User>().ToList().Except(currentAdditionalUsers).Select(u =>
                        {
                            return new UserDto { Id = u.Id, DomainLogon = u.DomainLogon };

                        }).ToList();

                requiredDataDto.AllTeamMemberships = allTeamMemberships;
                requiredDataDto.AllFilterCOBs = allFilterCOBs;
                requiredDataDto.AllFilterOffices = allFilterOffices;
                requiredDataDto.AllFilterMembers = allFilterMembers;
                requiredDataDto.AllAdditionalCOBs = allAdditionalCOBs;
                requiredDataDto.AllAdditionalOffices = allAdditionalOffices;
                requiredDataDto.AllAdditionalUsers = allAdditionalUsers;
            }

            return requiredDataDto;
        }

        public RequiredDataUserDto GetRequiredDataCreateUser()
        {
            var requiredDataDto = new RequiredDataUserDto();
            using (ConsoleRepository)
            {
                // 1. Get All TeamsMemberships
                var allTeamMemberships = ConsoleRepository.Query<Team>().ToList().Select(tm =>
                    {
                        return new TeamMembershipDto
                            {
                                Id = tm.Id,
                                EndDate = null,
                                StartDate = DateTime.Now,
                                Team = new TeamDto { Id = tm.Id, Title = tm.Title }
                            };

                    }).ToList();

                // 2. Get AllFilterCOBs 
                var allCOBs = ConsoleRepository.Query<COB>().ToList().Select(c =>
                    {
                        return new COBDto { Id = c.Id, Narrative = c.Narrative };

                    }).ToList();

                // 3. AllFilterOffices
                var allOffices = ConsoleRepository.Query<Office>().ToList().Select(o =>
                    {
                        return new OfficeDto { Id = o.Id, Title = o.Name };

                    }).ToList();

                // 4. AllFilterMembers
                var allUsers = ConsoleRepository.Query<User>().ToList().Select(u =>
                    {
                        return new UserDto { Id = u.Id, DomainLogon = u.DomainLogon };

                    }).ToList();

                requiredDataDto.AllTeamMemberships = allTeamMemberships;
                requiredDataDto.AllFilterCOBs = allCOBs;
                requiredDataDto.AllFilterOffices = allOffices;
                requiredDataDto.AllFilterMembers = allUsers;
                requiredDataDto.AllAdditionalCOBs = allCOBs;
                requiredDataDto.AllAdditionalOffices = allOffices;
                requiredDataDto.AllAdditionalUsers = allUsers;

                requiredDataDto.AllPrimaryOffices = allOffices;
                requiredDataDto.AllOriginatingOffices = allOffices;
                requiredDataDto.AllDefaultUnderwriters = allUsers;
            }

            return requiredDataDto;
        }

        public RequiredDataCreateTeamDto GetRequiredDataCreateTeam()
        {
            var requiredDto = new RequiredDataCreateTeamDto();
            using (ConsoleRepository)
            {
                var teams =
                    ConsoleRepository.Query<Team>(t => t.Memberships.Select(tm => tm.User), t => t.RelatedCOBs,
                                                  t => t.RelatedOffices, t => t.RelatedOffices).ToList();

                teams.ForEach(team =>
                    {
                        var allUsers = new List<UserDto>();
                        var allLinks = new List<LinkDto>();
                        var allRelatedCobs = new List<COBDto>();
                        var allRelatedOffices = new List<OfficeDto>();

                        allUsers = ConsoleRepository.Query<User>().ToList().Select(u =>
                            {
                                return new UserDto { Id = u.Id, DomainLogon = u.DomainLogon };

                            }).ToList();

                        allLinks = ConsoleRepository.Query<Link>().ToList().Select(l =>
                            {
                                return new LinkDto { Id = l.Id, Category = l.Category, Url = l.Url, Title = l.Title };

                            }).ToList();

                        allRelatedCobs = ConsoleRepository.Query<COB>().ToList().Select(c =>
                            {
                                return new COBDto { Id = c.Id, Narrative = c.Narrative };

                            }).ToList();

                        allRelatedOffices = ConsoleRepository.Query<Office>().ToList().Select(o =>
                            {
                                return new OfficeDto { Id = o.Id, Title = o.Name };

                            }).ToList();

                        requiredDto.AllLinks = allLinks;
                        requiredDto.AllRelatedCOBs = allRelatedCobs;
                        requiredDto.AllRelatedOffices = allRelatedOffices;
                        requiredDto.AllUsers = allUsers;

                    });
            }

            return requiredDto;
        }

        public List<UserTeamLinkDto> GetUserTeamLinks()
        {
            var userName = HttpContext.Current.User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
                throw new Exception("UserName is null"); // TODO: throw new NullReferenceException(userName)

            using (ConsoleRepository)
            {
                // 1. Get user by name
                var user =
                    ConsoleRepository.Query<User>(u => u.DomainLogon == userName,
                                                  t => t.TeamMemberships.Select(tm => tm.Team.Links)).FirstOrDefault();

                if (user == null)
                    throw new Exception("User not found");

                // 2. Get all the teams they are in
                var userTeamLinkList = new List<UserTeamLinkDto>();

                var allUrls = new List<Url>();
                foreach (var teamMembership in user.TeamMemberships)
                {
                    allUrls.AddRange(teamMembership.Team.Links.Select(l =>
                        {
                            if (!string.IsNullOrEmpty(l.Url))
                            {
                                return new Url { LinkUrl = l.Url, Title = l.Title, LinkCategory = l.Category };
                            }

                            return null;

                        }).ToList());
                }

                var distinctUrls = allUrls.Select(u => u).Distinct();

                var groupedUrls = distinctUrls.GroupBy(u => u.LinkCategory);

                foreach (var category in groupedUrls)
                {
                    var urls = category.Select(u =>
                        {
                            return new Url { Title = u.Title, LinkUrl = u.LinkUrl };
                        }).ToList();


                    userTeamLinkList.Add(new UserTeamLinkDto { CategoryName = category.Key, Urls = urls });
                }

                return userTeamLinkList;
            }
        }

        public TeamDto GetTeamBySubmissionTypeId(string submissionTypeId)
        {
            if (string.IsNullOrEmpty(submissionTypeId))
                throw new Exception("submissionTypeId is empty");

            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<Team>().Select(tm => new TeamDto { Id = tm.Id, Title = tm.Title, SubmissionTypeId = tm.SubmissionTypeId }).SingleOrDefault(t => t.SubmissionTypeId == submissionTypeId);
            }
        }

        #endregion

        #region QuoteTemplate

        public QuoteTemplate GetQuoteTemplate(int? quoteTemplateId)
        {
            if (quoteTemplateId == null) // TODO: Remove nullable from linkId ?
                throw new Exception("quoteTemplateId is empty"); // TODO: Throw new ArgumentNullException(linkId)

            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<QuoteTemplate>().FirstOrDefault(t => t.Id == quoteTemplateId.Value);
            }
        }

        public List<QuoteTemplate> GetQuoteTemplates()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<QuoteTemplate>().ToList();
            }
        }

        public QuoteTemplate CreateQuoteTemplate(QuoteTemplate quoteTemplate)
        {

            using (ConsoleRepository)
            {
                ConsoleRepository.Add<QuoteTemplate>(quoteTemplate);
                ConsoleRepository.SaveChanges();
                return quoteTemplate;
            }
        }

        public QuoteTemplate EditQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<QuoteTemplate>(quoteTemplate);
                ConsoleRepository.SaveChanges();
                return quoteTemplate;
            }
        }

        public string DeleteQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<QuoteTemplate>(quoteTemplate);
                ConsoleRepository.Delete<QuoteTemplate>(quoteTemplate);
                ConsoleRepository.SaveChanges();
                return "Successfully Deleted QuoteTemplate";
            }
        }

        public List<QuoteTemplate> GetQuoteTemplatesForTeam(int? teamId)
        {
            // TODO: Remove nullable from teamId ?
            // TODO: Where is the argument null reference check ?

            using (ConsoleRepository)
            {
                var firstOrDefault =
                    ConsoleRepository.Query<Team>(t => t.Id == teamId.Value, t => t.RelatedQuoteTemplates)
                                     .FirstOrDefault();
                if (firstOrDefault != null)
                    return firstOrDefault.RelatedQuoteTemplates.ToList();
            }

            return null;
        }

        public string SaveQuoteTemplatesForTeam(TeamQuoteTemplatesDto teamQuoteTemplatesDto)
        {
            using (ConsoleRepository)
            {
                // Check does Team Links exist
                var team =
                    ConsoleRepository.Query<Team>(t => t.Id == teamQuoteTemplatesDto.TeamId,
                                                  t => t.RelatedQuoteTemplates).FirstOrDefault();

                var quoteTemplatesChanged = false;
                if (team != null)
                {
                    // Remove links that need to be removed
                    var currentTeamQuoteTemplates = team.RelatedQuoteTemplates.Select(qt => qt.Id).ToList();

                    var quoteTemplatesToRemove =
                        currentTeamQuoteTemplates.Except(teamQuoteTemplatesDto.TeamQuoteTemplatesIdList ??
                                                         new List<int>()).ToList();

                    foreach (var quoteTemplateId in quoteTemplatesToRemove)
                    {
                        var quoteTemplateToDelete =
                            ConsoleRepository.Query<QuoteTemplate>().FirstOrDefault(qt => qt.Id == quoteTemplateId);

                        if (quoteTemplateToDelete != null &&
                            team.RelatedQuoteTemplates.Any(qt => qt.Equals(quoteTemplateToDelete)))
                        {
                            team.RelatedQuoteTemplates.Remove(quoteTemplateToDelete);
                            quoteTemplatesChanged = true;
                        }
                    }

                    // Add Links that need to be added
                    foreach (var quoteTemplateId in teamQuoteTemplatesDto.TeamQuoteTemplatesIdList ?? new List<int>())
                    {
                        if (team.RelatedQuoteTemplates.Any(qt => qt.Id == quoteTemplateId)) continue;
                        var quoteTemplateToAdd =
                            ConsoleRepository.Query<QuoteTemplate>().FirstOrDefault(qt => qt.Id == quoteTemplateId);

                        team.RelatedQuoteTemplates.Add(quoteTemplateToAdd);
                        quoteTemplatesChanged = true;
                    }

                    ConsoleRepository.SaveChanges();

                    if (quoteTemplatesChanged)
                        return "Saved QuoteTemplate(s) for Team";
                    else
                        return "QuoteTemplate(s) for Team have not changed";
                }
                else
                {
                    return "Save QuoteTemplates - Team does not Exist";
                }
            }
        }

        public List<UserTeamQuoteTemplateDto> GetUserTeamQuoteTemplates()
        {
            var userName = HttpContext.Current.User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
                throw new Exception("UserName is null"); // TODO: throw new NullReferenceException(userName)

            using (ConsoleRepository)
            {
                // 1. Get user by name
                var user =
                    ConsoleRepository.Query<User>(u => u.DomainLogon == userName,
                                                  t => t.TeamMemberships.Select(tm => tm.Team.Links)).FirstOrDefault();

                if (user == null)
                    throw new Exception("User not found");

                // 2. Get all the teams they are in
                var userTeamQuoteTemplateList = new List<UserTeamQuoteTemplateDto>();


                foreach (var teamMembership in user.TeamMemberships)
                {
                    userTeamQuoteTemplateList =
                        teamMembership.Team.RelatedQuoteTemplates.Select(
                            qt => new UserTeamQuoteTemplateDto { Name = qt.Name, RdlPath = qt.RdlPath }).ToList();
                }

                return userTeamQuoteTemplateList;
            }
        }

        #endregion

        #region Accelerators

        public AppAccelerator CreateAccelerator(AppAccelerator accelerator)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Add<AppAccelerator>(accelerator);
                ConsoleRepository.SaveChanges();
                return accelerator;
            }
        }

        public AppAccelerator EditAccelerator(AppAccelerator accelerator)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<AppAccelerator>(accelerator);
                ConsoleRepository.SaveChanges();
                return accelerator;
            }
        }

        public string DeleteAccelerator(AppAccelerator accelerator)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Attach<AppAccelerator>(accelerator);
                ConsoleRepository.Delete<AppAccelerator>(accelerator);
                ConsoleRepository.SaveChanges();
                return "Successfully Deleted accelerator";
            }
        }

        public List<AppAccelerator> GetAccelerators()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<AppAccelerator>().ToList();
            }
        }

        public List<AppAccelerator> GetAcceleratorsForTeam(int? teamId)
        {
            // TODO: Remove nullable from teamId ?
            // TODO: Where is the argument null reference check ?

            using (ConsoleRepository)
            {
                var firstOrDefault =
                    ConsoleRepository.Query<Team>(t => t.Id == teamId.Value, t => t.AppAccelerators).FirstOrDefault();
                if (firstOrDefault != null)
                    return firstOrDefault.AppAccelerators.ToList();
            }

            return null;
        }

        public string SaveAcceleratorsForTeam(TeamAppAcceleratorsDto teamAppAcceleratorsDto)
        {
            using (ConsoleRepository)
            {
                // Check does Team Accelerators exist
                var team = ConsoleRepository.Query<Team>(t => t.AppAccelerators).FirstOrDefault(t => t.Id == teamAppAcceleratorsDto.TeamId);

                bool appAcceleratorsChanged = false;
                if (team != null)
                {
                    // Remove Accelerators that need to be removed
                    var currentTeamAppAccelerators = team.AppAccelerators.Select(t => t.Id).ToList();
                    var appAcceleratorsToRemove =
                        currentTeamAppAccelerators.Except(teamAppAcceleratorsDto.TeamAppAcceleratorsIdList ??
                                                          new List<string>()).ToList();

                    foreach (var appAcceleratorId in appAcceleratorsToRemove.Where(a => !String.IsNullOrEmpty(a)))
                    {
                        var aa = team.AppAccelerators.FirstOrDefault(a => a.Id == appAcceleratorId);
                        team.AppAccelerators.Remove(aa);
                        appAcceleratorsChanged = true;
                    }

                    // Add Accelerators that need to be added
                    foreach (
                        var appAcceleratorId in teamAppAcceleratorsDto.TeamAppAcceleratorsIdList ?? new List<string>())
                    {
                        if (team.AppAccelerators.Any(a => a.Id == appAcceleratorId)) continue;
                        if (!String.IsNullOrEmpty(appAcceleratorId))
                        {
                            var appAcceleratorToAdd = new AppAccelerator() { Id = appAcceleratorId };
                            ConsoleRepository.AttachUnchanged<AppAccelerator>(appAcceleratorToAdd);

                            team.AppAccelerators.Add(appAcceleratorToAdd);
                            appAcceleratorsChanged = true;
                        }
                    }

                    ConsoleRepository.SaveChanges();

                    if (appAcceleratorsChanged)
                        return "Saved Accelerator(s) for Team";
                    else
                        return "Accelerator(s) for Team have not changed";
                }
                else
                {
                    return "Save Accelerators - Team does not Exist";
                }
            }
        }

        public List<UserTeamAcceleratorDto> GetUserTeamAccelerators()
        {
            var userName = HttpContext.Current.User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
                throw new Exception("UserName is null"); // TODO: throw new NullReferenceException(userName)

            using (ConsoleRepository)
            {
                // 1. Get user by name
                var user =
                    ConsoleRepository.Query<User>(u => u.DomainLogon == userName,
                                                  u => u.TeamMemberships.Select(t => t.Team.AppAccelerators))
                                     .FirstOrDefault();

                if (user == null)
                    throw new Exception("User not found");

                // 2. Get all the teams they are in
                var userTeamAppAcceleratorList = new List<UserTeamAcceleratorDto>();

                var allAppAcceleratorDtos = new List<AppAcceleratorDto>();
                foreach (var teamMembership in user.TeamMemberships)
                {
                    var teamAppAcceleratorDtos = teamMembership.Team.AppAccelerators.Select(a =>
                        {
                            if (!string.IsNullOrEmpty(a.DisplayName))
                            {
                                return new AppAcceleratorDto
                                    {
                                        Id = a.Id,
                                        DisplayName = a.DisplayName,
                                        Category = a.ActivityCategory
                                    };
                            }

                            return null;

                        }).ToList();

                    allAppAcceleratorDtos.AddRange(teamAppAcceleratorDtos);
                }

                var distinctAppAcceleratorDtos = allAppAcceleratorDtos.GroupBy(u => u.Id).Select(g => g.First());

                var groupeAcceleratorDtos = distinctAppAcceleratorDtos.GroupBy(u => u.Category);

                foreach (var category in groupeAcceleratorDtos)
                {
                    var appAcceleratorDtos = category.Select(a =>
                        {
                            return new AppAcceleratorDto { Id = a.Id, DisplayName = a.DisplayName };
                        }).ToList();


                    userTeamAppAcceleratorList.Add(new UserTeamAcceleratorDto
                        {
                            CategoryName = category.Key,
                            AppAcceleratorDtos = appAcceleratorDtos
                        });
                }

                return userTeamAppAcceleratorList;
            }
        }

        public string GetAcceleratorMetaDataById(string id)
        {
            const string template = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <openServiceDescription
                                xmlns=""http://www.microsoft.com/schemas/openservicedescription/1.0"">
                                <homepageUrl>http://@Model.HomepageUrl</homepageUrl>
                                <display>    
                                    <name>@Model.DisplayName</name>
                                    <icon>http://@Model.DisplayIcon</icon>
                                </display>
                                <activity category=""@Model.ActivityCategory "">
                                    <activityAction context=""selection"" >
                                        <preview action=""http://@Model.ActivityActionPreview"">
                                        </preview>
                                        <execute action=""http://@Model.ActivityActionExecute "">
                                        </execute>
                                    </activityAction>
                                </activity>
                            </openServiceDescription>";

            using (ConsoleRepository)
            {
                var appAccelerator = ConsoleRepository.Query<AppAccelerator>(a => a.Id == id).FirstOrDefault();
                var result = Razor.Parse(template, appAccelerator);
                var xml = new XmlDocument();
                xml.LoadXml(result);
                return xml.InnerXml;
            }
            return string.Empty;

            //if (CurrentHttpContext.Context.Request.Url != null)
            //{
            //    var result = Razor.Parse(template, new AppAccelerator
            //    {
            //        HomepageUrl =
            //            CurrentHttpContext.Context.Request.Url.Host + ":" +
            //            CurrentHttpContext.Context.Request.Url.Port,
            //        DisplayName = "WorldCheck",
            //        DisplayIcon =
            //            CurrentHttpContext.Context.Request.Url.Host + ":" +
            //            CurrentHttpContext.Context.Request.Url.Port + @"/Content/images/validus_logo_smaller.png",
            //        ActivityCategory = "Validus",
            //        ActivityActionPreview =
            //            CurrentHttpContext.Context.Request.Url.Host + ":" +
            //            CurrentHttpContext.Context.Request.Url.Port +
            //            "/WorldCheck/_WorldCheckSearchMatches?insuredName={selection}",
            //        ActivityActionExecute =
            //            CurrentHttpContext.Context.Request.Url.Host + ":" +
            //            CurrentHttpContext.Context.Request.Url.Port +
            //            "/WorldCheck/_WorldCheckSearchMatches?insuredName={selection}",
            //    });
            //}

        }

        #endregion

        #region Market Wording

        public MarketWording CreateMarketWording(MarketWording marketWording)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Add(marketWording);
                ConsoleRepository.SaveChanges();
                return marketWording;
            }
        }

        public MarketWording EditMarketWording(MarketWording marketWording)
        {
            using (var transactopnScope = new System.Transactions.TransactionScope(TransactionScopeOption.Required,
   new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            using (ConsoleRepository)
            {
                ConsoleRepository.Query<MarketWording>(mw => mw.Id == marketWording.Id).First().Title = marketWording.Title;
                //ConsoleRepository.Attach(marketWording);
                ConsoleRepository.SaveChanges();
                //Get the new version
                var newMarketWording =
                    ConsoleRepository.Query<MarketWording>(mw => mw.Key == marketWording.Key && mw.IsObsolete == false)
                                     .First();
                var affectedTeamOfficeSettings =
                    ConsoleRepository.Query<MarketWordingSetting>(mws => mws.MarketWording.Key == marketWording.Key).ToList();
                foreach (var affectedTeamOfficeSetting in affectedTeamOfficeSettings)
                {
                    affectedTeamOfficeSetting.MarketWording = newMarketWording;
                }
                ConsoleRepository.SaveChanges();
                transactopnScope.Complete();
                return newMarketWording;
            }
        }

        public string DeleteMarketWording(MarketWording marketWording)
        {
            using (ConsoleRepository)
            {
                try
                {
                    ConsoleRepository.Attach(marketWording);
                    ConsoleRepository.Delete(marketWording);
                    ConsoleRepository.SaveChanges();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Failed";
                }

            }
        }

        public List<MarketWording> GetMarketWordings()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<MarketWording>(mw => mw.WordingType != "Custom" || mw.WordingType == null).ToList();
            }
        }

        public List<MarketWording> GetMarketWordingsByPaging(string searchTerm, String sortCol, string sortDir, int skip,
                                                             int take, out Int32 count, out Int32 totalCount)
        {
            using (ConsoleRepository)
            {
                var filteredMarketWordingsQuery = ConsoleRepository.Query<MarketWording>(
                    mw =>
                    mw.WordingType != "Custom" ||
                    mw.WordingType == null &&
                    (mw.WordingRefNumber.Contains(searchTerm) || mw.Title.Contains(searchTerm)));

                count = totalCount = filteredMarketWordingsQuery.Count();

                return String.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase)
                           ? filteredMarketWordingsQuery.OrderBy(sortCol)
                                                        .Skip(skip)
                                                        .Take(take)
                                                        .ToList()
                           : filteredMarketWordingsQuery.OrderByDescending(sortCol)
                                                        .Skip(skip)
                                                        .Take(take)
                                                        .ToList();
            }
        }

        public List<MarketWordingSetting> GetMarketWordingsForTeamOffice(int? teamId, string officeId)
        {
            // TODO: Remove nullable from teamId ?
            // TODO: Where is the argument null reference check ?

            using (ConsoleRepository)
            {
                var team =
                    ConsoleRepository.Query<Team>(t => t.Id == teamId.Value, t => t.TeamOfficeSettings.Select(tos => tos.MarketWordingSettings.Select(mws => mws.MarketWording)), t => t.TeamOfficeSettings.Select(tos => tos.Office)).FirstOrDefault();
                if (team != null)
                {
                    var teamOfficeSetting = team.TeamOfficeSettings.FirstOrDefault(o => o.Office.Id == officeId);
                    if (teamOfficeSetting != null)
                        return teamOfficeSetting.MarketWordingSettings.OrderBy(mws => mws.DisplayOrder).ToList();
                }
            }

            return new List<MarketWordingSetting>();
        }

        public string SaveMarketWordingsForTeamOffice(TeamMarketWordingsDto teamMarketWordingsDto)
        {
            using (ConsoleRepository)
            {
                // Check does Team Market wordings exist
                var team = ConsoleRepository.Query<Team>(t => t.Id == teamMarketWordingsDto.TeamId, t => t.TeamOfficeSettings.Select(tos => tos.Office), t => t.TeamOfficeSettings.Select(tos => tos.MarketWordingSettings.Select(mws => mws.MarketWording))).FirstOrDefault();

                bool teamOfficeMarketWordingSettingsChanged = false;
                if (team != null)
                {
                    //<??> create office setting if one doesn't exist
                    if (team.TeamOfficeSettings == null ||
                        !team.TeamOfficeSettings.Any(tos => tos.Office.Id == teamMarketWordingsDto.OfficeId))
                    {
                        team.TeamOfficeSettings = team.TeamOfficeSettings ?? new List<TeamOfficeSetting>();
                        team.TeamOfficeSettings.Add(new TeamOfficeSetting { Office = ConsoleRepository.Query<Office>(o => o.Id == teamMarketWordingsDto.OfficeId).First(), MarketWordingSettings = new List<MarketWordingSetting>() });

                    }

                    var teamOfficeSetting =
                        team.TeamOfficeSettings.First(tos => tos.Office.Id == teamMarketWordingsDto.OfficeId);
                    // Remove MarketWording that need to be removed

                    var currentTeamOfficeMarketWordings = teamOfficeSetting.MarketWordingSettings.Select(mws => mws.MarketWording.Id).ToList();
                    var teamOfficeMarketWordingsToRemove =
                        currentTeamOfficeMarketWordings.Except(teamMarketWordingsDto.MarketWordingSettingDtoList != null ? teamMarketWordingsDto.MarketWordingSettingDtoList.Select(tnos => tnos.Id) : new List<int>()).ToList();

                    foreach (var teamOfficeMarketWordingId in teamOfficeMarketWordingsToRemove)
                    {
                        var teamOfficeMarketWordingToDelete =
                            ConsoleRepository.Query<MarketWording>().FirstOrDefault(mw => mw.Id == teamOfficeMarketWordingId);

                        if (teamOfficeMarketWordingToDelete != null)
                        {
                            var teamOfficeMarketWordingSettingsToDelete = teamOfficeSetting.MarketWordingSettings.FirstOrDefault(mws => mws.MarketWording.Equals(teamOfficeMarketWordingToDelete));
                            //teamOfficeSetting.MarketWordingSettings.Remove(teamOfficeMarketWordingSettingsToDelete);
                            ConsoleRepository.Delete(teamOfficeMarketWordingSettingsToDelete);
                            teamOfficeMarketWordingSettingsChanged = true;
                        }
                    }

                    // Add MarketWordings that need to be added
                    foreach (var marketWordingSetting in teamMarketWordingsDto.MarketWordingSettingDtoList ?? new List<MarketWordingSettingDto>())
                    {
                        if (teamOfficeSetting.MarketWordingSettings.Any(mws => mws.MarketWording.Id == marketWordingSetting.Id)) continue;
                        var marketWordingToAdd =
                            ConsoleRepository.Query<MarketWording>().FirstOrDefault(a => a.Id == marketWordingSetting.Id);

                        teamOfficeSetting.MarketWordingSettings.Add(new MarketWordingSetting { MarketWording = marketWordingToAdd });
                        teamOfficeMarketWordingSettingsChanged = true;
                    }

                    //Update display order
                    foreach (
                        var marketWordingSettingDto in
                            teamMarketWordingsDto.MarketWordingSettingDtoList ?? new List<MarketWordingSettingDto>())
                    {
                        var marketWordingSetting = teamOfficeSetting.MarketWordingSettings.First(
                            tncs => tncs.MarketWording.Id == marketWordingSettingDto.Id);
                        if (marketWordingSetting.DisplayOrder != marketWordingSettingDto.DisplayOrder)
                        {

                            marketWordingSetting.DisplayOrder = marketWordingSettingDto.DisplayOrder;
                            teamOfficeMarketWordingSettingsChanged = true;
                    }

                    }

                    ConsoleRepository.SaveChanges();

                    if (teamOfficeMarketWordingSettingsChanged)
                        return "Saved MarketWording(s) for Team-Office";
                    else
                        return "MarketWording(s) for Team-Office have not changed";
                }
                else
                {
                    return "Save MarketWordings - Team does not Exist";
                }
            }
        }

        #endregion

        #region SubjectToClauseWording

        public SubjectToClauseWording CreateSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Add(subjectToClauseWording);
                ConsoleRepository.SaveChanges();
                return subjectToClauseWording;
            }
        }

        public SubjectToClauseWording EditSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording)
        {
            using (var transactopnScope = new System.Transactions.TransactionScope(TransactionScopeOption.Required,
  new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            using (ConsoleRepository)
            {
                ConsoleRepository.Query<SubjectToClauseWording>(mw => mw.Id == subjectToClauseWording.Id).First().Title = subjectToClauseWording.Title;
                // ConsoleRepository.Attach(subjectToClauseWording);
                ConsoleRepository.SaveChanges();
                //Get the new version
                var newSubjectToClauseWording =
                    ConsoleRepository.Query<SubjectToClauseWording>(mw => mw.Key == subjectToClauseWording.Key && mw.IsObsolete == false)
                                     .First();
                var affectedTeamOfficeSettings =
                    ConsoleRepository.Query<SubjectToClauseWordingSetting>(stc => stc.SubjectToClauseWording.Key == subjectToClauseWording.Key).ToList();
                foreach (var affectedTeamOfficeSetting in affectedTeamOfficeSettings)
                {
                    affectedTeamOfficeSetting.SubjectToClauseWording = newSubjectToClauseWording;
                }
                ConsoleRepository.SaveChanges();
                transactopnScope.Complete();
                return newSubjectToClauseWording;
            }
        }

        public string DeleteSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording)
        {
            using (ConsoleRepository)
            {
                try
                {
                    ConsoleRepository.Attach(subjectToClauseWording);
                    ConsoleRepository.Delete(subjectToClauseWording);
                    ConsoleRepository.SaveChanges();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Failed";
                }

            }
        }

        public List<SubjectToClauseWording> GetSubjectToClauseWordings()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<SubjectToClauseWording>(stc => stc.WordingType != "Custom"|| stc.WordingType == null).ToList();
            }
        }

        public List<SubjectToClauseWordingSetting> GetSubjectToClauseWordingsForTeamOffice(int? teamId, string officeId)
        {
            // TODO: Remove nullable from teamId ?
            // TODO: Where is the argument null reference check ?

            using (ConsoleRepository)
            {
                var team =
                    ConsoleRepository.Query<Team>(t => t.Id == teamId.Value, t => t.TeamOfficeSettings.Select(tos => tos.SubjectToClauseWordingSettings.Select(mws => mws.SubjectToClauseWording)), t => t.TeamOfficeSettings.Select(tos => tos.Office)).FirstOrDefault();
                if (team != null)
                {
                    var teamOfficeSetting = team.TeamOfficeSettings.FirstOrDefault(o => o.Office.Id == officeId);
                    if (teamOfficeSetting != null)
                        return teamOfficeSetting.SubjectToClauseWordingSettings.OrderBy(scs=>scs.DisplayOrder).ToList();
                }
            }

            return new List<SubjectToClauseWordingSetting>();
        }

        public string SaveSubjectToClauseWordingsForTeamOffice(TeamSubjectToClauseWordingsDto teamSubjectToClauseWordingsDto)
        {
            using (ConsoleRepository)
            {
                // Check does Team Market wordings exist
                var team = ConsoleRepository.Query<Team>(t => t.Id == teamSubjectToClauseWordingsDto.TeamId, t => t.TeamOfficeSettings.Select(tos => tos.Office), t => t.TeamOfficeSettings.Select(tos => tos.SubjectToClauseWordingSettings.Select(mws => mws.SubjectToClauseWording))).FirstOrDefault();

                bool teamOfficeSubjectToClauseWordingSettingsChanged = false;
                if (team != null)
                {
                    //<??> create office setting if one doesn't exist
                    if (team.TeamOfficeSettings == null ||
                        !team.TeamOfficeSettings.Any(tos => tos.Office.Id == teamSubjectToClauseWordingsDto.OfficeId))
                    {
                        team.TeamOfficeSettings = team.TeamOfficeSettings ?? new List<TeamOfficeSetting>();
                        team.TeamOfficeSettings.Add(new TeamOfficeSetting { Office = ConsoleRepository.Query<Office>(o => o.Id == teamSubjectToClauseWordingsDto.OfficeId).First(), SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>() });

                    }

                    var teamOfficeSetting =
                        team.TeamOfficeSettings.First(tos => tos.Office.Id == teamSubjectToClauseWordingsDto.OfficeId);
                    // Remove SubjectToClauseWording that need to be removed

                    var currentTeamOfficeSubjectToClauseWordings = teamOfficeSetting.SubjectToClauseWordingSettings.Select(mws => mws.SubjectToClauseWording.Id).ToList();
                    var teamOfficeSubjectToClauseWordingsToRemove =
                        currentTeamOfficeSubjectToClauseWordings.Except(teamSubjectToClauseWordingsDto.SubjectToClauseWordingSettingDtoList != null ? teamSubjectToClauseWordingsDto.SubjectToClauseWordingSettingDtoList.Select(tnos => tnos.Id) : new List<int>()).ToList();

                    foreach (var teamOfficeSubjectToClauseWordingId in teamOfficeSubjectToClauseWordingsToRemove)
                    {
                        var teamOfficeSubjectToClauseWordingToDelete =
                            ConsoleRepository.Query<SubjectToClauseWording>().FirstOrDefault(mw => mw.Id == teamOfficeSubjectToClauseWordingId);

                        if (teamOfficeSubjectToClauseWordingToDelete != null)
                        {
                            var teamOfficeSubjectToClauseWordingSettingsToDelete = teamOfficeSetting.SubjectToClauseWordingSettings.FirstOrDefault(mws => mws.SubjectToClauseWording.Equals(teamOfficeSubjectToClauseWordingToDelete));
                            //teamOfficeSetting.SubjectToClauseWordingSettings.Remove(teamOfficeSubjectToClauseWordingSettingsToDelete);
                            ConsoleRepository.Delete(teamOfficeSubjectToClauseWordingSettingsToDelete);
                            teamOfficeSubjectToClauseWordingSettingsChanged = true;
                        }
                    }

                    // Add SubjectToClauseWordings that need to be added
                    foreach (var subjectToClauseWordingSetting in teamSubjectToClauseWordingsDto.SubjectToClauseWordingSettingDtoList ?? new List<SubjectToClauseWordingSettingDto>())
                    {
                        if (teamOfficeSetting.SubjectToClauseWordingSettings.Any(mws => mws.SubjectToClauseWording.Id == subjectToClauseWordingSetting.Id)) continue;
                        var subjectToClauseWordingToAdd =
                            ConsoleRepository.Query<SubjectToClauseWording>().FirstOrDefault(a => a.Id == subjectToClauseWordingSetting.Id);

                        teamOfficeSetting.SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { SubjectToClauseWording = subjectToClauseWordingToAdd });
                        teamOfficeSubjectToClauseWordingSettingsChanged = true;
                    }

                    //Update display order
                    foreach (
                        var subjectToClauseWordingSettingDto in
                            teamSubjectToClauseWordingsDto.SubjectToClauseWordingSettingDtoList ?? new List<SubjectToClauseWordingSettingDto>())
                    {
                        var subjectToClauseWordingSetting = teamOfficeSetting.SubjectToClauseWordingSettings.First(
                            tncs => tncs.SubjectToClauseWording.Id == subjectToClauseWordingSettingDto.Id);
                        if (subjectToClauseWordingSetting.DisplayOrder != subjectToClauseWordingSettingDto.DisplayOrder ||
                            subjectToClauseWordingSetting.IsStrikeThrough !=
                            subjectToClauseWordingSettingDto.IsStrikeThrough)
                        {

                            subjectToClauseWordingSetting.DisplayOrder = subjectToClauseWordingSettingDto.DisplayOrder;
                            subjectToClauseWordingSetting.IsStrikeThrough =
                                subjectToClauseWordingSettingDto.IsStrikeThrough;
                            teamOfficeSubjectToClauseWordingSettingsChanged = true;
                        }

                    }

                    ConsoleRepository.SaveChanges();

                    if (teamOfficeSubjectToClauseWordingSettingsChanged)
                        return "Saved SubjectToClauseWording(s) for Team-Office";
                    else
                        return "SubjectToClauseWording(s) for Team-Office have not changed";
                }
                else
                {
                    return "Save SubjectToClauseWordings - Team does not Exist";
                }
            }
        }

        #endregion

        #region TermsNConditionWording

        public TermsNConditionWording CreateTermsNConditionWording(TermsNConditionWording termsNConditionWording)
        {
            using (ConsoleRepository)
            {
                ConsoleRepository.Add(termsNConditionWording);
                ConsoleRepository.SaveChanges();
                return termsNConditionWording;
            }
        }

        public TermsNConditionWording EditTermsNConditionWording(TermsNConditionWording termsNConditionWording)
        {
            using (var transactopnScope = new System.Transactions.TransactionScope(TransactionScopeOption.Required,
 new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            using (ConsoleRepository)
            {
                ConsoleRepository.Query<TermsNConditionWording>(mw => mw.Id == termsNConditionWording.Id).First().Title = termsNConditionWording.Title;
                // ConsoleRepository.Attach(termsNConditionWording);
                ConsoleRepository.SaveChanges();
                //Get the new version
                var newTermsNConditionWording =
                    ConsoleRepository.Query<TermsNConditionWording>(tnc => tnc.Key == termsNConditionWording.Key && tnc.IsObsolete == false)
                                     .First();
                var affectedTeamOfficeSettings =
                    ConsoleRepository.Query<TermsNConditionWordingSetting>(tnc => tnc.TermsNConditionWording.Key == termsNConditionWording.Key).ToList();
                foreach (var affectedTeamOfficeSetting in affectedTeamOfficeSettings)
                {
                    affectedTeamOfficeSetting.TermsNConditionWording = newTermsNConditionWording;
                }
                ConsoleRepository.SaveChanges();
                transactopnScope.Complete();
                return newTermsNConditionWording;
            }
        }

        public string DeleteTermsNConditionWording(TermsNConditionWording termsNConditionWording)
        {
            using (ConsoleRepository)
            {
                try
                {
                    ConsoleRepository.Attach(termsNConditionWording);
                    ConsoleRepository.Delete(termsNConditionWording);
                    ConsoleRepository.SaveChanges();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Failed";
                }
            }
        }

        public List<TermsNConditionWording> GetTermsNConditionWordings()
        {
            using (ConsoleRepository)
            {
                return ConsoleRepository.Query<TermsNConditionWording>(tnc => tnc.WordingType != "Custom" || tnc.WordingType == null).ToList();
            }
        }

        public List<TermsNConditionWording> GetTermsNConditionWordingsByPaging(string searchTerm, String sortCol, string sortDir, int skip,
                                                             int take, out Int32 count, out Int32 totalCount)
        {
            using (ConsoleRepository)
            {
                var filteredMarketWordingsQuery = ConsoleRepository.Query<TermsNConditionWording>(
                    tnc =>
                    tnc.WordingType != "Custom" ||
                    tnc.WordingType == null &&
                    (tnc.WordingRefNumber.Contains(searchTerm) || tnc.Title.Contains(searchTerm)));

                count = totalCount = filteredMarketWordingsQuery.Count();

                return String.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase)
                           ? filteredMarketWordingsQuery.OrderBy(sortCol)
                                                        .Skip(skip)
                                                        .Take(take)
                                                        .ToList()
                           : filteredMarketWordingsQuery.OrderByDescending(sortCol)
                                                        .Skip(skip)
                                                        .Take(take)
                                                        .ToList();
            }
        }
        public List<TermsNConditionWordingSetting> GetTermsNConditionWordingsForTeamOffice(int? teamId, string officeId)
        {
            // TODO: Remove nullable from teamId ?
            // TODO: Where is the argument null reference check ?

            using (ConsoleRepository)
            {
                var team =
                    ConsoleRepository.Query<Team>(t => t.Id == teamId.Value, t => t.TeamOfficeSettings.Select(tos => tos.TermsNConditionWordingSettings.Select(mws => mws.TermsNConditionWording)), t => t.TeamOfficeSettings.Select(tos => tos.Office)).FirstOrDefault();
                if (team != null)
                {
                    var teamOfficeSetting = team.TeamOfficeSettings.FirstOrDefault(o => o.Office.Id == officeId);
                    if (teamOfficeSetting != null)
                        return teamOfficeSetting.TermsNConditionWordingSettings.OrderBy(tncs=>tncs.DisplayOrder).ToList();
                }
            }

            return new List<TermsNConditionWordingSetting>();
        }

        public string SaveTermsNConditionWordingsForTeamOffice(TeamTermsNConditionWordingsDto teamTermsNConditionWordingsDto)
        {
            using (ConsoleRepository)
            {
                // Check does Team Market wordings exist
                var team = ConsoleRepository.Query<Team>(t => t.Id == teamTermsNConditionWordingsDto.TeamId, t => t.TeamOfficeSettings.Select(tos => tos.Office), t => t.TeamOfficeSettings.Select(tos => tos.TermsNConditionWordingSettings.Select(mws => mws.TermsNConditionWording))).FirstOrDefault();

                bool teamOfficeTermsNConditionWordingSettingsChanged = false;
                if (team != null)
                {
                    //<??> create office setting if one doesn't exist
                    if (team.TeamOfficeSettings == null ||
                        !team.TeamOfficeSettings.Any(tos => tos.Office.Id == teamTermsNConditionWordingsDto.OfficeId))
                    {
                        team.TeamOfficeSettings = team.TeamOfficeSettings ?? new List<TeamOfficeSetting>();
                        team.TeamOfficeSettings.Add(new TeamOfficeSetting { Office = ConsoleRepository.Query<Office>(o => o.Id == teamTermsNConditionWordingsDto.OfficeId).First(), TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>() });

                    }

                    var teamOfficeSetting =
                        team.TeamOfficeSettings.First(tos => tos.Office.Id == teamTermsNConditionWordingsDto.OfficeId);
                    // Remove TermsNConditionWording that need to be removed
                    
                    var currentTeamOfficeTermsNConditionWordings = teamOfficeSetting.TermsNConditionWordingSettings.Select(mws => mws.TermsNConditionWording.Id).ToList();
                    var teamOfficeTermsNConditionWordingsToRemove =
                        currentTeamOfficeTermsNConditionWordings.Except(teamTermsNConditionWordingsDto.TermsNConditionWordingSettingDtoList!=null?teamTermsNConditionWordingsDto.TermsNConditionWordingSettingDtoList.Select(tnos=>tnos.Id):new List<int>()).ToList();

                    foreach (var teamOfficeTermsNConditionWordingId in teamOfficeTermsNConditionWordingsToRemove)
                    {
                        var teamOfficeTermsNConditionWordingToDelete =
                            ConsoleRepository.Query<TermsNConditionWording>().FirstOrDefault(mw => mw.Id == teamOfficeTermsNConditionWordingId);

                        if (teamOfficeTermsNConditionWordingToDelete != null)
                        {
                            var teamOfficeTermsNConditionWordingSettingsToDelete = teamOfficeSetting.TermsNConditionWordingSettings.FirstOrDefault(mws => mws.TermsNConditionWording.Equals(teamOfficeTermsNConditionWordingToDelete));
                            //teamOfficeSetting.TermsNConditionWordingSettings.Remove(teamOfficeTermsNConditionWordingSettingsToDelete);
                            ConsoleRepository.Delete(teamOfficeTermsNConditionWordingSettingsToDelete);
                            teamOfficeTermsNConditionWordingSettingsChanged = true;
                        }
                    }

                    // Add TermsNConditionWordings that need to be added
                    foreach (var termsNConditionWordingSetting in teamTermsNConditionWordingsDto.TermsNConditionWordingSettingDtoList ?? new List<TermsNConditionWordingSettingDto>())
                    {
                        if (teamOfficeSetting.TermsNConditionWordingSettings.Any(mws => mws.TermsNConditionWording.Id == termsNConditionWordingSetting.Id)) continue;
                        var termsNConditionWordingToAdd =
                            ConsoleRepository.Query<TermsNConditionWording>().FirstOrDefault(a => a.Id == termsNConditionWordingSetting.Id);

                        teamOfficeSetting.TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { TermsNConditionWording = termsNConditionWordingToAdd });
                        teamOfficeTermsNConditionWordingSettingsChanged = true;
                    }

                    //Update display order
                    foreach (
                        var termsNConditionWordingSettingDto in
                            teamTermsNConditionWordingsDto.TermsNConditionWordingSettingDtoList ?? new List<TermsNConditionWordingSettingDto>())
                    {
                        var termsNConditionWordingSetting = teamOfficeSetting.TermsNConditionWordingSettings.First(
                            tncs => tncs.TermsNConditionWording.Id == termsNConditionWordingSettingDto.Id);
                        if (termsNConditionWordingSetting.DisplayOrder != termsNConditionWordingSettingDto.DisplayOrder ||
                            termsNConditionWordingSetting.IsStrikeThrough !=
                            termsNConditionWordingSettingDto.IsStrikeThrough)
                        {
                            
                            termsNConditionWordingSetting.DisplayOrder = termsNConditionWordingSettingDto.DisplayOrder;
                            termsNConditionWordingSetting.IsStrikeThrough =
                                termsNConditionWordingSettingDto.IsStrikeThrough;
                            teamOfficeTermsNConditionWordingSettingsChanged = true;
                    }

                    }

                    ConsoleRepository.SaveChanges();

                    if (teamOfficeTermsNConditionWordingSettingsChanged)
                        return "Saved TermsNConditionWording(s) for Team-Office";
                    else
                        return "TermsNConditionWording(s) for Team-Office have not changed";
                }
                else
                {
                    return "Save TermsNConditionWordings - Team does not Exist";
                }
            }
        }
        
        #endregion

        #region Helpers

        public void AddTeamFilters(IEnumerable<int> teamIds, User user, Team team)
        {
            if (teamIds == null) return;

            bool teamPassedIn = team != null;

            foreach (var teamId in teamIds)
            {

                if (!teamPassedIn) // Has a new team been passed in?

                    team = ConsoleRepository.Query<Team>(t => t.RelatedCOBs, t => t.RelatedOffices)
                                            .FirstOrDefault(t => t.Id == teamId);

                if (team == null) continue; // Cannot find team in DB

                var relatedCOBs = team.RelatedCOBs;
                if (relatedCOBs != null && relatedCOBs.Count > 0)
                {
                    var cobsToAdd = (user.FilterCOBs != null && user.FilterCOBs.Count > 0)
                                        ? relatedCOBs.Except(user.FilterCOBs)
                                        : relatedCOBs;

                    if (user.FilterCOBs == null)
                        user.FilterCOBs = new Collection<COB>();
                    foreach (var cob in cobsToAdd)
                    {
                        user.FilterCOBs.Add(cob);
                    }
                }

                var relatedOffices = team.RelatedOffices;
                if (relatedOffices == null || relatedOffices.Count <= 0) continue;

                var officesToAdd = (user.FilterOffices != null && user.FilterOffices.Count > 0)
                                       ? relatedOffices.Except(user.FilterOffices)
                                       : relatedOffices;

                if (user.FilterOffices == null)
                    user.FilterOffices = new Collection<Office>();
                foreach (var office in officesToAdd)
                {
                    user.FilterOffices.Add(office);
                }


            }
        }

        private static void SetUserDto(UserDto userDto, User user, IConsoleRepository _consoleRepository)
        {
            userDto.Id = user.Id;
            userDto.DomainLogon = user.DomainLogon;
            userDto.IsActive = user.IsActive;

            // Currently Selected Values
            userDto.DefaultOrigOffice = (user.DefaultOrigOffice != null)
                                            ? new OfficeDto
                                                {
                                                    Id = user.DefaultOrigOffice.Id,
                                                    Title = user.DefaultOrigOffice.Name
                                                }
                                            : null;
            userDto.PrimaryOffice = (user.PrimaryOffice != null)
                                        ? new OfficeDto { Id = user.PrimaryOffice.Id, Title = user.PrimaryOffice.Name }
                                        : null;
            userDto.DefaultUW = (user.DefaultUW != null)
                                    ? new UserDto { Id = user.DefaultUW.Id, DomainLogon = user.DefaultUW.DomainLogon }
                                    : null;
            userDto.UnderwriterId = user.UnderwriterCode;

            OfficeDto officeDto = null;
            var officeList = _consoleRepository.Query<Office>().ToList();
            if (user.DefaultOrigOffice != null)
            {
                var defaultOrigOfficeList = new List<OfficeDto>();
                defaultOrigOfficeList.Add(userDto.DefaultOrigOffice);

                defaultOrigOfficeList.AddRange(officeList.Select(o =>
                    {
                        if (o.Id != userDto.DefaultOrigOffice.Id)
                            officeDto = new OfficeDto { Id = o.Id, Title = o.Name };
                        return officeDto;
                    }));
                userDto.DefaultOrigOfficeList = defaultOrigOfficeList;
            }

            if (user.PrimaryOffice != null)
            {
                var primaryOfficeList = new List<OfficeDto>();
                primaryOfficeList.Add(userDto.PrimaryOffice);

                primaryOfficeList.AddRange(
                    officeList.Where(o => o.Id != userDto.PrimaryOffice.Id)
                              .Select(o => new OfficeDto() { Id = o.Id, Title = o.Name }));

                userDto.PrimaryOfficeList = primaryOfficeList;
            }

            if (userDto.DefaultOrigOfficeList == null || userDto.PrimaryOfficeList == null)
            {
                var officeDtoList = officeList.ToList().Select(o =>
                    {
                        return new OfficeDto { Id = o.Id, Title = o.Name };
                    }).ToList();

                if (userDto.DefaultOrigOfficeList == null)
                    userDto.DefaultOrigOfficeList = officeDtoList;

                if (userDto.PrimaryOfficeList == null)
                    userDto.PrimaryOfficeList = officeDtoList;
            }

            if (userDto.DefaultUW != null)
            {
                var defaultUWDtoList = new List<UserDto>();
                defaultUWDtoList.Add(userDto.DefaultUW);

                UserDto defaultUW = null;
                defaultUWDtoList.AddRange(_consoleRepository.Query<User>().ToList().Select(u =>
                    {
                        if (u.Id != userDto.DefaultUW.Id)
                            defaultUW = new UserDto { Id = u.Id, DomainLogon = u.DomainLogon };
                        return defaultUW;
                    }).ToList());

                userDto.DefaultUWList = defaultUWDtoList;
            }

            if (userDto.DefaultUW == null)
            {
                var defaultUWDtoList = _consoleRepository.Query<User>().ToList().Select(u =>
                    {
                        return new UserDto { Id = u.Id, DomainLogon = u.DomainLogon };
                    }).ToList();
                userDto.DefaultUWList = defaultUWDtoList;
            }


            userDto.TeamMemberships = (user.TeamMemberships != null && user.TeamMemberships.Any())
                                          ? user.TeamMemberships.Where(m => m.IsCurrent).ToList().ConvertAll(tm =>
                new TeamMembershipDto
                {
                    TeamId = tm.TeamId,
                    EndDate = tm.EndDate,
                    Id = tm.Id,
                    PrimaryTeamMembership = tm.PrimaryTeamMembership,
                    IsCurrent = tm.IsCurrent,
                    StartDate = tm.StartDate,
                    Team = new TeamDto
                    {
                        Id = tm.Team.Id,
                        Title = tm.Team.Title,
                        DefaultDomicile = tm.Team.DefaultDomicile,
                        DefaultMOA = tm.Team.DefaultMOA,
                        PricingActuary = tm.Team.PricingActuary,
                        QuoteExpiryDaysDefault = tm.Team.QuoteExpiryDaysDefault,
                        DefaultPolicyType = tm.Team.DefaultPolicyType
                    }
                }) : null;

            userDto.AdditionalUsers = (user.AdditionalUsers != null && user.AdditionalUsers.Any())
                                          ? user.AdditionalUsers.ToList()
                                                .ConvertAll(ad => new UserDto { Id = ad.Id, DomainLogon = ad.DomainLogon })
                                          : null;
            userDto.AdditionalCOBs = (user.AdditionalCOBs != null && user.AdditionalCOBs.Any())
                                         ? user.AdditionalCOBs.ToList()
                                               .ConvertAll(cob => new COBDto { Id = cob.Id, Narrative = cob.Narrative })
                                         : null;
            userDto.FilterCOBs = (user.FilterCOBs != null && user.FilterCOBs.Any())
                                     ? user.FilterCOBs.ToList()
                                           .ConvertAll(cob => new COBDto { Id = cob.Id, Narrative = cob.Narrative })
                                     : null;
            userDto.AdditionalOffices = (user.AdditionalOffices != null && user.AdditionalOffices.Any())
                                            ? user.AdditionalOffices.ToList()
                                                  .ConvertAll(o => new OfficeDto { Id = o.Id, Title = o.Name })
                                            : null;
            userDto.FilterMembers = (user.FilterMembers != null && user.FilterMembers.Any())
                                        ? user.FilterMembers.ToList()
                                              .ConvertAll(fm => new UserDto { Id = fm.Id, DomainLogon = fm.DomainLogon })
                                        : null;
            userDto.FilterOffices = (user.FilterOffices != null && user.FilterOffices.Any())
                                        ? user.FilterOffices.ToList()
                                              .ConvertAll(fo => new OfficeDto { Id = fo.Id, Title = fo.Name })
                                        : null;

            var requiredData = GetRequiredDataEditUser(user.DomainLogon, _consoleRepository);

            userDto.AllAdditionalUsers = requiredData.AllAdditionalUsers;
            userDto.AllAdditionalCOBs = requiredData.AllAdditionalCOBs;
            userDto.AllAdditionalOffices = requiredData.AllAdditionalOffices;
            userDto.AllFilterCOBs = requiredData.AllFilterCOBs;
            userDto.AllFilterMembers = requiredData.AllFilterMembers;
            userDto.AllFilterOffices = requiredData.AllFilterOffices;
            userDto.AllTeamMemberships = requiredData.AllTeamMemberships;
        }

        #endregion


      
    }

}