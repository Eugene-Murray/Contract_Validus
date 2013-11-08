using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class Underwriter : ModelBase
    {
        [Key, DisplayName("Code"), StringLength(10)]
        public String Code { get; set; }

		[DisplayName("Name"), StringLength(256)]
		public String Name { get; set; }
    }
}