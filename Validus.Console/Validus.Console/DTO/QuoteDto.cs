using System;
using System.Configuration;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class QuoteDto
	{
		public string QuoteWebPolicyURL
		{
			get
			{
				return ConfigurationManager.AppSettings["WebPolicyURL"] + this.SubscribeReference;
			}
		}

		public string FacilityWebPolicyURL
		{
			get
			{
				return ConfigurationManager.AppSettings["WebPolicyURL"] + this.FacilityRef;
			}
		}

        public int Id { get; set; }
        public string StatusTooltip { get; set; }
        public int OptionId { get; set; }
        public int VersionNumber { get; set; }
        public OptionVersion OptionVersion { get; set; }
        public Guid? CorrelationToken { get; set; }
        public bool IsSubscribeMaster { get; set; }
        public int? CopiedFromQuoteId { get; set; }
        public string SubscribeReference { get; set; }
        public string RenPolId { get; set; }
        public string FacilityRef { get; set; }
        public string SubmissionStatus { get; set; }
        public string EntryStatus { get; set; }
        public string PolicyType { get; set; }
        public string OriginatingOfficeId { get; set; }
        public Office OriginatingOffice { get; set; }
        public string COBId { get; set; }
        public COB COB { get; set; }
        public string MOA { get; set; }
        public int AccountYear { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime QuoteExpiryDate { get; set; }
        public string TechnicalPricingMethod { get; set; }
        public string TechnicalPricingBindStatus { get; set; }
        public string TechnicalPricingPremiumPctgAmt { get; set; }
        public decimal? TechnicalPremium { get; set; }
        public string Currency { get; set; }
        public string LimitCCY { get; set; }
        public string ExcessCCY { get; set; }
        public decimal? BenchmarkPremium { get; set; }
        public string QuotedPremium { get; set; }
        public decimal? LimitAmount { get; set; }
		public decimal? ExcessAmount { get; set; }
		public string Description { get; set; }
        public string Comment { get; set; }
        public string DeclinatureReason { get; set; }
        public string DeclinatureComments { get; set; }
        public byte[] Timestamp { get; set; }
        public long? SubscribeTimestamp { get; set; }

        // EN
		public string AmountOrOPL { get; set; }
		public string AmountOrONP { get; set; }
		public string QuoteComments { get; set; }

        // PV
        public string PDPctgAmt { get; set; }
        public string PDExcessCurrency { get; set; }
        public decimal? PDExcessAmount { get; set; }
        public string BIPctgAmtDays { get; set; }
        public string BIExcessCurrency { get; set; }
        public decimal? BIExcessAmount { get; set; }
        public decimal? LineSize { get; set; }
        public bool LineToStand { get; set; }

        // FI
        public bool IsReinstatement { get; set; }
        public string RiskCodeId { get; set; }
    }
}
