using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class RiskCode : ModelBase
    {
        [Key, Required, StringLength(12)]
        public String Code { get; set; }

        [Required, StringLength(50)]
        public String Name { get; set; }
    }
}