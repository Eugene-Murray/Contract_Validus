using System;
using System.Collections.Generic;

namespace Validus.Console.DTO
{
    public class OptionDto
    {
        public Int32 Id { get; set; }
        public int OptionId { get; set; }
        public Int32 SubmissionId { get; set; }
        public String Title { get; set; }
        public String Comments { get; set; }
        public ICollection<OptionVersionDto> OptionVersions { get; set; }
        public List<int> OptionVersionNumberList { get; set; }
        public Byte[] Timestamp { get; set; }

        //  FI Specific
        public String RiskCodes { get; set; }
    }
}