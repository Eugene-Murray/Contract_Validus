using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SourceCode.Workflow.Client;
using Validus.Console.DTO;
//using SourceCode.Workflow.Client;

namespace Validus.Console.Data
{
    public interface IWorkItemData
    {
        Worklist GetWorklistItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField,WCSortOrder> sorts, String impersonateUser);

        List<WorkflowItem> GetWorkflowItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, String impersonateUser, out Int32 count, out Int32 totalCount);

        WorklistItem GetWorklistItemBySN(string serialNumber, String impersonateUser);

        WorkflowItem GetWorkflowItemBySN(string serialNumber, String impersonateUser);
    }
}