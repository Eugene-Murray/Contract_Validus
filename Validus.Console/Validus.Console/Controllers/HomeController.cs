using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.Properties;
using System.Linq;

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
            
            //  TODO: Improve efficiency
            var sc = Properties.Settings.Default.ActivityNameFilterOptions.Cast<String>()
                .Select(s => new SelectListItem() { Text=s, Value=s}).ToList();
            sc.Insert(0, new SelectListItem() { Value = "", Text = "(All Activities)" });
            this.ViewBag.ActivityNames = new SelectList(sc, "Value", "Text");

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            this._webSiteModuleManager.Dispose();

            base.Dispose(disposing);
        }
    }
}