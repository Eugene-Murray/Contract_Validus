using System;
using System.Collections.Generic;

namespace Validus.Console.DTO
{
    public class SearchResultBrokerDto : SearchResultsBaseDto
    {
        public string BrokerCode { get; set; }
        public string BrokerContact { get; set; }
        public string BrokerGroupCode { get; set; }
        public string BrokerName { get; set; }
        public string BrokerPSU { get; set; }
        public int? BrokerSeqId { get; set; }
        private const string _View = "_SearchBroker";

        public static SearchResultsBaseDto GetInstance(IDictionary<string, object> item)
        {
            // make sure this is a policy one
            if ((item["VALContentType"] != null) && (item["VALContentType"].ToString().ToLower().Equals("subscribebroker")))
            {
                var newItem = new SearchResultBrokerDto();

                newItem.BrokerContact = (GetItemObject(item, "VALBkrCtc") == null) ? null : item["VALBkrCtc"].ToString();
                newItem.BrokerGroupCode = (GetItemObject(item, "VALBkrGrpCd") == null) ? null : item["VALBkrGrpCd"].ToString();
                newItem.BrokerCode = (GetItemObject(item, "VALBkrCd") == null) ? null : item["VALBkrCd"].ToString();
                newItem.BrokerName = (GetItemObject(item, "VALBkrNm") == null) ? null : item["VALBkrNm"].ToString();
                newItem.BrokerPSU = (GetItemObject(item, "VALBkrPsu") == null) ? null : item["VALBkrPsu"].ToString();
                newItem.BrokerSeqId = (GetItemObject(item, "VALBkrSeqId") == null)
                                  ? 0
                                  : Int32.Parse(item["VALBkrSeqId"].ToString());
                newItem.HitHightlightSummary = (GetItemObject(item, "HITHIGHLIGHTEDSUMMARY") == null)
                                           ? null
                                           : item["HITHIGHLIGHTEDSUMMARY"].ToString();
                newItem.View = _View;
                if ((string.IsNullOrEmpty(newItem.BrokerName)) || (string.IsNullOrEmpty(newItem.BrokerCode)))
                    return null;
                return newItem;
            }
            return null;
        }
    }
}