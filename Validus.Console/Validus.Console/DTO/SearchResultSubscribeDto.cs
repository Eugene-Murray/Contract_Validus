using System.Collections.Generic;

namespace Validus.Console.DTO
{
    public class SearchResultSubscribeDto : SearchResultsBaseDto
    {
        public string PolicyId { get; set; }
        public string InsuredName { get; set; }
        public string BrokerCode { get; set; }
        public string AccountingYear { get; set; }
        public string COB { get; set; }
        public string Division { get; set; }
        public string EntryStatus { get; set; }
        public string InceptionDate { get; set; }
        public string Leader { get; set; }
        public string OriginatingOffice { get; set; }
        public string Status { get; set; }
        public string Underwriter { get; set; }
        public string WrittenDate { get; set; }
        public string Description { get; set; }
        private const string _View = "_SearchSubscribe";

        public static SearchResultSubscribeDto GetInstance(IDictionary<string, object> item)
        {
            // make sure this is a policy one
            if ((item["VALContentType"] != null) && (item["VALContentType"].ToString().ToLower().Equals("policy")))
            {
                var newItem = new SearchResultSubscribeDto();
                newItem.PolicyId = (GetItemObject(item, "VALPolID") == null) ? null : item["VALPolID"].ToString();
                newItem.InsuredName = (GetItemObject(item, "VALInsdNm") == null) ? null : item["VALInsdNm"].ToString();
                newItem.BrokerCode = (GetItemObject(item, "VALBkrCd") == null) ? null : item["VALBkrCd"].ToString();
                newItem.AccountingYear = (GetItemObject(item, "VALAcctgYr") == null) ? null : item["VALAcctgYr"].ToString();
                newItem.COB = (GetItemObject(item, "VALCob") == null) ? null : item["VALCob"].ToString();
                newItem.Division = (GetItemObject(item, "VALDivision") == null) ? null : item["VALDivision"].ToString();
                newItem.EntryStatus = (GetItemObject(item, "VALEntSt") == null) ? null : item["VALEntSt"].ToString();
                newItem.InceptionDate = (GetItemObject(item, "VALIncpDt") == null) ? null : item["VALIncpDt"].ToString();
                newItem.Leader = (GetItemObject(item, "VALLdr") == null) ? null : item["VALLdr"].ToString();
                newItem.OriginatingOffice = (GetItemObject(item, "VALOrigOff") == null) ? null : item["VALOrigOff"].ToString();
                newItem.Status = (GetItemObject(item, "VALSt") == null) ? null : item["VALSt"].ToString();
                newItem.Underwriter = (GetItemObject(item, "VALUwrPsu") == null) ? null : item["VALUwrPsu"].ToString();
                newItem.HitHightlightSummary = (GetItemObject(item, "HITHIGHLIGHTEDSUMMARY") == null) ? null : item["HITHIGHLIGHTEDSUMMARY"].ToString();
                newItem.Description = (GetItemObject(item, "VALDescription") == null)
                                          ? null
                                          : item["VALDescription"].ToString();
                newItem.View = _View;
                return newItem;
            }
            return null;
        }
    }
}