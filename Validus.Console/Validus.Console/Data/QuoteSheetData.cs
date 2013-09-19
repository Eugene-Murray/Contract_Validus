using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Validus.Console.DTO;
using Validus.Console.DocumentManagementService;
using Validus.Console.ReportingService;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Models;
using System.Configuration;

namespace Validus.Console.Data
{
    public class QuoteSheetData : IQuoteSheetData
    {
	    public byte[] CreateQuoteSheetPdf(CreateQuoteSheetDto dto)
	    {
		    var parameters = new List<ParameterValue>();
		    //var reportPath = ConfigurationManager.AppSettings["QuoteSheetReportPath"] ??
		                     //"/Underwriting/Console/QuoteSheet";
            var reportPath = dto.QuoteSheetTemplateUrl ??
		                     ConfigurationManager.AppSettings["QuoteSheetReportPath"];

		    using (var reportService = new ReportExecutionService())
		    {
			    reportService.PreAuthenticate = true;
			    reportService.Credentials = CredentialCache.DefaultCredentials;

			    foreach (var option in dto.OptionList)
			    {
				    parameters.AddRange(option.OptionVersionNumberList.Select(versionNumber => new ParameterValue
				    {
					    Name = "OptionVersions",
					    Value = option.OptionId + "," + versionNumber
				    }));
			    }

			    reportService.LoadReport(reportPath, null);
			    reportService.SetExecutionParameters(parameters.ToArray(), "en-gb");

			    String extension, encoding, mimetype;
			    String[] streamIds;
			    Warning[] warnings;

			    return reportService.Render("PDF", null, out extension, out encoding,
			                                out mimetype, out warnings, out streamIds);
		    }
	    }

        public Guid SaveQuoteSheetToDMS(QuoteSheet quoteSheet, byte[] reportBytes, Submission submission)
        {
	        var fileId = Guid.Empty.ToString();

			// TODO: Exception handling
	        using (var dmsService = new DMSService())
	        {
		        fileId = dmsService.FNUploadDocument(quoteSheet.Title + ".pdf", quoteSheet.Title, reportBytes,
		                                             quoteSheet.ObjectStore, quoteSheet.ObjectStore);

				dmsService.FNUpdateDocumentProperties(fileId, quoteSheet.ObjectStore, quoteSheet.ObjectStore,
		                                              this.SetFileNetProperties(quoteSheet, submission));
	        }

			return new Guid(fileId);
        }

		// TODO: Don't we need know which quote is generating the quotesheet?
        private FileNetProperty[] SetFileNetProperties(QuoteSheet quoteSheet, Submission submission)
        {
            var options = submission.Options.Select(o => o);
            var optionVersions = options.SelectMany(ov => ov.OptionVersions);
            var quotes = optionVersions.SelectMany(ov => ov.Quotes).Where(q => q.IsSubscribeMaster).ToList();

            var subscribeRefList = quotes.Select(q => q.SubscribeReference).Aggregate(string.Empty, (current, subscribeRef) => current + (subscribeRef + ";"));
	        var currentDateTime = DateTime.Now;

			var mainQuote = quotes[0]; // TODO: For the moment, we will use the first quote we come across
			var businessPlanList = string.Empty;

			// TODO: Exception handling
			using (var subscribeService = new SubscribeSoapService.Subscribe())
			{
				businessPlanList = quotes.Select(q => q.SubscribeReference)
					.Select(subscribeService.GetPolicyDetail)
					.Aggregate(businessPlanList, (current, policyDetails) => current + (policyDetails.BusinessPlan + ";"));
            }

            List<FileNetProperty> properties = new List<FileNetProperty>
            {
                    /* Base Document Properties
				 * 
				 * Mandatory;
				 * - DocumentTitle (String)
				 * 
				 * Optional;
				 * - Description (String)
				 */
                    new FileNetProperty {Key = "DocumentTitle", Value = "Quote sheet"},
                 //   new FileNetProperty {Key = "Description", Value = "Quote sheet description"},

                    /* Underwriting Document Properties
				 * 
				 * Mandatory;
				 * - uwPolicyID (ListOfString): List of Subscribe references delimited by semi-colon.
				 * - uwBusinessPlan (ListOfString): List of business plans delimited by semi-colon.
				 * - uwDocType (String): Document type.
				 * - uwInsuredName (String): Insured name.
				 * - uwCOB (String): Class of business code.
				 * - uwUnderwriter (String): Underwriter pseudonym.
				 * - uwBrokerPSU (String): Broker pseudonym.
				 * - uwInceptionDate (Date): Inception date.
				 * - uwAccountingYear (Integer): Accounting year.
				 * - uwStatus (String): Status, which is always "QUOTE" at this stage.
				 * - uwEntryStatus (String): Entry status, which is either "PARTIAL" or "NTU" at this stage.
				 * 
				 * Optional;
				 * - PolicyIDSP (String): List of Subscribe references delimited by semi-colon.
				 * - BusinessPlanSP (String): List of business plans delimited by semi-colon.
				 * - uwWrittenDate (Date): Written date.
				 * - AccountingYearFloat (Float): Accounting year.
				 * 
				 * Note 1: 
				 * FileNet has two fields for each Policy ID and Business Plan. One field is a String data-type 
				 * and the other (the primary field) is a ListOfString. For consistency, both of the values should be
				 * the same with the exception that the ListOfString will be parsed by the web service into an array
				 * by a specified delimiter (this was a quick-fix on the web service).
				 * 
				 * In order to pass back a ListOfString value to the FileNet web service, a delimited string should
				 * be passed with the delimiter specified in the Delimiter property of the FileNetProperty class.
				 * 
				 * Note 2:
				 * The values for the document type must exist in the FileNet document type choice list or it will
				 * throw back an error.
				 * 
				 * Note 3:
				 * FileNet has two fields for accounting year. One field is a Float data-type and the other is an
				 * Integer. For consistency, both of the values should be the same.
				 */
                    new FileNetProperty {Key = "uwPolicyID", Value = subscribeRefList, Delimiter = ";"},
                    new FileNetProperty {Key = "PolicyIDSP", Value = subscribeRefList},
                    new FileNetProperty {Key = "uwBusinessPlan", Value = businessPlanList, Delimiter = ";"},
                    new FileNetProperty {Key = "BusinessPlanSP", Value = businessPlanList},
                    new FileNetProperty {Key = "uwDocType", Value = "Quote"},
                    new FileNetProperty {Key = "uwInsuredName", Value = submission.InsuredName},
                    new FileNetProperty {Key = "uwCOB", Value = mainQuote.COBId},
                    new FileNetProperty {Key = "uwUnderwriter", Value = submission.UnderwriterContactCode},
                    new FileNetProperty {Key = "uwBrokerPSU", Value = submission.BrokerPseudonym},
                    new FileNetProperty {Key = "uwAccountingYear", Value = mainQuote.AccountYear},
                    new FileNetProperty {Key = "uwStatus", Value = "QUOTE"},
                    new FileNetProperty {Key = "uwEntryStatus", Value = mainQuote.EntryStatus},

                    /* Other Custom Document Properties
				 * 
				 * Mandatory;
				 * - uwInputDeviceID (String): The system that uploaded the document ("Console"). The other entry 
				 *		points are Kofax, Worflow (via Web Services) and SharePoint (via ICC). This is to help 
				 *		administrators understand the origins of the document.
				 * - uwIndexPerson (String): Login name of the user that uploaded the document.
				 * - uwLoadToDMSDateAndTime (Date): Upload date.
				 * - uwOpsInfo (String): Delimited string of workflow status flags, which is always
				 *		"Aggs=0;FacRI=0;Group=0;BPQARequired=0;PTradeFile=0;S2000=0;PreBind=0;PostBind=0;ReSign=0;t&cChange=0;UrgentCase=0;ForeignLanguage=0"
				 *		at this stage.
				 * 
				 * Optional;
				 * - uwIndexDateTime (Date): Upload date.
				 * - strLoadToDMSDateAndTime (String): Upload date-time string in the format of "dd/MM/yyyy HH:mm".
				 * - WSTriggerStatus (String): Workflow initiation status flag used by FileNet, which is always
				 *		"INVALID" at this stage.
				 */
                    new FileNetProperty {Key = "uwInputDeviceID", Value = "Console"},
                    new FileNetProperty {Key = "uwIndexPerson", Value = quoteSheet.IssuedBy.DomainLogon},
                    // TODO: Is this the current user?
                    new FileNetProperty {Key = "uwIndexDateTime", Value = currentDateTime},
                    new FileNetProperty {Key = "uwLoadToDMSDateAndTime", Value = currentDateTime},
                    new FileNetProperty
                        {
                            Key = "strLoadToDMSDateAndTime",
                            Value = currentDateTime.ToString("dd/MM/yyyy HH:mm")
                        },
                    new FileNetProperty {Key = "WSTriggerStatus", Value = "INVALID"},
                    new FileNetProperty
                        {
                            Key = "uwOpsInfo",
                            Value =
                                "Aggs=0;FacRI=0;Group=0;BPQARequired=0;PTradeFile=0;S2000=0;PreBind=0;PostBind=0;ReSign=0;t&cChange=0;UrgentCase=0;ForeignLanguage=0"
                        }

                };

            if (mainQuote.InceptionDate != null)
            {
                properties.Add(new FileNetProperty
                {
                    Key = "uwInceptionDate",
                    Value = mainQuote.InceptionDate.GetValueOrDefault()
                });
            }

            return properties.ToArray();
        }

    }
}