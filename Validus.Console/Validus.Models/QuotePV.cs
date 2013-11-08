using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    [Table("QuotesPV")]
    public class QuotePV : Quote
    {
        [Display(Name = "PD % / Amt"), StringLength(10)]
        public String PDPctgAmt { get; set; }
        
        [Display(Name = "PD Excess Amount"), DataType(DataType.Currency)]
        public Decimal? PDExcessAmount { get; set; }

        [Display(Name = "BI % / Amt / Days"), StringLength(10)]
        public String BIPctgAmtDays { get; set; }

        [Display(Name = "BI Excess Amount"), DataType(DataType.Currency)]
        public Decimal? BIExcessAmount { get; set; }

        [Display(Name = "Line Size"), Range(0,100)]
        public Decimal? LineSize { get; set; }
    }
}