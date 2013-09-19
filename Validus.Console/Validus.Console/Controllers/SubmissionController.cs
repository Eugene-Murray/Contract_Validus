using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.MVC;
using Validus.Models;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using Validator = System.ComponentModel.DataAnnotations.Validator;


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
			    this.ViewBag.DefaultOffice = (user.DefaultOrigOffice != null)
				                                 ? string.Format("{0} : {1}",
				                                                 user.DefaultOrigOffice.Id,
				                                                 user.DefaultOrigOffice.Name)
				                                 : string.Empty;

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

			    this.ViewBag.DefaultUnderwriter = (underwriter != null)
				                                      ? string.Format("{0} : {1}",
				                                                      underwriter.Code,
				                                                      underwriter.Name)
				                                      : string.Empty;
		    }
	    }


		// GET: /Submission/_TemplateEN
        [HttpGet,
        OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
        public PartialViewResult _TemplateEN()
        {
			this.SetupTemplate("EN");

            return this.PartialView();
        }

		//
		// GET: /Submission/_TemplatePV
        [HttpGet,
        OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
        public PartialViewResult _TemplatePV()
        {
			this.SetupTemplate("PV");

            return this.PartialView();
        }

        //
        // GET: /Submission/_TemplatePV
        [HttpGet,
        OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
        public PartialViewResult _TemplateFI()
        {
            this.SetupTemplate("FI");

            return this.PartialView();
        }

        //
		// GET: /Submission/_OptionTemplate
        // NOTE: Cannot set OutputCache as it is a child action of _Template
		//[HttpGet] TODO: Bring back and use as a base
		//public PartialViewResult _OptionTemplate()
		//{
		//	return this.PartialView();
		//}

		//
		// GET: /Submission/_OptionTemplateEN
        [HttpGet]
        public PartialViewResult _OptionTemplateEN()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_OptionTemplatePV
        [HttpGet]
        public PartialViewResult _OptionTemplatePV()
        {
            return this.PartialView();
        }

        //
        // GET: /Submission/_OptionTemplateFI
        [HttpGet]
        public PartialViewResult _OptionTemplateFI()
        {
            //var user = this.WebSiteModuleManager.EnsureCurrentUser();

            //if (user != null)
            //{
            //    if (user.TeamMemberships != null)
            //    {
            //        var membership = user.TeamMemberships
            //                             .FirstOrDefault(tm => tm.IsCurrent &&
            //                                                   tm.Team.SubmissionTypeId == "FI");

            //        if (membership != null && membership.Team != null)
            //        {
            //            ViewBag.RelatedRiskList = membership.Team.RelatedRisks;
            //        }
            //    }
            //}

            return this.PartialView();
        }

		//
		// GET: /Submission/_OptionVersionTemplatePV
        [HttpGet]
        public PartialViewResult _OptionVersionTemplatePV()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_OptionTemplateExampleEnergy
        [HttpGet]
        public PartialViewResult _OptionTemplateExampleEnergy()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_QuoteTemplate
		// NOTE: Cannot set OutputCache as it is a child action of _OptionTemplate
		//[HttpGet] TODO: Bring back and use as a base
		//public PartialViewResult _QuoteTemplate()
		//{
		//	return this.PartialView();
		//}

		//
		// GET: /Submission/_QuoteTemplateEN
        [HttpGet]
		public PartialViewResult _QuoteTemplateEN(string typeId = null)
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_QuoteTemplatePV
        [HttpGet]
		public PartialViewResult _QuoteTemplatePV(string typeId = null)
        {
            return this.PartialView();
        }

        //
        // GET: /Submission/_QuoteTemplateFI
        [HttpGet]
        public PartialViewResult _QuoteTemplateFI(string typeId = null)
        {
            return this.PartialView();
        }

        [HttpGet]
        public PartialViewResult _QuoteTemplateExampleEnergy()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_MarketWordingTemplate
        [HttpGet]
        public PartialViewResult _MarketWordingTemplate()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_TermsNConditionTemplate
        [HttpGet]
        public PartialViewResult _TermsNConditionTemplate()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_SubjectToClauseWordingTemplate
        [HttpGet]
        public PartialViewResult _SubjectToClauseWordingTemplate()
        {
            return this.PartialView();
        }

		//
		// GET: /Submission/_QuoteComparisonTable
        public PartialViewResult _QuoteComparisonTable()
        {
			return this.PartialView();
        }

        public PartialViewResult _CreateBrokerContact()
        {
			return this.PartialView();
        }

        public PartialViewResult _SubmissionSideBar()
        {
			return this.PartialView();
        }

        //
		// GET: /Submission/GetSubmission/5
        [HttpGet,
        OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult GetSubmission(int id = 0)
        {
            var submission = this.SubmissionModule.GetSubmissionById(id);

	        if (submission == null)
		        throw new HttpException((int)HttpStatusCode.NotFound,
		                                string.Format("Submission of id '{0}' could not be found", id));

            return new JsonNetResult
            {
                Data = new { Submission = submission }
            };
        }

        [HttpGet,
        OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _CrossSellingCheck(string insuredName, int thisSubmissionId)
        {
            if (string.IsNullOrEmpty(insuredName)) 
                return this.PartialView(new List<CrossSellingCheckDto>());

            var matches = this.SubmissionModule.CrossSellingCheck(Uri.UnescapeDataString(insuredName), thisSubmissionId);

                Response.StatusCode = (Int32)HttpStatusCode.Created;
            return PartialView(matches);
        }
       
        [HttpPost, 
		Authorize(Roles = @"ConsoleUW")]
        public ActionResult CreateSubmission(SubmissionDto submissionDto)
		{
			var savedSubmission = default(Submission);
            var errors = new List<string>();

            switch (submissionDto.SubmissionTypeId)
            {
	            case "EN":
	            {
		            var submissionEnergy = SubmissionModuleHelpers.SetupENSubmission(submissionDto);
                    
                    var results = Validation.Validate(submissionEnergy);
                    if (!results.IsValid)
                    {
                        errors.AddRange(results.Select(vr=>vr.Message));
                    
                    }

	                if (errors.Count == 0)
	                {
	                    savedSubmission = this.SubmissionModule.CreateSubmission(submissionEnergy, out errors);
	                }
	            } break;
	            case "PV":
	            {
		            var submissionPV = SubmissionModuleHelpers.SetupPVSubmission(submissionDto);
                    
                    var results = Validation.Validate(submissionPV);
                    if (!results.IsValid)
                    {
                        errors.AddRange(results.Select(vr => vr.Message));

                    }

	                if (errors.Count == 0)
	                {
	                    savedSubmission = this.SubmissionModule.CreateSubmission(submissionPV, out errors);
	                }
	            } break;
                case "FI":
                    {
                        var submissionFI = SubmissionModuleHelpers.SetupFISubmission(submissionDto);

                        savedSubmission = this.SubmissionModule.CreateSubmission(submissionFI, out errors);
                    } break;
				default:
		            throw new NotImplementedException(submissionDto.SubmissionTypeId);
            }
           
            if (errors.Count == 0)
            {
                //foreach (var auditTrail in submissionDto.AuditTrails.Where(at => at.Description.Contains(savedSubmission.InsuredName)))
                if (submissionDto.AuditTrails != null)
                    foreach (var auditTrail in submissionDto.AuditTrails.Where(at => at.Description.Length > "World Check requested for insured name: ".Length))
                {
                    this.AuditTrailModule.Audit(auditTrail.Source, savedSubmission.Id.ToString(), auditTrail.Title, auditTrail.Description);
                }

                this.Response.StatusCode = (int)HttpStatusCode.Created;
                this.Response.AddHeader("Location", "/submission/getsubmission/" + savedSubmission.Id);
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return new JsonNetResult
            {
                Data = new
                {
                    Submission = savedSubmission,
                    Error = new Dictionary<string, string[]> { { "Subscribe", errors.ToArray() } }
                }
            };
        }

        [HttpPut, 
		Authorize(Roles = @"ConsoleUW")]
        public ActionResult EditSubmission(SubmissionDto submissionDto)
		{
			var submission = default(Submission);
            var errors = new List<String>();
            var quotes = new List<Quote>();

            switch (submissionDto.SubmissionTypeId)
            {
	            case "EN":
	            {
		            var submissionEnergy = SubmissionModuleHelpers.SetupENSubmission(submissionDto);
                    var results = Validation.Validate(submissionEnergy);
                    if (!results.IsValid)
                    {
                        errors.AddRange(results.Select(vr=>vr.Message));
                    
                    }

	                if (errors.Count == 0)
	                {
	                    submission = this.SubmissionModule.UpdateSubmission(submissionEnergy, out errors, out quotes);
	                }

	            } break;
	            case "PV":
	            {
		            var submissionPV = SubmissionModuleHelpers.SetupPVSubmission(submissionDto);
                    var results = Validation.Validate(submissionPV);
                    if (!results.IsValid)
                    {
                        errors.AddRange(results.Select(vr=>vr.Message));
                    
                    }

	                if (errors.Count == 0)
	                {
	                    submission = this.SubmissionModule.UpdateSubmission(submissionPV, out errors, out quotes);
	                }
	            } break;
                case "FI":
                    {
                        var submissionFI = SubmissionModuleHelpers.SetupFISubmission(submissionDto);

                        submission = this.SubmissionModule.UpdateSubmission(submissionFI, out errors, out quotes);
                    } break;
				default:
		            throw new NotImplementedException(submissionDto.SubmissionTypeId);
            }

           

	        if (errors.Count == 0)
	        {
		        //foreach (var auditTrail in submissionDto.AuditTrails.Where(at => at.Description.Contains(submission.InsuredName)))
	            if (submissionDto.AuditTrails != null)
	                foreach (var auditTrail in submissionDto.AuditTrails.Where(at => at.Description.Length > "World Check requested for insured name: ".Length))
		        {
			        this.AuditTrailModule.Audit(auditTrail.Source,
			                                    submission.Id.ToString(),
			                                    auditTrail.Title,
			                                    auditTrail.Description);
		        }
	        }
	        else
	        {
		        this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
	        }

	        return new JsonNetResult
            {
                Data = new
                {
                    Submission = submission,
                    Error = new Dictionary<string, string[]> { { "Subscribe", errors.ToArray() } }
                }
            };
        }



	    [Authorize(Roles = @"ConsoleUW"),
		OutputCache(CacheProfile = "NoCacheProfile")]
	    public ActionResult _WorldCheckAuditTrailForSubmission(int id)
	    {
			var auditTrails = this.AuditTrailModule.GetAuditTrails("Submission", id.ToString());

			this.Response.StatusCode = (int)HttpStatusCode.Created;

			return this.PartialView(auditTrails);
	    }

	    //
        // GET: /Submission/_IndexJSON
        [HttpGet]
        public ActionResult _IndexJSON(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0,
            Boolean applyProfileFilters = true, Int32 iDisplayLength = 10, Int32 iDisplayStart = 0)
        {
            var iTotalRecords = default(int);
			var iTotalDisplayRecords = default(int);
            var sortCol = string.Empty;

            //  iSortCol_0 currently comes from the order of columns in the UI I think. This looks a bit fragile - is DataTables the best grid?
            switch (iSortCol_0)
            {
                case 0:
                    sortCol = "Id";
                    break;
                case 1:
                    sortCol = "InsuredName";
                    break;
                case 2:
                    sortCol = "BrokerPseudonym";
                    break;
                default:
                    sortCol = "InceptionDate";
                    break;
            }

            var aaData = this.SubmissionModule.GetSubmissions(sSearch, iDisplayStart, iDisplayLength,
                                                                sortCol, sSortDir_0, applyProfileFilters, out iTotalDisplayRecords,
                                                                out iTotalRecords);

            // TODO: Change to JsonNetResult
            return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Submission/_Preview/5
        [HttpGet,
        OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
        public ActionResult _Preview(int id)
        {
            var submission = this.SubmissionModule.GetSubmissionPreviewById(id);

            if (submission == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return this.PartialView("_Preview", submission);
        }

        protected override void Dispose(bool disposing)
        {
            this.SubmissionModule.Dispose();

            base.Dispose(disposing);
        }
    }
}