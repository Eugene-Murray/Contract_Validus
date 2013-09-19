using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public abstract class Wording : ModelBase, IVersion
    {
        //Wording Ref Number			Wording Type		Wording Title
        [Key, DisplayName("Id")]
        public int Id { get; set; }
        //Supplied from external system
        [StringLength(256)]
        public string WordingRefNumber { get; set; }
        //Supplied from external system
        [StringLength(15)]
        public string WordingType { get; set; }

        [Required, DisplayName("Statement"), StringLength(256)]
        public string Title { get; set; }

        [Required]
        public int VersionNo { get; set; }

        [Required]
        public Guid Key { get; set; }

        [Required]
        public bool IsObsolete { get; set; }
      
    }
}
