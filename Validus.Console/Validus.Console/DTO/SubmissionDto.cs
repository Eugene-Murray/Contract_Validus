using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Validus.Console.BusinessLogic;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class SubmissionDto
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String InsuredName { get; set; }
        public Int32 InsuredId { get; set; }
        public String BrokerCode { get; set; }
        public String BrokerPseudonym { get; set; }
        public Int32 BrokerSequenceId { get; set; }
        public String BrokerContact { get; set; }
        public String NonLondonBrokerCode { get; set; }
        public String NonLondonBrokerName { get; set; }
        public String UnderwriterCode { get; set; }
        public Underwriter Underwriter { get; set; }
        public String UnderwriterContactCode { get; set; }
        public Underwriter UnderwriterContact { get; set; }
        public String QuotingOfficeId { get; set; }
        public Office QuotingOffice { get; set; }
        public String Domicile { get; set; }
        public String Leader { get; set; }
        public Decimal? Brokerage { get; set; }
        public String QuoteSheetNotes { get; set; }
        public String UnderwriterNotes { get; set; }
        public Byte[] Timestamp { get; set; }
        public ICollection<OptionDto> Options { get; set; }
        public String SubmissionTypeId { get; set; }

        public String ExtraProperty1 { get; set; }
        public String ExtraProperty2 { get; set; }


        #region wording

        public ICollection<MarketWordingSettingDto> SubmissionMarketWordingsList { get; set; }
        public ICollection<MarketWordingSettingDto> CustomSubmissionMarketWordingsList { get; set; }

        public ICollection<TermsNConditionWordingSettingDto> SubmissionTermsNConditionWordingsList { get; set; }
        public ICollection<TermsNConditionWordingSettingDto> CustomSubmissionTermsNConditionWordingsList { get; set; }

        public ICollection<SubjectToClauseWordingSettingDto> SubmissionSubjectToClauseWordingsList { get; set; }
        public ICollection<SubjectToClauseWordingSettingDto> CustomSubmissionSubjectToClauseWordingsList { get; set; }

        #endregion

        // ExampleEnergy
        public String ExampleEnergy_SubExtraProperty1 { get; set; }
        public String ExampleEnergy_SubExtraProperty2 { get; set; }

        // PV
        public String Industry { get; set; }
        public String Situation { get; set; }
        public Decimal? Order { get; set; }
        public Decimal? EstSignPctg { get; set; }

        // AuditTrail
        public List<AuditTrail> AuditTrails { get; set; }
    }
}
