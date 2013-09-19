using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.HTTP;
using Validus.Core.MVC;
using Validus.Models;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead, ConsoleUW"), HttpModelStateFilter]
    public class SubmissionApiController : ApiController
    {

        public ISubmissionModule SubmissionModule { get; protected set; }
        public IWebSiteModuleManager WebSiteModuleManager { get; protected set; }
        public IAuditTrailModule AuditTrailModule { get; protected set; }

        public SubmissionApiController(ISubmissionModule submissionModule,
                                    IWebSiteModuleManager webSiteModuleManager,
                                    IAuditTrailModule auditTrailModule)
        {
            this.SubmissionModule = submissionModule;
            this.WebSiteModuleManager = webSiteModuleManager;
            this.AuditTrailModule = auditTrailModule;
        }

        [HttpPost,
       Authorize(Roles = @"ConsoleUW")]
        public Submission CreateSubmission(Submission submission)
        {
            var savedSubmission = default(Submission);
            var errors = new List<string>();
            savedSubmission = this.SubmissionModule.CreateSubmission(SubmissionModuleHelpers.SetupWording(submission), out errors);

            if (errors.Count == 0)
            {
                if (submission.AuditTrails != null)
                    foreach (var auditTrail in submission.AuditTrails.Where(at => at.Description.Length > "World Check requested for insured name: ".Length))
                    {
                        this.AuditTrailModule.Audit(auditTrail.Source, savedSubmission.Id.ToString(), auditTrail.Title, auditTrail.Description);
                    }
                return savedSubmission;
            }

            throw new HttpResponseException(this.Request.CreateResponse<object>(HttpStatusCode.BadRequest, new
                {
                    Submission = savedSubmission,
                    Error = new Dictionary<string, string[]> { { "Subscribe", errors.ToArray() } }
                }));

        }

        [HttpPut,
    Authorize(Roles = @"ConsoleUW")]
        public Submission EditSubmission(Submission submission)
        {
            var savedSubmission = default(Submission);
            var errors = new List<String>();
            var quotes = new List<Quote>();
            savedSubmission = this.SubmissionModule.UpdateSubmission(SubmissionModuleHelpers.SetupWording(submission), out errors, out quotes);


            if (errors.Count == 0)
            {
                if (submission.AuditTrails != null)
                    foreach (var auditTrail in submission.AuditTrails.Where(at => at.Description.Length > "World Check requested for insured name: ".Length))
                    {
                        this.AuditTrailModule.Audit(auditTrail.Source,
                                                    submission.Id.ToString(),
                                                    auditTrail.Title,
                                                    auditTrail.Description);
                    }
                return savedSubmission;
            }

            throw new HttpResponseException(this.Request.CreateResponse<object>(HttpStatusCode.BadRequest, new
            {
                Submission = savedSubmission,
                Error = new Dictionary<string, string[]> { { "Subscribe", errors.ToArray() } }
            }));


        }
    }
}
