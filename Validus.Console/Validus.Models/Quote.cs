using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Validus.Models.HTTP;

namespace Validus.Models
{
    [JsonConverter(typeof(QuoteConvertor))]
    public class Quote : ModelBase
    {
		[Required, DisplayName("Id")]
        public Int32 Id { get; set; }

		[Required, DisplayName("Option Id")]
		public Int32 OptionId { get; set; }

		[Required, DisplayName("Version Number")]
		public Int32 VersionNumber { get; set; }

		[DisplayName("Option Version"), ScriptIgnore, JsonIgnore]
		public OptionVersion OptionVersion { get; set; }

		[DisplayName("Correlation Token")]
		public Guid? CorrelationToken { get; set; }

		[DisplayName("Is Primary?")]
		public Boolean IsSubscribeMaster { get; set; }

		[DisplayName("Copied From")]
		public Int32? CopiedFromQuoteId { get; set; }

		[DisplayName("Subscribe Reference"), StringLength(50),
		RegularExpression(@"^[A-Z]{3}\d{6}[A-Z](\d{6}|\d{2})$", ErrorMessage = "Not a valid Subscribe Reference")]
		public String SubscribeReference { get; set; }

		[DisplayName("Renewed From"), StringLength(50),
		RegularExpression(@"^[A-Z]{3}\d{6}[A-Z](\d{6}|\d{2})$", ErrorMessage = "Not a valid Subscribe Reference")]
		public String RenPolId { get; set; }

		[DisplayName("Facility Reference"), StringLength(50),
		RegularExpression(@"^[A-Z]{3}\d{6}[A-Z](\d{6}|\d{2})$", ErrorMessage = "Not a valid Subscribe Reference")]
		public String FacilityRef { get; set; }

		[Required, DisplayName("Submission Status"), StringLength(50)]
		public String SubmissionStatus { get; set; }

		[Required, DisplayName("Entry Status"), StringLength(50)]
		public String EntryStatus { get; set; }

		[Required, DisplayName("Policy Type"), StringLength(50)]
		public String PolicyType { get; set; }

		[Required, DisplayName("Originating Office"), StringLength(10),
		RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Not a valid Office")]
		public String OriginatingOfficeId { get; set; }

        [DisplayName("Originating Office")]
		public Office OriginatingOffice { get; set; }

		[Required, DisplayName("COB"), StringLength(10),
		RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Not a valid COB")]
		public String COBId { get; set; }

		[DisplayName("COB")]
        public COB COB { get; set; }

		[Required, DisplayName("MOA"), StringLength(10),
		RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Not a valid MOA")]
        public String MOA { get; set; }

		[Required, DisplayName("Account Year"),
		RegularExpression(@"^\d{4}$", ErrorMessage = "Not a valid Account Year")] // TODO: Should we change to "^20\d{2}$" ?
        public Int32 AccountYear { get; set; }

		[DisplayName("Inception Date")]
        public DateTime? InceptionDate { get; set; }

		[DisplayName("Expiry Date")]
		public DateTime? ExpiryDate { get; set; }

		[Required, DisplayName("Quote Expiry Date")]
		public DateTime QuoteExpiryDate { get; set; }

		[DisplayName("Pricing Method"), StringLength(10)] // TODO: Confirm max length
		public String TechnicalPricingMethod { get; set; }

        [DisplayName("Bind"), StringLength(10)] // Technical premium "Pre" or "Post". Default "Pre"
		public String TechnicalPricingBindStatus { get; set; }

        [DisplayName("% / Amt"), StringLength(10)] // "%" or "Amt"
		public String TechnicalPricingPremiumPctgAmt { get; set; }

		[DisplayName("Technical Premium")] // TODO: Is this choice between currency and percentage ? (Use "DataType.Currency" ?)
		public Decimal? TechnicalPremium { get; set; }

		[DisplayName("Pricing Currency"), StringLength(10),
		RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "Not a valid Currency")]
		public String Currency { get; set; }

		[DisplayName("Limit Currency"), StringLength(10),
		RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "Not a valid Currency")]
		public String LimitCCY { get; set; }

		[DisplayName("Excess Currency"), StringLength(10),
		RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "Not a valid Currency")]
		public String ExcessCCY { get; set; }

		[DisplayName("Benchmark Premium"), DataType(DataType.Currency)] // TODO: Confirm that DataType.Currency is useful
        public Decimal? BenchmarkPremium { get; set; }

		[DisplayName("Quoted Premium"), DataType(DataType.Currency)] // TODO: Confirm that DataType.Currency is useful
        public Decimal? QuotedPremium { get; set; }

		[DisplayName("Limit Amount"), DataType(DataType.Currency)] // TODO: Confirm that DataType.Currency is useful
        public Decimal? LimitAmount { get; set; }

		[DisplayName("Excess Amount"), DataType(DataType.Currency)] // TODO: Confirm that DataType.Currency is useful
        public Decimal? ExcessAmount { get; set; }

		[DisplayName("Comment"), AllowHtml, DataType(DataType.MultilineText)] // TODO: Confirm max length & really allow HTML ?
        public String Comment { get; set; }

		[DisplayName("Declinature Reason"), StringLength(256)] // TODO: Confirm max length
		public String DeclinatureReason { get; set; }

		[DisplayName("Declinature Comments"), DataType(DataType.MultilineText)] // TODO: Confirm max length
		public String DeclinatureComments { get; set; }

		[Timestamp, DisplayName("Timestamp")]
		public Byte[] Timestamp { get; set; }

		[DisplayName("Subscribe Timestamp")]
		public long? SubscribeTimestamp { get; set; }

    }
}