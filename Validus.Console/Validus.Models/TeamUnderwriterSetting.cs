using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validus.Models
{
    public class TeamUnderwriterSetting : ModelBase
    {
        [Key, Column(Order = 0)]
        public int TeamId { get; set; }

        [Key, Column(Order = 1)]
        public string UnderwriterCode { get; set; }

        [ForeignKey("TeamId")]
        [DisplayName("Team")]
        public Team Team { get; set; }

        [ForeignKey("UnderwriterCode")]
        [DisplayName("Underwriter")]
        public Underwriter Underwriter { get; set; }

        [DisplayName("Signature "), StringLength(256)]
        public string Signature { get; set; }
    }
}
