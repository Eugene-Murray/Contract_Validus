using System;
using System.Data;
using Validus.Console.DocumentManagementService;

namespace Validus.Console.Data
{
	public class UwDocumentData : IUwDocumentData
	{
		public DataTable SearchByPolicyId(String policyId)
		{
			using (var dmsService = new DMSService())
			{
				return dmsService.GetUWDocumentsByObject(new UWDocument
					{
						RowLimit = 0,
						IsDetailed = true,
						PolicyID = policyId
					});
			}
		}
	}
}