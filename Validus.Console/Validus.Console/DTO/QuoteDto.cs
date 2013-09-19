using System;
using System.Configuration;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class QuoteDto
    {
        public Int32 Id { get; set; }
        public string StatusTooltip { get; set; }
        public Int32 OptionId { get; set; }
        public Int32 VersionNumber { get; set; }
        public OptionVersion OptionVersion { get; set; }
        public Guid? CorrelationToken { get; set; }
        public Boolean IsSubscribeMaster { get; set; }
        public Int32? CopiedFromQuoteId { get; set; }
        public String SubscribeReference { get; set; }
        public String RenPolId { get; set; }
        public String FacilityRef { get; set; }
        public String SubmissionStatus { get; set; }
        public String EntryStatus { get; set; }
        public String PolicyType { get; set; }
        public String OriginatingOfficeId { get; set; }
        public Office OriginatingOffice { get; set; }
        public String COBId { get; set; }
        public COB COB { get; set; }
        public String MOA { get; set; }
        public Int32 AccountYear { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime QuoteExpiryDate { get; set; }
        public String TechnicalPricingMethod { get; set; }
        public String TechnicalPricingBindStatus { get; set; }
        public String TechnicalPricingPremiumPctgAmt { get; set; }
        public Decimal? TechnicalPremium { get; set; }
        public String Currency { get; set; }
        public String LimitCCY { get; set; }
        public String ExcessCCY { get; set; }
        public Decimal? BenchmarkPremium { get; set; }
        public string QuotedPremium { get; set; }
        public Decimal? LimitAmount { get; set; }
        public Decimal? ExcessAmount { get; set; }
        public String Comment { get; set; }
        public String DeclinatureReason { get; set; }
        public String DeclinatureComments { get; set; }
        public Byte[] Timestamp { get; set; }
        public long? SubscribeTimestamp { get; set; }

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

        // EN
		public string AmountOrOPL { get; set; }
		public string AmountOrONP { get; set; }
		public string QuoteComments { get; set; }

        // PV
        public String PDPctgAmt { get; set; }
        public String PDExcessCurrency { get; set; }
        public Decimal? PDExcessAmount { get; set; }
        public String BIPctgAmtDays { get; set; }
        public String BIExcessCurrency { get; set; }
        public Decimal? BIExcessAmount { get; set; }
        public Decimal? LineSize { get; set; }
        public Boolean LineToStand { get; set; }

        // FI
        public Boolean IsReinstatement { get; set; }
        public String RiskCodeId { get; set; }
    }
}
