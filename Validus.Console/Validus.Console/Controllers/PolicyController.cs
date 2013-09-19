using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Core.MVC;
using Validus.Models;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead")]
    public class PolicyController : Controller
    {
        private readonly IPolicyBusinessModule _bm;
        private readonly IWebSiteModuleManager _webSiteModuleManager;

        public PolicyController(IPolicyBusinessModule bm, IWebSiteModuleManager webSiteModuleManager)
        {
			this._bm = bm;
            _webSiteModuleManager = webSiteModuleManager;
        }

        //
        // GET: /Policy/_IndexJSON
        public ActionResult _RenewalIndexJSON(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0, 
            DateTime? expiryStartDate, DateTime? expiryEndDate, Boolean applyProfileFilters = true, 
            Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            Int32 iTotalRecords;
            Int32 iTotalDisplayRecords;
            String sortCol;

            switch (iSortCol_0)
            {
                case 0:
                    sortCol = "Underwriter";                    
                    break;
                case 1:
                    sortCol = "InsuredName";
                    break;
                case 2:
                    sortCol = "PolicyId";
                    break;
                case 3:
                default:
                    sortCol = "ExpiryDate";
                    break;
            }

			var aaData = this._bm.GetRenewalPolicies(expiryStartDate.HasValue ? expiryStartDate.Value : DateTime.Today.AddDays(-7), 
                expiryEndDate.HasValue ? expiryEndDate.Value : DateTime.Today.AddDays(30), 
                sSearch, sortCol, sSortDir_0, 
                iDisplayStart, iDisplayLength, 
                applyProfileFilters, out iTotalDisplayRecords, out iTotalRecords);

            return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
        }

		// TODO: This code needs sorting out...
		//
		// GET: /Policy/_IndexJSON
	    public ActionResult _RenewalIndexJSONDetailed(string sSearch, int sEcho, int iSortCol_0, string sSortDir_0,
	                                                  DateTime? expiryStartDate, DateTime? expiryEndDate,
	                                                  bool applyProfileFilters = true, int iDisplayLength = 10, 
													  int iDisplayStart = 0)
	    {
			var iTotalDisplayRecords = 0;
			var iTotalRecords = 0;
		    var sortColumns = new[]
		    {
			    "Underwriter", "InsuredName", "PolicyId", "InceptionDate", "ExpiryDate", "COB", 
				"OriginatingOffice", "Leader", "Broker"
		    };

		    var aaData = this._bm.GetRenewalPoliciesDetailed(
			    expiryStartDate.HasValue
				    ? expiryStartDate.Value
				    : DateTime.Today.AddDays(-7),
			    expiryEndDate.HasValue
				    ? expiryEndDate.Value
				    : DateTime.Today.AddDays(30),
			    sSearch,
			    sortColumns[iSortCol_0],
			    sSortDir_0,
			    iDisplayStart,
			    iDisplayLength,
			    applyProfileFilters,
				out iTotalDisplayRecords,
				out iTotalRecords);

		    return Json(new 
			{ 
				sEcho, 
				iTotalRecords, 
				iTotalDisplayRecords,
				aaData
			}, JsonRequestBehavior.AllowGet);
	    }

	    //
        // GET: /Policy/_RenewalIndexDetailed
        public ActionResult _RenewalIndexDetailed()
        {
            return PartialView();
        }

		//
		// GET: /Policy/_RenewalPreview?PolId=
        public ActionResult _RenewalPreview(String PolId)
        {
            var user = _webSiteModuleManager.EnsureCurrentUser();
            this.ViewBag.TeamMemberships = user.TeamMemberships.Where(t => t.Team.SubmissionTypeId != null).ToArray();

            var r = this._bm.GetRenewalPolicyDetailsByPolId(PolId);
            return PartialView(r);
        }

        public ActionResult _RenewalPolicyDetailed(String Id)
        {
			var r = this._bm.GetRenewalPolicyDetailsByPolId(Id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _PolicyDetailed(String Id)
        {
            var r = this._bm.GetPolicyDetailsByPolId(Id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
    }
}