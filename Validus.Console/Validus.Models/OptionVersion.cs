using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;
using Validus.Models.HTTP;

namespace Validus.Models
{
    [JsonConverter(typeof(OptionVersionConvertor))]
    public class OptionVersion : ModelBase
    {
        [Required, Key, DisplayName("Option Id"), Column(Order = 0)]
        public Int32 OptionId { get; set; }

		[Required, Key, DisplayName("Version Number"), Column(Order = 1)]
        public Int32 VersionNumber { get; set; }

		[Required, DisplayName("Title"), StringLength(256)]
        public String Title	{ get; set; }
		
		[DisplayName("Comments"), DataType(DataType.MultilineText)]
        public String Comments { get; set; }

		[DisplayName("Option"), ScriptIgnore, JsonIgnore]
        public Option Option { get; set; }

        [DisplayName("Is Experiment?")]
        public Boolean IsExperiment	{ get; set; }

        [DisplayName("Is Locked?")]
		public Boolean IsLocked { get; set; }

        //[ObjectCollectionValidator]
        [DisplayName("Quotes")]
        public ICollection<Quote> Quotes { get; set; }

        //[ObjectCollectionValidator]
        [DisplayName("Quote Sheets")]
        public ICollection<QuoteSheet> QuoteSheets { get; set; }

		[Timestamp, DisplayName("Timestamp")]
		public Byte[] Timestamp { get; set; }
    }
}