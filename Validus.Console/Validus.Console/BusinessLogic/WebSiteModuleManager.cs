using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Console.Data;
using Validus.Core.HttpContext;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public class WebSiteModuleManager : IWebSiteModuleManager
    {
        private readonly ICurrentHttpContext _currentHttpContext;
        private readonly IConsoleRepository _repository;

        public WebSiteModuleManager(IConsoleRepository repository, ICurrentHttpContext currentHttpContext)
        {
            _repository = repository;
            _currentHttpContext = currentHttpContext;   
        }

        public User EnsureCurrentUser()
        {
            if (_currentHttpContext == null)
                throw new Exception("_currentHttpContext is null");

            var u = (_currentHttpContext.Context.Session != null)
                        ? _currentHttpContext.Context.Session["User"] as User
                        : null;

            if (u == null)
            {
                if (_repository.Query<User>().Any(w => (w.DomainLogon.ToLower() == _currentHttpContext.CurrentUser.Identity.Name.ToLower() && w.IsActive == true)))
                {
                    u = _repository.Query<User>(
                                            us => us.FilterCOBs,
                                            us => us.FilterOffices,
                                            us => us.FilterMembers,
                                            us => us.AdditionalCOBs,
                                            us => us.AdditionalOffices,
                                            us => us.AdditionalUsers,
                                            us => us.OpenTabs,
                                            us => us.TeamMemberships.Select(tm => tm.Team.RelatedRisks),
                                            us => us.DefaultOrigOffice,
                                            us => us.DefaultUW,
                                            us => us.Underwriter,
                                            us => us.DefaultUW.Underwriter
                                    ).SingleOrDefault(w => (w.DomainLogon == _currentHttpContext.CurrentUser.Identity.Name && w.IsActive == true));

                    if (u.TeamMemberships != null)
                        u.TeamMemberships = u.TeamMemberships.Where(tm => tm.IsCurrent).ToList();
                }

                if (u == null)
                {
                    u = new User { DomainLogon = _currentHttpContext.CurrentUser.Identity.Name };
                    u = _repository.Add(u);
                    _repository.SaveChanges();
                }

                if (_currentHttpContext.Context.Session != null)
                    _currentHttpContext.Context.Session["User"] = u;
            }

            return u;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}