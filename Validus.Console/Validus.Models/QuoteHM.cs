using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Validus.Models
{
    [Table("QuotesHM")]
    public class QuoteHM : Quote
	{
		[DisplayName("Amt / OPL")]
		public string AmountOrOPL { get; set; }

		[DisplayName("% / Amt / % ONP")]
		public string AmountOrONP { get; set; }

        [Display(Name = "Vessel Top Limit Currency"), StringLength(10),
            RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "Not a valid Currency")]
        public String VesselTopLimitCurrency { get; set; }

        [Display(Name = "Vessel Top Limit Amount")]
        public Decimal? VesselTopLimitAmount { get; set; }
	
    }
}
