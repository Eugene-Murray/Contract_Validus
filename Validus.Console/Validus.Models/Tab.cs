using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class Tab : ModelBase
	{
		[DisplayName("Id")]
        public Int32 Id { get; set; }

		[DisplayName("User Id")]
        public Int32 UserId { get; set; }

        [DisplayName("User"), JsonIgnore, ScriptIgnore]
        public User User { get; set; }

        [Required, DisplayName("Url"), DataType(DataType.Url), StringLength(2048)]
		public String Url { get; set; }
    }
}