using System;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;

namespace Validus.Console.Controllers
{
	[Authorize(Roles = @"ConsoleRead")]
    public class HomeController : Controller
    {
        private readonly IWebSiteModuleManager _webSiteModuleManager;

        public HomeController(IWebSiteModuleManager webSiteModuleManager)
        {
            this._webSiteModuleManager = webSiteModuleManager;
        }

        public ActionResult Index()
        {
            var user = this._webSiteModuleManager.EnsureCurrentUser();
            this.ViewBag.OpenTabs = user.OpenTabs;
            this.ViewBag.TeamMemberships = user.TeamMemberships;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            this._webSiteModuleManager.Dispose();

            base.Dispose(disposing);
        }
    }
}