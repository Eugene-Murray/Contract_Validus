using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class RenewalDataDto
    {
        public string PolicyId { get; set; }
        public List<string> TeamSubmissionTypes { get; set; }
    }
}