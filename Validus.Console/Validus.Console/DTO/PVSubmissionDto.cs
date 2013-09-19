using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class PVSubmissionDto : SubmissionDto
    {
        public string SubmissionType { get; set; }
		public string SubmissionTypeId { get; set; }
		public string ExtraProperty1 { get; set; }
		public string ExtraProperty2 { get; set; }
		public string ExtraProperty3 { get; set; }
        public string ExtraProperty4 { get; set; }
    }
}