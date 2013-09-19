using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class QuoteSheet : ModelBase
	{
		[DisplayName("Id")]
        public Int32 Id  { get; set; }

		[Required, DisplayName("Title"), StringLength(256)]
		public String Title { get; set; }

		[Required, DisplayName("FileNet Id")]
		public Guid Guid { get; set; }

		[Required, DisplayName("FileNet Object Store"), StringLength(50)]
        public String ObjectStore { get; set; }

        [DisplayName("Issued Date")]
		public DateTime? IssuedDate { get; set; }

		[DisplayName("Issued By")]
		public User IssuedBy { get; set; }

		[Required, DisplayName("Issued By Id")]
		public Int32 IssuedById { get; set; }

		[DisplayName("Option Versions")]
		public ICollection<OptionVersion> OptionVersions { get; set; }
    }
}