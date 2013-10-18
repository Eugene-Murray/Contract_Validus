using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class RiskPreviewDto
    {
        public string WebPolicyURL { get; set; }
        public string PolicyId { get; set; }
        public string InsuredName { get; set; }
        public string Description { get; set; }
        public string EntryStatus { get; set; }
        public string PolicyStatus { get; set; }
        public string SubmissionStatus { get; set; }
        public string BrokerGroupCode { get; set; }
        public string PSU { get; set; }
        public string CD { get; set; }
        public string Contact { get; set; }
        public string UMR { get; set; }
        public string Underwriter { get; set; }
        public string SubscribeNotes { get; set; }
        public string ConsoleNotes { get; set; }
        public string DMSCount { get; set; }
    }
}