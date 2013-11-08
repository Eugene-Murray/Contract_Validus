using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Validus.Console.BusinessLogic;
using Validus.Core.HTTP;
using Validus.Models;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead, ConsoleUW"), HttpModelStateFilter]
    public class SubmissionApiController : ApiController
    {
        public ISubmissionModule SubmissionModule { get; protected set; }
        public IAuditTrailModule AuditTrailModule { get; protected set; }

	    public SubmissionApiController(ISubmissionModule submissionModule,
	                                   IAuditTrailModule auditTrailModule)
	    {
		    this.SubmissionModule = submissionModule;
		    this.AuditTrailModule = auditTrailModule;
	    }

		[HttpGet]
		public HttpResponseMessage GetSubmission(int id = 0)
		{
			var submission = this.SubmissionModule.GetSubmissionById(id);

			if (submission == null)
				throw new HttpException((int)HttpStatusCode.NotFound,
										string.Format("Submission of id '{0}' could not be found", id));

			return this.Request.CreateResponse(HttpStatusCode.OK, new
			{
				Submission = submission
			});
		}

	    [HttpPost, Authorize(Roles = @"ConsoleUW")]
		public HttpResponseMessage CreateSubmission(Submission submission)
	    {
            var errors = new List<string>();

		    var newSubmission = this.SubmissionModule.CreateSubmission(SubmissionModuleHelpers.SetupWording(submission),
		                                                                 out errors);

		    if (errors.Count == 0 && submission.AuditTrails != null)
		    {
			    foreach (var auditTrail in submission.AuditTrails
			                                         .Where(at => at.Description.Length >
			                                                      "World Check requested for insured name: ".Length))
			    {
				    this.AuditTrailModule.Audit(auditTrail.Source,
												newSubmission.Id.ToString(),
				                                auditTrail.Title,
				                                auditTrail.Description);
			    }
		    }

		    return this.Request.CreateResponse(errors.Count == 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest, new
			{
				Submission = newSubmission,
				Error = new Dictionary<string, string[]> { { "Subscribe", errors.ToArray() } }
			});
        }

        [HttpPut, Authorize(Roles = @"ConsoleUW")]
        public HttpResponseMessage EditSubmission(Submission submission)
        {
            var errors = new List<String>();
            var quotes = new List<Quote>();

	        var savedSubmission = this.SubmissionModule.UpdateSubmission(SubmissionModuleHelpers.SetupWording(submission),
	                                                                     out errors, out quotes);

			if (errors.Count == 0 && submission.AuditTrails != null)
	        {
		        foreach (var auditTrail in submission.AuditTrails
		                                             .Where(at => at.Description.Length >
		                                                          "World Check requested for insured name: ".Length))
		        {
			        this.AuditTrailModule.Audit(auditTrail.Source,
			                                    submission.Id.ToString(),
			                                    auditTrail.Title,
			                                    auditTrail.Description);
		        }
	        }

	        return this.Request.CreateResponse(errors.Count == 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest, new
	        {
		        Submission = savedSubmission,
		        Error = new Dictionary<string, string[]> { { "Subscribe", errors.ToArray() } }
	        });
        }
    }
}
