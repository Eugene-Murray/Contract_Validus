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
    public class ModelBase : IAudit
    {
        [DisplayName("Created On")]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("Modified On")]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName("Created On"), StringLength(256)]
        public String CreatedBy { get; set; }

        [DisplayName("Modified On"), StringLength(256)]
        public String ModifiedBy { get; set; }
    }
}
