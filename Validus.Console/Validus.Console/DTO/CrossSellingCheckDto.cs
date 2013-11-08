using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class CrossSellingCheckDto
    {
        public string Count { get; set; }
        public string SubmissionId { get; set; }
        public string SubmissionTypeId { get; set; }
        public string SubmissionTitle { get; set; }
        public string QuotingOffice { get; set; }
        public string Underwriter { get; set; }
        public string ButtonTitle { get; set; }
        public bool IsReadOnly { get; set; }
    }
}