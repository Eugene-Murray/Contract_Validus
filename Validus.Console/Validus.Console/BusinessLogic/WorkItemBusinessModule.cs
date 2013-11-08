using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SourceCode.Workflow.Client;
using Validus.Console.Data;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
    public class WorkItemBusinessModule : IWorkItemBusinessModule
    {
        IWorkItemData r = null;
        public WorkItemBusinessModule(IWorkItemData rep)
        {
            r = rep;
        }

        public Object[] GetUserWorklistItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, out Int32 count, out Int32 totalCount)
        {
            List<Object> o = new List<Object>();

            Worklist wl = r.GetWorklistItems(searchTerm, skip, take, sorts, HttpContext.Current.User.Identity.Name);
            
            foreach (WorklistItem a in wl)
            {
                DateTime StartDate;
                try
                {
                    if ((a.ActivityInstanceDestination.DataFields["SLAStartDate"].Value != null) &&
                        (a.ActivityInstanceDestination.DataFields["SLAStartDate"].ValueType == DataType.TypeDate))
                    {
                        StartDate = (DateTime)a.ActivityInstanceDestination.DataFields["SLAStartDate"].Value;
                    }
                    else
                        StartDate = a.ActivityInstanceDestination.StartDate;
                }
                catch
                {
                    StartDate = a.ActivityInstanceDestination.StartDate;
                }
                // get the information from the folio
                string folio = a.ProcessInstance.Folio;
                string[] foliofields = folio.Split('/');
                string PolicyID = string.Empty;
                string Insured = string.Empty;
                if (foliofields.Length > 2) PolicyID = foliofields[2].Trim();
                if (foliofields.Length > 4) Insured = foliofields[4].Trim();

                o.Add(new { a.SerialNumber, a.ActivityInstanceDestination.Name, a.ProcessInstance.Folio, PolicyID, Insured, StartDate, a.AllocatedUser });
            }

            //  This number represents a count taking into account a datatables filter. 
            //  Not using this kind of filter?, so will always be total?
            count = wl.TotalCount;
            //  Total records before filter
            totalCount = wl.TotalCount; 
            
            return o.ToArray();
        }

        public Object[] GetUserWorkflowItems(String searchTerm, Int32 skip, Int32 take, Dictionary<WCField, WCSortOrder> sorts, out Int32 count, out Int32 totalCount, List<Tuple<String, String, String>> extraFilters)
        {
            List<WorkflowItem> o = new List<WorkflowItem>();

            o = r.GetWorkflowItems(searchTerm, skip, take, sorts, HttpContext.Current.User.Identity.Name, out count, out totalCount, extraFilters);
            
            return o.ToArray();
        }

        public WorklistItem GetWorklistItemBySN(string serialNumber)
        {
            return r.GetWorklistItemBySN(serialNumber, HttpContext.Current.User.Identity.Name);
        }

        public WorkflowItem GetWorkflowItemBySN(string serialNumber)
        {
            return r.GetWorkflowItemBySN(serialNumber, HttpContext.Current.User.Identity.Name);
        }
    }
}