using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Validus.Models
{
    [Table("QuotesEN")]
    public class QuoteEN : Quote
	{
		[DisplayName("Amt / OPL")]
		public string AmountOrOPL { get; set; }

		[DisplayName("% / Amt / % ONP")]
		public string AmountOrONP { get; set; }

		[DisplayName("Quote Comments"), StringLength(256)] // TODO: Confirm max length
		public string QuoteComments { get; set; }
    }
}
