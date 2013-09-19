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
    public class QuoteTemplate : ModelBase
    {
        [DisplayName("Id")]
        public Int32 Id { get; set; }

        [DisplayName("Name"), StringLength(20)]
        public String Name { get; set; }

        [DisplayName("RdlPath"), StringLength(256)]
        public String RdlPath { get; set; }
    }
}
