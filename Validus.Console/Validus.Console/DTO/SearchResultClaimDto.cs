using System;
using System.Collections.Generic;

namespace Validus.Console.DTO
{
    public class SearchResultClaimDto : SearchResultsBaseDto
    {
        public string BrokerCode { get; set; }
        public string BrokerContact { get; set; }
        public string BPR { get; set; }
        public string CCY { get; set; }
        public string Leader { get; set; }
        public string Description { get; set; }
        public string DOL { get; set; }
        public string InsuredName { get; set; }
        public string LossLocation { get; set; }
        public Decimal? Paid { get; set; }
        public string PolicyId { get; set; }
        public Decimal? Reserve { get; set; }
        public string Status { get; set; }
        public string Underwriter { get; set; }
        private const string _View = "_SearchClaim";

        public static SearchResultsBaseDto GetInstance(IDictionary<string, object> item)
        {
            // make sure this is a policy one
            if ((item["VALContentType"] != null) && (item["VALContentType"].ToString().ToLower().Equals("claims")))
            {
                var newItem = new SearchResultClaimDto();
                newItem.BrokerContact = (GetItemObject(item, "VALBkrCtc") == null) ? null : item["VALBkrCtc"].ToString();
                newItem.BrokerCode = (GetItemObject(item, "VALBkrCd") == null) ? null : item["VALBkrCd"].ToString();
                newItem.BPR = (GetItemObject(item, "VALBpr") == null) ? null : item["VALBpr"].ToString();
                newItem.CCY = (GetItemObject(item, "VALCcy") == null) ? null : item["VALCcy"].ToString();
                newItem.Leader = (GetItemObject(item, "VALLdr") == null) ? null : item["VALLdr"].ToString();
                newItem.Description = (GetItemObject(item, "VALDescription") == null)
                                          ? null
                                          : item["VALDescription"].ToString();
                newItem.DOL = (GetItemObject(item, "VALDol") == null) ? null : item["VALDol"].ToString();
                newItem.InsuredName = (GetItemObject(item, "VALInsdNm") == null) ? null : item["VALInsdNm"].ToString();
                newItem.LossLocation = (GetItemObject(item, "VALLoccLocn") == null)
                                           ? null
                                           : item["VALLoccLocn"].ToString();
                newItem.PolicyId = (GetItemObject(item, "VALPolID") == null) ? null : item["VALPolID"].ToString();
                newItem.Status = (GetItemObject(item, "VALSt") == null) ? null : item["VALSt"].ToString();
                newItem.Underwriter = (GetItemObject(item, "VALUwrPsu") == null) ? null : item["VALUwrPsu"].ToString();
                newItem.Reserve = (GetItemObject(item, "VALReserve") == null)
                                      ? 0
                                      : Decimal.Parse(item["VALReserve"].ToString());
                newItem.Paid = (GetItemObject(item, "VALPaid") == null) ? 0 : Decimal.Parse(item["VALPaid"].ToString());
                newItem.HitHightlightSummary = (GetItemObject(item, "HITHIGHLIGHTEDSUMMARY") == null)
                                                   ? null
                                                   : item["HITHIGHLIGHTEDSUMMARY"].ToString();
                newItem.View = _View;
                return newItem;
            }
            return null;
        }

    }
}