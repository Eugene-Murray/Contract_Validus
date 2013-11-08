using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.Workflow.Client;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
    public interface IWorkItemBusinessModule
    {
        Object[] GetUserWorklistItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, out Int32 count, out Int32 totalCount);
        Object[] GetUserWorkflowItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, out Int32 count, out Int32 totalCount, List<Tuple<String, String, String>> extraFilters);
        WorklistItem GetWorklistItemBySN(string serialNumber);
        WorkflowItem GetWorkflowItemBySN(string serialNumber);
    }
}
