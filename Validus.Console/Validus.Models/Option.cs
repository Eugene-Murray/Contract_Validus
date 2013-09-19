using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;
using Validus.Models.HTTP;

namespace Validus.Models
{
    [JsonConverter(typeof(OptionConvertor))]
    public class Option : ModelBase 
    {
		[Required, DisplayName("Id")]
		public Int32 Id { get; set; }

		[Required, DisplayName("Submission Id")]
		public Int32 SubmissionId { get; set; }

		[Required, DisplayName("Title"), StringLength(256)]  // TODO: Confirm max length
        public String Title	{ get; set; }

		[DisplayName("Title"), DataType(DataType.MultilineText)]  // TODO: Confirm max length
        public String Comments { get; set; }

        [DisplayName("Options"), ScriptIgnore, JsonIgnore]
        public Submission Submission { get; set; }

       // [ObjectCollectionValidator]
        [DisplayName("Option Versions")]
        public ICollection<OptionVersion> OptionVersions { get; set; }

		[Timestamp, DisplayName("Timestamp")]
		public Byte[] Timestamp { get; set; }
    }
}