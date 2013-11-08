using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class RiskPreviewDto
    {
        [DisplayName("Policy Id")]
        public string PolicyId { get; set; }
        [DisplayName("Insured Name")]
        public string InsuredName { get; set; }
        public string Description { get; set; }
        [DisplayName("Entry Status")]
        public string EntryStatus { get; set; }
        [DisplayName("Policy Status")]
        public string PolicyStatus { get; set; }
        [DisplayName("Submission Status")]
        public string SubmissionStatus { get; set; }
        [DisplayName("Broker Group Code")]
        public string BrokerGroupCode { get; set; }
        public string PSU { get; set; }
        public string CD { get; set; }
        public string Contact { get; set; }
        public string UMR { get; set; }
        public string Underwriter { get; set; }
        [DisplayName("Subscribe Notes")]
        public string SubscribeNotes { get; set; }
        [DisplayName("Console Quote Notes")]
        public string ConsoleQuoteNotes { get; set; }
        [DisplayName("No of Related Documents")]
        public string DMSCount { get; set; }

        public string WebPolicyURL
        {
            get { return ConfigurationManager.AppSettings["WebPolicyURL"] + PolicyId; }
        }
    }
}