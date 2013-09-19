using System;
using System.Collections.Generic;
using SourceCode.Workflow.Client;
using Validus.Console.DTO;

namespace Validus.Console.Data
{
    public class WorkItemData : IWorkItemData
    {
        public Worklist GetWorklistItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, String impersonateUser)
        {
            using (var k2Connection = new Connection())
            {
                ConnectionSetup k2Setup = new ConnectionSetup();
                k2Setup.ConnectionString = Properties.Settings.Default.WorkflowServerConnectionString;

                k2Connection.Open(k2Setup);

                if (!String.IsNullOrEmpty(impersonateUser))
                    k2Connection.ImpersonateUser("K2:" + impersonateUser);

                var workCriteria = new WorklistCriteria { NoData = true, Platform = "ASP" };                                                
                                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    String searchTermFormat = String.Format("%{0}%", searchTerm);
                    workCriteria.AddFilterField(WCLogical.StartBracket, WCField.None, WCCompare.Equal, null);           //  (
                    workCriteria.AddFilterField(WCField.ActivityName, WCCompare.Like, searchTermFormat);                //  ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.EventName, WCCompare.Like, searchTermFormat);     //  OR ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.ProcessFolio, WCCompare.Like, searchTermFormat);  //  OR ...
                    workCriteria.AddFilterField(WCLogical.EndBracket, WCField.None, WCCompare.Equal, null);             //  )
                }

                //  No AND required - seems like bug - this bit gets put in a different bit of the query K2 creates.
                //  Hide allocated items like the SharePoint K2 worlist does by default.
                workCriteria.AddFilterField(WCField.WorklistItemStatus, WCCompare.NotEqual, WorklistStatus.Allocated);                 

                workCriteria.StartIndex = skip;
                workCriteria.Count = take;                
                
                foreach (KeyValuePair<WCField,WCSortOrder> sort in sorts)
                {
                    workCriteria.AddSortField(sort.Key, sort.Value);                                        
                }

                Worklist k2Worklist = k2Connection.OpenWorklist(workCriteria);      

                return k2Worklist;
            }
        }

        public List<WorkflowItem> GetWorkflowItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, String impersonateUser, out Int32 count, out Int32 totalCount)
        {
            List<WorkflowItem> retArray = new List<WorkflowItem>();
            using (var k2Connection = new Connection())
            {
                ConnectionSetup k2Setup = new ConnectionSetup();
                k2Setup.ConnectionString = Properties.Settings.Default.WorkflowServerConnectionString;

                k2Connection.Open(k2Setup);

                if (!String.IsNullOrEmpty(impersonateUser))
                    k2Connection.ImpersonateUser(impersonateUser);

                var workCriteria = new WorklistCriteria { NoData = true, Platform = "ASP" };        

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    String searchTermFormat = String.Format("%{0}%", searchTerm);
                    workCriteria.AddFilterField(WCLogical.StartBracket, WCField.None, WCCompare.Equal, null);           //  (
                    workCriteria.AddFilterField(WCField.ActivityName, WCCompare.Like, searchTermFormat);                //  ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.ProcessData, "BPC", WCCompare.Equal, searchTerm);
                    workCriteria.AddFilterField(WCLogical.Or, WCField.EventName, WCCompare.Like, searchTermFormat);     //  OR ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.ProcessFolio, WCCompare.Like, searchTermFormat);  //  OR ...
                    workCriteria.AddFilterField(WCLogical.EndBracket, WCField.None, WCCompare.Equal, null);             //  )
                }

                //  No AND required - seems like bug - this bit gets put in a different bit of the query K2 creates.
                //  Hide allocated items like the SharePoint K2 worlist does by default.
                //  workCriteria.AddFilterField(WCField.WorklistItemStatus, WCCompare.NotEqual, WorklistStatus.Allocated);

                //workCriteria.AddSortField(WCField.ActivityStartDate, WCSortOrder.Descending);

                workCriteria.StartIndex = skip;
                workCriteria.Count = take;

                foreach (KeyValuePair<WCField, WCSortOrder> sort in sorts)
                {
                    workCriteria.AddSortField(sort.Key, sort.Value);
                }

                Worklist k2Worklist = k2Connection.OpenWorklist(workCriteria);

                foreach (WorklistItem item in k2Worklist)
                {
                    retArray.Add(new WorkflowItem(item));
                }

                count = k2Worklist.TotalCount;
                //  Total records before filter
                totalCount = k2Worklist.TotalCount; 

                return retArray;
            }
        }

        public WorklistItem GetWorklistItemBySN(string serialNumber, String impersonateUser)
        {
            using (var k2Connection = new Connection())
            {
                ConnectionSetup k2Setup = new ConnectionSetup();
                k2Setup.ConnectionString = Properties.Settings.Default.WorkflowServerConnectionString;

                k2Connection.Open(k2Setup);

                if (!String.IsNullOrEmpty(impersonateUser))
                    k2Connection.ImpersonateUser(impersonateUser);

                return k2Connection.OpenWorklistItem(serialNumber);
            }           
        }

        public WorkflowItem GetWorkflowItemBySN(string serialNumber, String impersonateUser)
        {
            using (var k2Connection = new Connection())
            {
                ConnectionSetup k2Setup = new ConnectionSetup();
                k2Setup.ConnectionString = Properties.Settings.Default.WorkflowServerConnectionString;

                k2Connection.Open(k2Setup);

                if (!String.IsNullOrEmpty(impersonateUser))
                    k2Connection.ImpersonateUser(impersonateUser);

                var workCriteria = new WorklistCriteria { NoData = true, Platform = "ASP" };

                if (!string.IsNullOrEmpty(serialNumber))
                {
                    workCriteria.AddFilterField(WCField.SerialNumber, WCCompare.Equal, serialNumber);        
                }
                
                Worklist k2Worklist = k2Connection.OpenWorklist(workCriteria);

                WorkflowItem retItem = null;

                foreach (WorklistItem item in k2Worklist)
                {
                    retItem = new WorkflowItem(item);
                }

                // now that we have the workflow item get the count of the related documents
                if ((retItem!=null)&&(!string.IsNullOrEmpty(retItem.PolicyID)))
                {
                    using (DocumentService.DocumentServiceClient client = new DocumentService.DocumentServiceClient())
                    {
                        retItem.RelatedDocumentCount = client.GetDocumentsCountForPolicy(retItem.PolicyID);
                    }
                }

                return retItem;

            }
        }
    }
}