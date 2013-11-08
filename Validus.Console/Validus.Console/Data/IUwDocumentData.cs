using System;
using System.Collections.Generic;
using System.Data;
using Validus.Console.DTO;
using Validus.Console.DocumentManagementService;

namespace Validus.Console.Data
{
	public interface IUwDocumentData
	{
		//List<UwDocumentDto> SearchByPolicyId(String policyId);
		DataTable SearchByPolicyId(String policyId);
	}
}