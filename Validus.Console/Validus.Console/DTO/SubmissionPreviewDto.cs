using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Validus.Console.DTO
{
    public class SubmissionPreviewDto
    {
        public string SubmissionId { get; set; }
        public string SubmissionTypeId { get; set; }
        public string Title { get; set; }
        public string InsuredName { get; set; }
        [DisplayName("Broker"), StringLength(50)]
        public String BrokerPseudonym { get; set; }
        [DisplayName("Broker Contact")]
        public string BrokerContact { get; set; }
        public string BrokerCode { get; set; }
        public List<QuoteDto> QuoteList { get; set; }
        public QuoteSheetDto QuoteSheet { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}
