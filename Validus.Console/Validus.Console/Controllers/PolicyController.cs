﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.LogHandling;
using Validus.Core.MVC;
using Validus.Models;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead")]
    public class PolicyController : Controller
    {
        private readonly IPolicyBusinessModule _policyBusinessModule;
        private readonly IWebSiteModuleManager _webSiteModuleManager;
        private readonly ILogHandler _logHandler;

        public PolicyController(IPolicyBusinessModule bm, IWebSiteModuleManager webSiteModuleManager, ILogHandler logHandler)
        {
			this._policyBusinessModule = bm;
            _webSiteModuleManager = webSiteModuleManager;
            _logHandler = logHandler;
        }

        //
        // GET: /Policy/_IndexJSON
        public ActionResult _RenewalIndexJSON(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0,
            DateTime? expiryStartDate, DateTime? expiryEndDate, String[] extraFilters, Boolean applyProfileFilters = true, 
            Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {

            List<Tuple<String, String, String>> filters = new List<Tuple<string, string, string>>();
            if (extraFilters != null)
            {
                //  TODO: use linq
                foreach (String extraFilter in extraFilters)
                {
                    String[] filter = extraFilter.Split(new Char[] { ':' });
                    filters.Add(new Tuple<String, String, String>(filter[0], filter[1], filter[2]));
                }
            }

            Int32 iTotalRecords;
            Int32 iTotalDisplayRecords;
            String sortCol = this.Request[String.Format("mDataProp_{0}", iSortCol_0)];

			var aaData = this._policyBusinessModule.GetRenewalPolicies(expiryStartDate.HasValue ? expiryStartDate.Value : DateTime.Today.AddDays(-7), 
                expiryEndDate.HasValue ? expiryEndDate.Value : DateTime.Today.AddDays(30), 
                sSearch, sortCol, sSortDir_0, 
                iDisplayStart, iDisplayLength, 
                applyProfileFilters, out iTotalDisplayRecords, out iTotalRecords, filters);

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
            String sortCol = this.Request[String.Format("mDataProp_{0}", iSortCol_0)];

		    var aaData = this._policyBusinessModule.GetRenewalPoliciesDetailed(
			    expiryStartDate.HasValue
				    ? expiryStartDate.Value
				    : DateTime.Today.AddDays(-7),
			    expiryEndDate.HasValue
				    ? expiryEndDate.Value
				    : DateTime.Today.AddDays(30),
			    sSearch,
			    sortCol,
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

        public ActionResult _RenewalPreview(string polId)
        {
            var user = _webSiteModuleManager.EnsureCurrentUser();
            this.ViewBag.TeamMemberships = user.TeamMemberships.Where(t => t.Team.SubmissionTypeId != null).ToArray();

            var r = this._policyBusinessModule.GetRenewalPolicyDetailsByPolId(polId);
            return PartialView(r);
        }

        public async Task<ActionResult> _RiskPreview(string polId)
        {
            Task<RiskPreviewDto> riskPreviewDto = this._policyBusinessModule.RiskPreview(polId);
            var response = await riskPreviewDto;
            return PartialView(response);
        }

        public ActionResult _RenewalPolicyDetailed(String Id)
        {
			var r = this._policyBusinessModule.GetRenewalPolicyDetailsByPolId(Id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _PolicyDetailed(String Id)
        {
            var r = this._policyBusinessModule.GetPolicyDetailsByPolId(Id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

    }
}