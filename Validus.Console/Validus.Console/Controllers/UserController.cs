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
    }
}
