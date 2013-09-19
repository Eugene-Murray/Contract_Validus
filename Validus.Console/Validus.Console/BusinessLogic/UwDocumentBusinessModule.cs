using System;
using System.Data;
using Validus.Console.Data;

namespace Validus.Console.BusinessLogic
{
	public class UwDocumentBusinessModule : IUwDocumentBusinessModule
	{
		private readonly IUwDocumentData _uwDocumentData;

		public UwDocumentBusinessModule(IUwDocumentData uwDocumentData)
		{
			this._uwDocumentData = uwDocumentData;
		}

		public DataTable SearchByPolicyIds(string policyIds)
		{
			DataTable uwDocumentsTable = null;

			if (string.IsNullOrEmpty(policyIds) == false)
			{
				var policyIdList = policyIds.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var policyId in policyIdList)
				{
					var searchResultTable = this._uwDocumentData.SearchByPolicyId(policyId);

					if (uwDocumentsTable == null)
					{
						uwDocumentsTable = searchResultTable;
					}
					else
					{
						foreach (DataRow searchResultRow in searchResultTable.Rows)
						{
							uwDocumentsTable.ImportRow(searchResultRow);
						}
					}
				}
			}

			return uwDocumentsTable;
		}
	}
}