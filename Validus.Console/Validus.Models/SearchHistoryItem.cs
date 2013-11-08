using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class SearchHistoryItem : ModelBase
	{
		[DisplayName("Id")]
        public Int32 Id { get; set; }

        [Required, DisplayName("Search Term"), StringLength(512)]
        public String SearchString { get; set; }

		[Required, DisplayName("User")]
        public User User { get; set; }

		[Required, DisplayName("Executed On")]
		public DateTime ExecutedAt { get; set; }

		[DisplayName("Created On")]
		public DateTime CreatedOn { get; set; }

		[DisplayName("Modified On")]
		public DateTime ModifiedOn { get; set; }

		[DisplayName("Created On"), StringLength(256)]
		public String CreatedBy { get; set; }

		[DisplayName("Modified On"), StringLength(256)]
		public String ModifiedBy { get; set; }
    }
}