using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Core.MVC;
using Validus.Models;

namespace Validus.Console.Controllers
{
	[Authorize(Roles="ConsoleUW")]
    public class QuoteSheetController : Controller
	{
		protected readonly string NotFoundMessage = "Submission with an id of {0} could not be found";

		protected IQuoteSheetModule QuoteSheetModule { get; set; }

        public QuoteSheetController(IQuoteSheetModule quotesheetModule)
        {
			this.QuoteSheetModule = quotesheetModule;
        }

		//
		// POST: api/QuoteSheet
		[HttpPost]
		public ActionResult CreateQuote(CreateQuoteSheetDto dto)
		{
			Submission submission = null;

			var url = this.QuoteSheetModule.CreateQuoteSheet(dto, out submission);

			if (submission == null)
				throw new HttpException((int)HttpStatusCode.NotFound,
										string.Format(this.NotFoundMessage, dto.SubmissionId));

			this.Response.StatusCode = (int)HttpStatusCode.Created;
			this.Response.AddHeader("Location", url);

			return new JsonNetResult
			{
				Data = new { Submission = submission }
			};
        }

		protected override void Dispose(bool disposing)
		{
			this.QuoteSheetModule.Dispose();

			base.Dispose(disposing);
		}
    }
}