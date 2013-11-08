using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    [Table("OptionsFI")]
    public class OptionFI : Option 
    {
        [DisplayName("Risk Codes")]
        public String RiskCodes { get; set; }
    }
}