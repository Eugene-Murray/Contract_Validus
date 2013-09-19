using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validus.Models
{
    public class AuditTrail : ModelBase
    {
        [DisplayName("Id")]
        public Int32 Id { get; set; }

        [DisplayName("Source"), StringLength(20)]
        public string Source { get; set; }

        [DisplayName("Reference"), StringLength(20)]
        public string Reference { get; set; }

        [DisplayName("Title"), StringLength(256)]
        public string Title { get; set; }

        [DisplayName("Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}
