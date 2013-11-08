using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Validus.Models
{
    [Table("QuotesCA")]
    public class QuoteCA : Quote
	{
		[DisplayName("Amt / OPL")]
		public string AmountOrOPL { get; set; }

		[DisplayName("% / Amt / % ONP")]
		public string AmountOrONP { get; set; }

        [Display(Name = "Line Size"), Range(0, 100)]
        public Decimal? LineSize { get; set; }

    }
}
