using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class TeamOfficeSetting:ModelBase
    {
        //[Key, DisplayName("Id")]
        //public int  Id { get; set; }
        [Key, Column(Order = 0)]
        public int TeamId { get; set; }
        [Key, Column(Order = 1)]
        public string OfficeId { get; set; }

        [ForeignKey("TeamId")]
        [Required, DisplayName("Team")]
        public Team Team { get; set; }

        [ForeignKey("OfficeId")]
        [Required, DisplayName("Office")]
        public Office Office { get; set; }
        
     
        [DisplayName("Market Wordings Keys")]
        public ICollection<MarketWordingSetting> MarketWordingSettings { get; set; }

        [DisplayName("Terms and Condition Keys")]
        public ICollection<TermsNConditionWordingSetting> TermsNConditionWordingSettings { get; set; }

        [DisplayName("Terms and Condition Keys")]
        public ICollection<SubjectToClauseWordingSetting> SubjectToClauseWordingSettings { get; set; }
     
    }
}
