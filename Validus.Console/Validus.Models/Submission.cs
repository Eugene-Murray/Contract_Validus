using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Validus.Console.DTO;
using Validus.Models.HTTP;

namespace Validus.Models
{
    [JsonConverter(typeof(SubmissonConvertor))]
    public class Submission : ModelBase
    {
		[Required, DisplayName("Id")]
        public Int32 Id { get; set; }

		[Required, DisplayName("Title"), StringLength(100)]
		public String Title { get; set; }

		[Required, DisplayName("Insured Name"), StringLength(100)]
		public String InsuredName { get; set; }

		[Required, DisplayName("Insured Id")]
		public Int32 InsuredId { get; set; }

		[Required, DisplayName("Broker"), StringLength(10),
		RegularExpression(@"^\d+$", ErrorMessage = "Not a valid broker")]
		public String BrokerCode { get; set; }

		[Required, DisplayName("Broker Pseudonym"), StringLength(10),
		RegularExpression(@"^[A-Za-z]{2,4}$", ErrorMessage = "Not a valid broker")]
		public String BrokerPseudonym { get; set; }

		[Required, DisplayName("Broker Sequence Id")]
		public Int32 BrokerSequenceId { get; set; }

		[DisplayName("Broker Contact"), StringLength(50)]
		public String BrokerContact { get; set; }
        
		[DisplayName("Non London Broker Code"), StringLength(10),
        RegularExpression(@"^[\dA-Za-z]{2,}$", ErrorMessage = "Not a valid non-London broker")]
        public String NonLondonBrokerCode { get; set; }

        [DisplayName("Non London Broker Name"), StringLength(100)]
		public String NonLondonBrokerName { get; set; }

		[Required, DisplayName("Underwriter"), StringLength(10),
		RegularExpression(@"^[A-Z]{3,4}$", ErrorMessage = "Not a valid underwriter")]
		public String UnderwriterCode { get; set; }

		[DisplayName("Underwriter")]
		public Underwriter Underwriter { get; set; }

		[Required, DisplayName("Underwriter Contact"), StringLength(10),
		RegularExpression(@"^[A-Z]{3,4}$", ErrorMessage = "Not a valid underwriter contact")]
		public String UnderwriterContactCode { get; set; }

		[DisplayName("Underwriter Contact")]
		public Underwriter UnderwriterContact { get; set; }

		[Required, DisplayName("Quoting Office"), StringLength(10),
		RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Not a valid office")]
		public String QuotingOfficeId { get; set; }

		[DisplayName("Quoting Office")]
		public Office QuotingOffice { get; set; }

		[Required, DisplayName("Domicile"), StringLength(10),
		RegularExpression(@"^[A-Za-z]{2}$")]
		public String Domicile { get; set; }

        [DisplayName("Leader No"), StringLength(4)]
        public string LeaderNo { get; set; }

		[DisplayName("Leader"), StringLength(10),
		RegularExpression(@"^\S{2,4}$", ErrorMessage = "Not a valid leader")]
		public String Leader { get; set; }

		[DisplayName("Brokerage"), 
		Range(0, 100, ErrorMessage = "Not a valid percentage")]
		public Decimal? Brokerage { get; set; }

		[DisplayName("Quote Sheet Notes"), DataType(DataType.MultilineText)] // TODO: Confirm max length
		public String QuoteSheetNotes { get; set; }

		[DisplayName("Underwriter Notes"), DataType(DataType.MultilineText)] // TODO: Confirm max length
		public String UnderwriterNotes { get; set; }

		[Timestamp, DisplayName("Timestamp")]
		public Byte[] Timestamp { get; set; }

        [DisplayName("Options")]
        public ICollection<Option> Options { get; set; }

        [DisplayName("Submission Type Id")]
        public String SubmissionTypeId { get; set; }

        [StringLength(20)]  //  Chose 20 to match subscribe polanlycd
        public String Industry { get; set; }

        [StringLength(20), Display(Name = "Situation (Of Risk)")]  //  Chose 20 to match Subscribe 
        public String Situation { get; set; }

        //  Corresponds to Subscribe WtnOrdPctg?
        [Range(0, 100)]
        public Decimal? Order { get; set; }

        [Display(Name="Est. Signing"), Range(0, 999)] // TODO: 999% ?
        public Decimal? EstSignPctg { get; set; }

        public List<AdditionalInsured> AdditionalInsuredList { get; set; }

        #region wording

        public ICollection<MarketWordingSetting> MarketWordingSettings { get; set; }
        public ICollection<MarketWordingSetting> CustomMarketWordingSettings { get; set; }

        public ICollection<TermsNConditionWordingSetting> TermsNConditionWordingSettings { get; set; }
        public ICollection<TermsNConditionWordingSetting> CustomTermsNConditionWordingSettings { get; set; }

        public ICollection<SubjectToClauseWordingSetting> SubjectToClauseWordingSettings { get; set; }
        public ICollection<SubjectToClauseWordingSetting> CustomSubjectToClauseWordingSettings { get; set; }

        [NotMapped]
        public List<MarketWordingSettingDto> SubmissionMarketWordingsList { get; set; }
        [NotMapped]
        public List<MarketWordingSettingDto> CustomSubmissionMarketWordingsList { get; set; }

        [NotMapped]
        public List<TermsNConditionWordingSettingDto> SubmissionTermsNConditionWordingsList { get; set; }
        [NotMapped]
        public List<TermsNConditionWordingSettingDto> CustomSubmissionTermsNConditionWordingsList { get; set; }

        [NotMapped]
        public List<SubjectToClauseWordingSettingDto> SubmissionSubjectToClauseWordingsList { get; set; }
        [NotMapped]
        public List<SubjectToClauseWordingSettingDto> CustomSubmissionSubjectToClauseWordingsList { get; set; }
        
        #endregion
        // AuditTrail
        [NotMapped]
        public List<AuditTrail> AuditTrails { get; set; }

        [NotMapped]
        public string NewBrokerContactName { get; set; }

        [NotMapped]
        public string NewBrokerContactCompany { get; set; }

        [NotMapped]
        public string NewBrokerContactEmail { get; set; }

        [NotMapped]
        public string NewBrokerContactPhoneNumber { get; set; }
    }
}