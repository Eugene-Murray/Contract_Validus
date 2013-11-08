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
    public class SubmissionType : ModelBase
    {
        [Required, DisplayName("Id")]
        public String Id { get; set; }

        [Required, DisplayName("Title"), StringLength(100)]
        public String Title { get; set; }
    }
}
