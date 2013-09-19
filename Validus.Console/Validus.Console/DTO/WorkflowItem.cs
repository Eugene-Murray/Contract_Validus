using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using SourceCode.Workflow.Client;

namespace Validus.Console.DTO
{
    public enum WorfklowItemType
    {
        Normal,
        ContractCertainty
    }

    public enum WorkflowItemStatus
    {
        Available,
        Allocated,
        Open
    }

    public class WorkflowItem
    {
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("Policy ID")]
        public string PolicyID { get; set; }

        public string Insured { get; set; }
        public string UWR { get; set; }
        public string Office { get; set; }
        public string Activity { get; set; }

        public string TaskURL { get; set; }

        [DisplayName("Allocated User")]
        public string AllocatedUser { get; set; }
        public string SerialNumber { get; set; }
        public string BPC { get; set; }
        public string TaskId { get; set; }
        public int ProcInstId { get; set; }

        [DisplayName("Resubmission")]
        public bool IsResubmission { get; set; }

        [DisplayName("Urgent")]
        public bool IsUrgent { get; set; }

        [DisplayName("Foreign")]
        public bool IsForeign { get; set; }

        [DisplayName("Risk Based Peer Review")]
        public bool IsRiskBasedPeerReview { get; set; }

        [DisplayName("Query Responded")]
        public bool HasQueryResponse { get; set; }

        [DisplayName("Release")]
        public bool CanRelease { get; set; }

        public WorfklowItemType WorkflowType { get; set; }
        public WorkflowItemStatus Status { get; set; }

        [DisplayName("No of Related Documents")]
        public int RelatedDocumentCount { get; set; }

        public string WebPolicyURL 
        {
            get
            {
                return ConfigurationManager.AppSettings["WebPolicyURL"] + PolicyID;
            }
        }

        public string ViewflowURL
        {
            get
            {
                return ConfigurationManager.AppSettings["ViewflowURL"] + ProcInstId;
            }
        }

        public bool CanOpen
        {
            get
            {
                if ((Status == WorkflowItemStatus.Available) || (Status == WorkflowItemStatus.Open))
                    return true;
                if (Status == WorkflowItemStatus.Allocated)
                {
                    if (AllocatedUser.ToLower().Equals(HttpContext.Current.User.Identity.Name.ToLower()))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }


        public WorkflowItem() { }

        public WorkflowItem(WorklistItem wlItem)
        {
            DateTime startDate = wlItem.ActivityInstanceDestination.StartDate;
            try
            {
                if (wlItem.ActivityInstanceDestination.DataFields.Count != 0)
                {
                    if ((wlItem.ActivityInstanceDestination.DataFields["SLAStartDate"].Value != null) &&
                        (wlItem.ActivityInstanceDestination.DataFields["SLAStartDate"].ValueType == DataType.TypeDate))
                    {
                        startDate = (DateTime)wlItem.ActivityInstanceDestination.DataFields["SLAStartDate"].Value;
                    }
                }
            }
            catch
            {
                //  Gulp!
            }

            // get the data from the worklist
            StartDate = startDate;
            Activity = wlItem.ActivityInstanceDestination.Name;
            TaskURL = wlItem.Data;
            SerialNumber = wlItem.SerialNumber;
            ProcInstId = wlItem.ProcessInstance.ID;
            AllocatedUser = string.Empty;
            CanRelease = false;
            IsForeign = false;
            IsResubmission = false;
            IsUrgent = false;
            WorkflowType = WorfklowItemType.Normal;

            // get the status for the item
            switch (wlItem.Status)
            {
                case WorklistStatus.Available:
                    {
                        AllocatedUser = wlItem.AllocatedUser.Substring(wlItem.AllocatedUser.IndexOf(":", StringComparison.Ordinal) + 1);
                        if (!string.IsNullOrEmpty(AllocatedUser))
                            AllocatedUser = string.Empty;

                        Status = WorkflowItemStatus.Available;
                    }
                    break;
                case WorklistStatus.Allocated:
                    {
                        AllocatedUser = wlItem.AllocatedUser.Substring(wlItem.AllocatedUser.IndexOf(":", StringComparison.Ordinal) + 1);
                        Status = WorkflowItemStatus.Allocated;
                        if (!string.IsNullOrEmpty(AllocatedUser))
                        {
                            AllocatedUser = AllocatedUser;
                        }

                    } break;
                case WorklistStatus.Open:
                    {
                        Status = WorkflowItemStatus.Open;
                        CanRelease = true;
                        AllocatedUser = wlItem.AllocatedUser.Substring(wlItem.AllocatedUser.IndexOf(":", StringComparison.Ordinal) + 1);
                        if (!string.IsNullOrEmpty(AllocatedUser))
                            AllocatedUser = AllocatedUser;
                    } break;
            }

            // get the information from the folio
            string folio = wlItem.ProcessInstance.Folio;
            string[] foliofields = folio.Split('/');

            if (foliofields.Length > 1) UWR = foliofields[1].Trim();
            if (foliofields.Length > 2) PolicyID = foliofields[2].Trim();
            if (foliofields.Length > 4) Insured = foliofields[4].Trim();

            if (IsExternalWordings(Activity))
            {
                WorkflowType = WorfklowItemType.ContractCertainty;
                if (foliofields.Length > 5) IsResubmission = (foliofields[5].Trim().ToLower() == "true");
                if (foliofields.Length > 6) IsUrgent = (foliofields[6].Trim().ToLower() == "true");
                if (foliofields.Length > 7) IsForeign = (foliofields[7].Trim().ToLower() == "true");
                BPC = (foliofields.Length > 3) ? foliofields[3].Trim() : String.Empty;
            }
            else
            {
                Office = (foliofields.Length > 3) ? foliofields[3].Trim() : String.Empty;
            }

            // get the Query Response and Risk Based Peer Review 
            string riskBasedPeerReview = GetDataFieldValue(wlItem, "NeedsUROReview");
            string queryRespondedOS = GetDataFieldValue(wlItem, "OS PR Query Responded");
            string queryRespondedPR = GetDataFieldValue(wlItem, "PR Query Responded");
            string queryRespondedURO = GetDataFieldValue(wlItem, "URO Query Responded");

            if (!string.IsNullOrEmpty(queryRespondedOS) && queryRespondedOS.ToLower() == "true")
                HasQueryResponse = true;
            else if (!string.IsNullOrEmpty(queryRespondedPR) && queryRespondedPR.ToLower() == "true")
                HasQueryResponse = true;
            else if (!string.IsNullOrEmpty(queryRespondedURO) && queryRespondedURO.ToLower() == "true")
                HasQueryResponse = true;
            else
                HasQueryResponse = false;

            if (!string.IsNullOrEmpty(riskBasedPeerReview) && riskBasedPeerReview.ToLower() == "true")
                IsRiskBasedPeerReview = true;
            else
                IsRiskBasedPeerReview = false;
        }

        #region Helper Static Methods

        internal static bool IsExternalWordings(string activityName)
        {
            return ConfigurationManager.AppSettings["EWTasks"].ToLower().Contains(activityName.ToLower());
        }

        /// <summary>
        /// Returns the value of the WorklistItem value for the provided field.
        /// </summary>
        /// <param name="selectedWorklistItem">Opened K2 Worklist reference</param>
        /// <param name="fieldName"></param>
        /// <returns>Value of K2 Process Data field </returns>
        public static string GetDataFieldValue(WorklistItem selectedWorklistItem, string fieldName)
        {
            return (from DataField item in selectedWorklistItem.ProcessInstance.DataFields
                    where item.Name.ToUpper() == fieldName.ToUpper()
                    select item).Count() == 1 ? selectedWorklistItem.ProcessInstance.DataFields[fieldName].Value.ToString() : string.Empty;
        }
        #endregion
    }
}