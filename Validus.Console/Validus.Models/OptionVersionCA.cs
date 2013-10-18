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
    [Table("OptionVersionsCA")]
    public class OptionVersionCA : OptionVersion
    {
        [Display(Name="TSI Currency"), StringLength(10),
            RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "Not a valid Currency")]
        public String TSICurrency { get; set; }

        [Display(Name = "TSI Amount")]
        public Decimal? TSIAmount { get; set; }
    }
}