using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Validus.Console.Data;
using Validus.Console.DTO;
using Validus.Core.HttpContext;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public class QuoteSheetModule : IQuoteSheetModule
    {
        protected readonly string QuoteSheetUrl = "{0}?FileID={1}&ObjectStore=Underwriting";
        protected readonly string QuoteSheetTitle = "QuoteSheet_{0}_{1}_{2}";
        protected readonly string NotFoundMessage = "Submission with an id of {0} could not be found";

        protected IConsoleRepository ConsoleRepository { get; set; }
        protected ISubmissionModule SubmissionModule { get; set; }
        protected IQuoteSheetData QuoteSheetData { get; set; }
        protected ICurrentHttpContext HttpContext { get; set; }

        public QuoteSheetModule(IConsoleRepository consoleRepository, ISubmissionModule submissionModule,
                                IQuoteSheetData quotesheetData, ICurrentHttpContext httpContext)
        {
            this.ConsoleRepository = consoleRepository;
            this.SubmissionModule = submissionModule;
            this.QuoteSheetData = quotesheetData;
            this.HttpContext = httpContext;
        }

        public string CreateQuoteSheet(CreateQuoteSheetDto dto, out Submission submission)
        {
            // TODO: use the correct quote sheet...
            var quoteSheetTemplateId = dto.QuoteSheetTemplateId;
            dto.QuoteSheetTemplateUrl = this.ConsoleRepository.Query<QuoteTemplate>()
                                            .FirstOrDefault(qt => qt.Id == dto.QuoteSheetTemplateId).RdlPath;
            dto.QuoteSheetTemplateName = this.ConsoleRepository.Query<QuoteTemplate>()
                                            .FirstOrDefault(qt => qt.Id == dto.QuoteSheetTemplateId).Name;

            submission = this.SubmissionModule.GetSubmissionById(dto.SubmissionId);
            //todo this is done to clear previous context. has to be fixed once softdelet is fixed.
            using (IConsoleRepository consoleRepository = new ConsoleRepository())
            {
                if (submission == null)
                    throw new KeyNotFoundException(string.Format(this.NotFoundMessage, dto.SubmissionId));
                consoleRepository.Attach(submission);
                var currentUser = consoleRepository.Query<User>()
                                      .FirstOrDefault(u => u.DomainLogon == this.HttpContext.CurrentUser.Identity.Name);

                if (currentUser == null)
                    throw new ApplicationException("Current user could not be found");

                var quotesheet = new QuoteSheet
                {
                    Title = string.Format(this.QuoteSheetTitle, submission.Title, submission.InsuredName, DateTime.Now),
                    IssuedBy = currentUser,
                    IssuedById = currentUser.Id,
                    IssuedDate = DateTime.Now,
                    ObjectStore = "Underwriting"
                };

                var content = this.QuoteSheetData.CreateQuoteSheetPdf(dto);

                quotesheet.Guid = this.QuoteSheetData.SaveQuoteSheetToDMS(quotesheet, content, submission);

                var versions = (from version in submission.Options.SelectMany(o => o.OptionVersions)
                                from option in dto.OptionList
                                where version.OptionId == option.OptionId
                                from versionNumber in option.OptionVersionNumberList
                                where version.VersionNumber == versionNumber
                                select version).ToList();

                quotesheet.OptionVersions = versions;

                foreach (var quote in versions.SelectMany(ov => ov.Quotes))
                {
                    quote.OptionVersion.IsLocked = true;

                    quote.SubmissionStatus = "QUOTED";
                }

                consoleRepository.Add(quotesheet);
                consoleRepository.SaveChanges();
                return string.Format(this.QuoteSheetUrl,
                               ConfigurationManager.AppSettings["UWDmsFileDownloadURL"],
                               quotesheet.Guid);
            }

          
        }

        public void Dispose()
        {
            this.SubmissionModule.Dispose();
            this.ConsoleRepository.Dispose();
        }
    }
}