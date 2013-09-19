using System;
using System.Collections.Generic;

namespace Validus.Console.DTO
{
    public class CreateQuoteSheetDto
    {
        public string SubmissionTypeId { get; set; }
        public int QuoteSheetTemplateId { get; set; }
        public string QuoteSheetTemplateUrl { get; set; }
        public int SubmissionId { get; set; }
        public List<OptionDto> OptionList { get; set; }
    }
}