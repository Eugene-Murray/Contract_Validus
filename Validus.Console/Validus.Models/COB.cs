using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class COB : ModelBase
    {
		[DisplayName("Id"), StringLength(10)]
        public String Id { get; set; }

		[Required, DisplayName("Narrative"), StringLength(100)]
		public String Narrative { get; set; }
    }
}