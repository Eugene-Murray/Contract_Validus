using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.MVC;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead, ConsoleUW"), MvcModelStateFilter]
    public class SubmissionController : Controller
    {
        public ISubmissionModule SubmissionModule { get; protected set; }
        public IWebSiteModuleManager WebSiteModuleManager { get; protected set; }
        public IAuditTrailModule AuditTrailModule { get; protected set; }

	    public SubmissionController(ISubmissionModule submissionModule,
	                                IWebSiteModuleManager webSiteModuleManager,
	                                IAuditTrailModule auditTrailModule)
	    {
		    this.SubmissionModule = submissionModule;
		    this.WebSiteModuleManager = webSiteModuleManager;
		    this.AuditTrailModule = auditTrailModule;
	    }
       
	    private void SetupTemplate(string typeId = null)
	    {
		    var user = this.WebSiteModuleManager.EnsureCurrentUser();

		    if (user != null)
		    {
			    this.ViewBag.DefaultOffice = (user.DefaultOrigOffice != null) ? user.DefaultOrigOffice.Id : string.Empty;

			    if (user.TeamMemberships != null)
			    {
				    var membership = user.TeamMemberships
				                         .FirstOrDefault(tm => tm.IsCurrent &&
				                                               tm.Team.SubmissionTypeId == typeId);

				    if (membership != null && membership.Team != null)
				    {
				        this.ViewBag.DefaultQuoteExpiry = membership.Team.QuoteExpiryDaysDefault.ToString();
				        this.ViewBag.DefaultPolicyType = membership.Team.DefaultPolicyType;
				    }
			    }

			    var underwriter = (user.DefaultUW != null)
				                      ? user.DefaultUW.Underwriter ?? user.Underwriter
				                      : user.Underwriter;

			    this.ViewBag.DefaultUnderwriter = (underwriter != null) ? underwriter.Code : string.Empty;
				this.ViewBag.DefaultNonLondonBroker = user.NonLondonBroker ?? string.Empty;
		    }
	    }

		#region Base Templates

		// TODO: public PartialViewResult _Template()
		// TODO: public PartialViewResult _OptionTemplate()
		// TODO: public PartialViewResult _OptionVersionTemplate()
		// TODO: public PartialViewResult _QuoteTemplate()

		// GET: /Submission/_Preview/5
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _Preview(int id)
		{
			var submission = this.SubmissionModule.GetSubmissionPreviewById(id);

			if (submission == null)
			{
				this.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}

			return this.PartialView("_Preview", submission);
		}

		// GET: /Submission/_QuoteComparisonTable
		public PartialViewResult _QuoteComparisonTable()
		{
			return this.PartialView();
		}

		// GET: /Submission/_SubmissionIndexDetailed
		public PartialViewResult _SubmissionIndexDetailed()
		{
			return PartialView();
		}

		#endregion

		#region CA Templates

		// GET: /Submission/_TemplateCA
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _TemplateCA()
		{
			this.SetupTemplate("CA");

			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplateCA
		[HttpGet]
		public PartialViewResult _OptionTemplateCA()
		{
			return this.PartialView();
		}

		// GET: /Submission/_OptionVersionTemplateCA
		[HttpGet]
		public PartialViewResult _OptionVersionTemplateCA()
		{
			return this.PartialView();
		}

		// GET: /Submission/_QuoteTemplateCA
		[HttpGet]
		public PartialViewResult _QuoteTemplateCA()
		{
			return this.PartialView();
		}

		#endregion

		#region EN Templates

		// GET: /Submission/_TemplateEN
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _TemplateEN()
		{
			this.SetupTemplate("EN");

			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplateEN
		[HttpGet]
		public PartialViewResult _OptionTemplateEN()
		{
			return this.PartialView();
		}

		// GET: /Submission/_QuoteTemplateEN
		[HttpGet]
		public PartialViewResult _QuoteTemplateEN()
		{
			return this.PartialView();
		}

		#endregion

		#region FI Templates

		// GET: /Submission/_TemplatePV
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _TemplateFI()
		{
			this.SetupTemplate("FI");

			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplateFI
		[HttpGet]
		public PartialViewResult _OptionTemplateFI()
		{
			return this.PartialView();
		}

		// GET: /Submission/_QuoteTemplateFI
		[HttpGet]
		public PartialViewResult _QuoteTemplateFI()
		{
			return this.PartialView();
		}

		#endregion

		#region HM Templates

		// GET: /Submission/_TemplateHM
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _TemplateHM()
		{
			this.SetupTemplate("HM");

			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplateHM
		[HttpGet]
		public PartialViewResult _OptionTemplateHM()
		{
			return this.PartialView();
		}

		// GET: /Submission/_OptionVersionTemplateHM
		[HttpGet]
		public PartialViewResult _OptionVersionTemplateHM()
		{
			return this.PartialView();
		}

		// GET: /Submission/_QuoteTemplateHM
		[HttpGet]
		public PartialViewResult _QuoteTemplateHM(string typeId = null)
		{
			return this.PartialView();
		}

		#endregion

		#region ME Templates

		// GET: /Submission/_TemplateME
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _TemplateME()
		{
			this.SetupTemplate("ME");

			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplateME
		[HttpGet]
		public PartialViewResult _OptionTemplateME()
		{
			return this.PartialView();
		}

		// GET: /Submission/_QuoteTemplateME
		[HttpGet]
		public PartialViewResult _QuoteTemplateME()
		{
			return this.PartialView();
		}

		#endregion

		#region PV Templates

		// GET: /Submission/_TemplatePV
		[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
		public PartialViewResult _TemplatePV()
		{
			this.SetupTemplate("PV");

			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplatePV
		[HttpGet]
		public PartialViewResult _OptionTemplatePV()
		{
			return this.PartialView();
		}

		// GET: /Submission/_OptionVersionTemplatePV
		[HttpGet]
		public PartialViewResult _OptionVersionTemplatePV()
		{
			return this.PartialView();
		}

		// GET: /Submission/_OptionTemplatePV
		[HttpGet]
		public PartialViewResult _QuoteTemplatePV()
		{
			return this.PartialView();
		}

		#endregion

        #region Wordings

        // GET: /Submission/_MarketWordingTemplate
        [HttpGet]
		public PartialViewResult _CreateBrokerContact()
        {
            return this.PartialView();
        }

		// GET: /Submission/_CrossSellingCheck
		[HttpGet, OutputCache(CacheProfile = "NoCacheProfile")]
		public PartialViewResult _CrossSellingCheck(string insuredName, int submissionId)
        {
			return this.PartialView(!string.IsNullOrEmpty(insuredName)
				? this.SubmissionModule.CrossSellingCheck(Uri.UnescapeDataString(insuredName), submissionId)
				: new List<CrossSellingCheckDto>());
        }

		// GET: /Submission/_MarketWordingTemplate
		public PartialViewResult _MarketWordingTemplate()
        {
            return this.PartialView();
        }

		// GET: /Submission/_SubjectToClauseWordingTemplate
        [HttpGet]
        public PartialViewResult _SubjectToClauseWordingTemplate()
        {
			return this.PartialView();
        }

		// GET: /Submission/_SubmissionSideBar
        public PartialViewResult _SubmissionSideBar()
        {
			return this.PartialView();
        }

		// GET: /Submission/_TermsNConditionTemplate
        [HttpGet]
        public PartialViewResult _TermsNConditionTemplate()
        {
            return this.PartialView();
        }

		// GET: /Submission/_WorldCheckAuditTrailForSubmission
	    [Authorize(Roles = @"ConsoleUW"), OutputCache(CacheProfile = "NoCacheProfile")]
		public PartialViewResult _WorldCheckAuditTrailForSubmission(int id)
        {
			return this.PartialView(this.AuditTrailModule.GetAuditTrails("Submission", id.ToString()));
        }

        public PartialViewResult _SaveQuoteSheetButtons()
        {
            return this.PartialView();
        }

		#endregion

		#region jQuery Data-Tables Actions

		//
		// GET: /Submission/_IndexJSON
		[HttpGet]
		public ActionResult _IndexJSON(string sSearch, int sEcho, int iSortCol_0, string sSortDir_0, String[] extraFilters,
									   bool applyProfileFilters = true, int iDisplayLength = 10, int iDisplayStart = 0)
		{
            List<Tuple<String, String, String>> filters = new List<Tuple<string,string,string>>();
            if (extraFilters != null)
            {
                //  TODO: use linq
                foreach (String extraFilter in extraFilters)
                {
                    String[] filter = extraFilter.Split(new Char[] { ':' });
                    filters.Add(new Tuple<String, String, String>(filter[0], filter[1], filter[2]));
                }
            }
                
			var iTotalRecords = default(int);
			var iTotalDisplayRecords = default(int);
			var sortCol = string.Empty;

			sortCol = this.Request[String.Format("mDataProp_{0}", iSortCol_0)];

			var aaData = this.SubmissionModule.GetSubmissions(sSearch, iDisplayStart, iDisplayLength,
			                                                  sortCol, sSortDir_0, applyProfileFilters, out iTotalDisplayRecords,
			                                                  out iTotalRecords, filters);

			// TODO: Change to JsonNetResult
			return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Submission/_IndexJSONDetailed
	    [HttpGet]
		public ActionResult _IndexJSONDetailed(string sSearch, int sEcho, int iSortCol_0, string sSortDir_0,
											   bool applyProfileFilters = true, int iDisplayLength = 10,
											   int iDisplayStart = 0)
	    {
		    var iTotalRecords = default(int);
		    var iTotalDisplayRecords = default(int);
		    var sortCol = string.Empty;

		    sortCol = this.Request[String.Format("mDataProp_{0}", iSortCol_0)];

		    var aaData = this.SubmissionModule.GetSubmissionsDetailed(sSearch, iDisplayStart, iDisplayLength,
		                                                              sortCol, sSortDir_0, applyProfileFilters,
		                                                              out iTotalDisplayRecords,
		                                                              out iTotalRecords);

		    // TODO: Change to JsonNetResult
		    return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
	    }

	    #endregion

        protected override void Dispose(bool disposing)
        {
            this.SubmissionModule.Dispose();
			this.WebSiteModuleManager.Dispose();

            base.Dispose(disposing);
        }
    }
}