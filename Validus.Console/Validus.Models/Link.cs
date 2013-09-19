using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class Link : ModelBase
	{
		[DisplayName("Id")]
        public Int32 Id { get; set; }

		[Required, DisplayName("Url"), DataType(DataType.Url), StringLength(2048)]
        public String Url { get; set; }

		[Required, DisplayName("Title"), StringLength(256)]
        public String Title { get; set; }

		[DisplayName("Category"), StringLength(256)]
        public String Category { get; set; }

		[DisplayName("Teams"), ScriptIgnore, JsonIgnore]
		public ICollection<Team> Teams { get; set; }
    }
}