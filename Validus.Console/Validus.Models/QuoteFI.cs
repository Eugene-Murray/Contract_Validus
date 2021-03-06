﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    [Table("QuotesFI")]
    public class QuoteFI : Quote
    {
        public QuoteFI()
        {
            this.RiskCodeId = null;
            this.RiskCode = null;
        }
        
        [Display(Name = "Cover/Risk"), StringLength(10), ForeignKey("RiskCode")]
        public String RiskCodeId { get; set; }

        [Display(Name = "Cover/Risk")]
        public RiskCode RiskCode { get; set; }

        [DisplayName("Amt / OPL")]
        public string AmountOrOPL { get; set; }

        [DisplayName("% / Amt / % ONP")]
        public string AmountOrONP { get; set; }

        [Display(Name = "Line Size")]
        public Decimal? LineSize { get; set; }

        [DisplayName("% / Amt"), StringLength(10)] // "%" or "Amt"
        public String LineSizePctgAmt { get; set; }

        [Display(Name = "Line is 'To Stand'")]
        public Boolean LineToStand { get; set; }

        [Display(Name = "Is Reinstatement")]
        public Boolean IsReinstatement { get; set; }
    }
}