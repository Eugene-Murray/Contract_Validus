using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class Office : ModelBase
    {
		[DisplayName("Id"), StringLength(256)]
        public String Id { get; set; }

        [Required, StringLength(256)]
        public String Name { get; set; }

        [Required, DisplayName("Title"), StringLength(256)]
        public String Title { get; set; }

        [DisplayName("Address")]
        public Address Address { get; set; }

        [DisplayName("Footer"), StringLength(256)]
		public String Footer { get; set; }
    }
}