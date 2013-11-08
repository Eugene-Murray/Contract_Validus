using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.LogHandling;
using Validus.Core.MVC;
using Validus.Models;
using System.Linq;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead")]
    public class UserController : Controller
    {
        public readonly IWebSiteModuleManager _websiteModuleManager;
        public readonly ILogHandler _logHandler;

        public UserController(IWebSiteModuleManager websiteModuleManager, ILogHandler logHandler)
        {
            this._websiteModuleManager = websiteModuleManager;
            this._logHandler = logHandler;
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult Current()
        {
            User u = _websiteModuleManager.EnsureCurrentUser();
            return new JsonNetResult() { Data = u };
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult Settings()
        {
            User u = _websiteModuleManager.EnsureCurrentUser();

            List<String> l = new List<String>();
            if (u.TeamMemberships != null)
            {
                foreach (var tm in u.TeamMemberships)
                {
                    if (!String.IsNullOrEmpty(tm.Team.SubmissionTypeId))
                        l.Add(tm.Team.SubmissionTypeId);
                }
            }

            return new JsonNetResult() { Data = l };
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult MySettings()
        {
            User u = _websiteModuleManager.EnsureCurrentUser();

            return new JsonNetResult() { Data = new { 
                FilterCOBs = u.FilterCOBs.Select(fc => fc.Id),
                FilterOffices = u.FilterOffices.Select(fc => fc.Id),
                FilterMembers = u.FilterMembers.Select(fc => fc.UnderwriterCode).Where(a => !String.IsNullOrEmpty(a)),
                AdditionalCOBs = u.AdditionalCOBs.Select(fc => fc.Id),
                AdditionalOffices = u.AdditionalOffices.Select(fc => fc.Id),
                AdditionalMembers = u.AdditionalUsers.Select(fc => fc.UnderwriterCode).Where(a => !String.IsNullOrEmpty(a)),
                                                    } 
                                        };
        }
    }
}
