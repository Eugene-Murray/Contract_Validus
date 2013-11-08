using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Validus.Console.DTO
{
	public class DocumentDto
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public String ObjectStore { get; set; }

		[Required]
		public String Url { get; set; }

		[Required]
		public String Name { get; set; }

		[Required]
		public String Type { get; set; }

		[Required]
		public String Title { get; set; }

		public String Description { get; set; }

		[Required]
		[DisplayName("Created On")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime CreatedOn { get; set; }

		[Required]
		[DisplayName("Created By")]
		public String CreatedBy { get; set; }
	}
}