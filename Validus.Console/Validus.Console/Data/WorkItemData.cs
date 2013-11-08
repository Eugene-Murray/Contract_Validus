using System;
using System.Collections.Generic;
using SourceCode.Workflow.Client;
using Validus.Console.DTO;
using System.Linq;

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
                    //String searchTermFormat = String.Format("% / % / % / % / %{0}%", searchTerm);
                    workCriteria.AddFilterField(WCLogical.StartBracket, WCField.None, WCCompare.Equal, null);           //  (
                    workCriteria.AddFilterField(WCField.ActivityName, WCCompare.Like, searchTermFormat);                //  ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.EventName, WCCompare.Like, searchTermFormat);     //  OR ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.ProcessFolio, WCCompare.Like, searchTermFormat);  //  OR ...
                    workCriteria.AddFilterField(WCLogical.EndBracket, WCField.None, WCCompare.Equal, null);             //  )
                    //workCriteria.AddFilterField(WCField.ProcessFolio, WCCompare.Like, searchTermFormat);
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

        public List<WorkflowItem> GetWorkflowItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, String impersonateUser, out Int32 count, out Int32 totalCount, List<Tuple<String, String, String>> extraFilters)
        {
            Tuple<String, String, String> cobEF = null;
            Tuple<String, String, String> yearEF = null;
            Tuple<String, String, String> descEF = null;
            Tuple<String, String, String> insdEF = null;
            Tuple<String, String, String> bkrEF = null;
            Tuple<String, String, String> uwrEF = null;
            Tuple<String, String, String> actEF = null;             

            if (extraFilters != null)
            {
                cobEF = extraFilters.FirstOrDefault(f => f.Item1 == "COBId");
                yearEF = extraFilters.FirstOrDefault(f => f.Item1 == "AccountYear");
                descEF = extraFilters.FirstOrDefault(f => f.Item1 == "Description");
                insdEF = extraFilters.FirstOrDefault(f => f.Item1 == "InsuredName");
                bkrEF = extraFilters.FirstOrDefault(f => f.Item1 == "BrokerPseudonym");
                uwrEF = extraFilters.FirstOrDefault(f => f.Item1 == "UnderwriterCode");
                actEF = extraFilters.FirstOrDefault(f => f.Item1 == "Activity");
            }
            String cobExtraFilter = cobEF == null ? String.Empty : cobEF.Item3;
            Int32 yearParse;
            Int32? yearExtraFilter = null;
            if (Int32.TryParse(yearEF == null ? String.Empty : yearEF.Item3, out yearParse))
                yearExtraFilter = yearParse;

            String descExtraFilter = descEF == null ? String.Empty : descEF.Item3;
            String insdExtraFilter = insdEF == null ? String.Empty : insdEF.Item3;
            String bkrExtraFilter = bkrEF == null ? String.Empty : bkrEF.Item3;
            String uwrExtraFilter = uwrEF == null ? String.Empty : uwrEF.Item3;
            String actExtraFilter = actEF == null ? String.Empty : actEF.Item3;

            List<WorkflowItem> retArray = new List<WorkflowItem>();
            using (var k2Connection = new Connection())
            {
                ConnectionSetup k2Setup = new ConnectionSetup();
                k2Setup.ConnectionString = Properties.Settings.Default.WorkflowServerConnectionString;

                k2Connection.Open(k2Setup);

                if (!String.IsNullOrEmpty(impersonateUser))
                    k2Connection.ImpersonateUser(impersonateUser);

                var workCriteria = new WorklistCriteria { NoData = true, Platform = "ASP" };
                
                //String propertyFilter = String.Format("% / {0} / {1} / % / %{2}%", uwrExtraFilter, searchTerm);
                String propertyFilter = String.Format("% / {0} / {1}%{2} / % / {3}",
                    String.IsNullOrEmpty(uwrExtraFilter) ? "%" : uwrExtraFilter,
                    cobExtraFilter,
                    yearExtraFilter == null ? String.Empty : yearExtraFilter.ToString().Substring(2),
                    String.IsNullOrEmpty(insdExtraFilter) ? "%" : "%" + insdExtraFilter + "%");
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    String searchTermFormat = String.Format("%{0}%", searchTerm);

                    //if (extraFilters.Count > 0)
                    workCriteria.AddFilterField(WCLogical.StartBracket, WCField.None, WCCompare.Equal, null);           //  (

                    workCriteria.AddFilterField(WCLogical.StartBracket, WCField.None, WCCompare.Equal, null);           //      (
                    workCriteria.AddFilterField(WCField.ActivityName, WCCompare.Like, searchTermFormat);                //          ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.ProcessData, "BPC", WCCompare.Equal, searchTerm); //          OR ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.EventName, WCCompare.Like, searchTermFormat);     //          OR ...
                    workCriteria.AddFilterField(WCLogical.Or, WCField.ProcessFolio, WCCompare.Like, searchTermFormat);  //          OR ...
                    workCriteria.AddFilterField(WCLogical.EndBracket, WCField.None, WCCompare.Equal, null);             //      )     

                    if (extraFilters.Count > 0)
                    {
                        workCriteria.AddFilterField(WCLogical.And, WCField.None, WCCompare.Equal, null);                //      AND
                        workCriteria.AddFilterField(WCLogical.StartBracket, WCField.None, WCCompare.Equal, null);       //      (
                        workCriteria.AddFilterField(WCField.ProcessFolio, WCCompare.Like, propertyFilter);              //          ...
                        workCriteria.AddFilterField(WCLogical.EndBracket, WCField.None, WCCompare.Equal, null);         //      )                        
                    }

                    if (!String.IsNullOrEmpty(actExtraFilter))
                        //  TODO: OR event name?
                        //workCriteria.AddFilterField(WCField.EventName, WCCompare.Equal, actExtraFilter);                     //     ...
                        workCriteria.AddFilterField(WCLogical.And, WCField.ActivityName, WCCompare.Equal, actExtraFilter);                     //     ...

                    //if (extraFilters.Count > 0)
                    workCriteria.AddFilterField(WCLogical.EndBracket, WCField.None, WCCompare.Equal, null);             //  )
                    //workCriteria.AddFilterField(WCField., WCField.None, WCCompare.Equal, null);
                }
                else
                {
                    if (extraFilters.Count > 0)
                    {
                        workCriteria.AddFilterField(WCField.ProcessFolio, WCCompare.Like, propertyFilter);
                        
                    }
                    
                    if (!String.IsNullOrEmpty(actExtraFilter))
                        workCriteria.AddFilterField(WCLogical.And, WCField.ActivityName, WCCompare.Equal , actExtraFilter);                     //     ...
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