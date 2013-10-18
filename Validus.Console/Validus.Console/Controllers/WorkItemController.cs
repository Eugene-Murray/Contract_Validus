using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SourceCode.Workflow.Client;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;

namespace Validus.Console.Controllers
{
    [Authorize(Roles = @"ConsoleRead")]
    public class WorkItemController : Controller
    {
        private readonly IWorkItemBusinessModule _bm;

        public WorkItemController(IWorkItemBusinessModule bm)
        {
            this._bm = bm;
        }

        //
        // GET: /WorkItem/_IndexJSON
        public ActionResult _IndexJSON(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0, 
            Int32 iDisplayLength = 10, 
            Int32 iDisplayStart = 0)            
        {
            Int32 iTotalRecords;
            Int32 iTotalDisplayRecords;

            WCSortOrder sortOrder = (String.IsNullOrEmpty(sSortDir_0) ? WCSortOrder.Descending : (String.Equals(sSortDir_0, "ASC", StringComparison.OrdinalIgnoreCase) ? WCSortOrder.Ascending : WCSortOrder.Descending));
            WCField sortColumn;

            var sorts = new Dictionary<WCField, WCSortOrder>();

            //  TODO - improve sorting. Just use enum all the way through the layers
            //  Or lookup enum value from string?
            //  iSortCol_0 currently comes from the order of columns in the UI I think. This looks a bit fragile - is DataTables the best grid?
            switch (iSortCol_0)
            {
                case 0:
                    sortColumn = WCField.ProcessName;
                    break;
                case 1:
                    sortColumn = WCField.ProcessFolio;
                    break;
                case 2:
                    sortColumn = WCField.EventStartDate;
                    break;
                default:
                    sortColumn = WCField.EventPriority;
                    break;
            }

            sorts.Add(sortColumn, sortOrder);

			// TODO: Exception handling
			Object[] aaData = this._bm.GetUserWorklistItems(sSearch, iDisplayStart, iDisplayLength, sorts, out iTotalDisplayRecords, out iTotalRecords);

            return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "NoCacheProfile")]
        public async Task<ActionResult> GetUserWorkflowItems(String sSearch, Int32 sEcho, Int32 iSortCol_0, String sSortDir_0,
            Int32 iDisplayLength = 10,
            Int32 iDisplayStart = 0)
        {
            Int32 iTotalRecords;
            Int32 iTotalDisplayRecords;

            WCSortOrder sortOrder = (String.IsNullOrEmpty(sSortDir_0) ? WCSortOrder.Descending : (String.Equals(sSortDir_0, "ASC", StringComparison.OrdinalIgnoreCase) ? WCSortOrder.Ascending : WCSortOrder.Descending));
            WCField sortColumn;

            var sorts = new Dictionary<WCField, WCSortOrder>();

            //  iSortCol_0 currently comes from the order of columns in the UI I think. This looks a bit fragile - is DataTables the best grid?
            //  This section needs rework as some columns are now coming from process data.
            switch (iSortCol_0)
            {
                case 0:
                    sortColumn = WCField.ActivityStartDate; // Not same as SLAStartDate - needs work.
                    break;
                case 1:
                    sortColumn = WCField.ProcessFolio;  // Add sorting by policy Id
                    break;
                case 2:
                    sortColumn = WCField.ActivityName;
                    break;
                default:
                    sortColumn = WCField.EventPriority;
                    break;
            }

            sorts.Add(sortColumn, sortOrder);

			// TODO: Exception handling
			Object[] aaData =  this._bm.GetUserWorkflowItems(sSearch, iDisplayStart, iDisplayLength, sorts, out iTotalDisplayRecords, out iTotalRecords);

            return Json(new { sEcho, iTotalRecords, iTotalDisplayRecords, aaData }, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _Preview(String Id)
		{
			WorkflowItem w = this._bm.GetWorkflowItemBySN(Id);

            return PartialView(w);
        }

        //
		// GET: /WorkItem/_WorkflowTasksDetailed
		[OutputCache(CacheProfile = "NoCacheProfile")]
        public ActionResult _WorkflowTasksDetailed()
        {
            return PartialView();
        }
    }
}
