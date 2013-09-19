using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Validus.Console.DTO
{
	public class UwDocumentDto : DocumentDto
	{
		[DisplayName("Policy Id's")]
		public List<String> PolicyIds { get; set; }

		[DisplayName("Document Type")]
		public String DocumentType { get; set; }

		[DisplayName("Insured")]
		public String InsuredName { get; set; }

		public String Broker { get; set; }

		[DisplayName("Uwr")]
		public String Underwriter { get; set; }
	}
}