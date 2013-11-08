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
    public class AdditionalInsured : ModelBase
    {
        [DisplayName("Id")]
        public Int32 Id { get; set; }
        [Required, DisplayName("Insured Id")]
        public int InsuredId { get; set; }
        public string InsuredName { get; set; }
        [Required, DisplayName("Insured Type"), StringLength(50)]
        public string InsuredType { get; set; }
    }
}
