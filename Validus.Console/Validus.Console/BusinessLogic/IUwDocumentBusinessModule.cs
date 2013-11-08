using System;
using System.Collections.Generic;
using System.Data;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
	public interface IUwDocumentBusinessModule
	{
		DataTable SearchByPolicyIds(string policyIds);
	}
}