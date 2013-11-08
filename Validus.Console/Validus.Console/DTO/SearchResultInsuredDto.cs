using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class SearchResultInsuredDto : SearchResultsBaseDto
    {
        public string InsuredName { get; set; }
        public int NoOfRisks { get; set; }
        public string LastYear { get; set; }
        public string FirstYear { get; set; }
        public int NoOfLiveRisks { get; set; }
        public int NoOfOtherRisks { get; set; }
        public string LastLiveYear { get; set; }
        public string FirstLiveYear { get; set; }
        public int? InsuredId { get; set; }
        private const string _View = "_SearchInsured";

        public static SearchResultsBaseDto GetInstance(IDictionary<string, object> item)
        {
            // make sure this is a policy one
            if ((item["VALContentType"] != null) && (item["VALContentType"].ToString().ToLower().Equals("subscribeinsured")))
            {
                var newItem = new SearchResultInsuredDto();

                newItem.InsuredName = (GetItemObject(item, "VALInsdNm") == null) ? null : item["VALInsdNm"].ToString();
                newItem.LastYear = (GetItemObject(item, "VALLastYear") == null) ? null : item["VALLastYear"].ToString();
                newItem.FirstYear = (GetItemObject(item, "VALFirstYear") == null) ? null : item["VALFirstYear"].ToString();
                newItem.LastYear = (GetItemObject(item, "VALLastLiveYear") == null) ? null : item["VALLastLiveYear"].ToString();
                newItem.FirstYear = (GetItemObject(item, "VALFirstLiveYear") == null) ? null : item["VALFirstLiveYear"].ToString();
                newItem.NoOfRisks = (GetItemObject(item, "VALNoOfRisks") == null)
                                  ? 0
                                  : Int32.Parse(item["VALNoOfRisks"].ToString());
                newItem.NoOfLiveRisks = (GetItemObject(item, "VALNoOfLiveRisks") == null)
                                  ? 0
                                  : Int32.Parse(item["VALNoOfLiveRisks"].ToString());
                newItem.NoOfOtherRisks = (GetItemObject(item, "VALNoOfOtherRisks") == null)
                                  ? 0
                                  : Int32.Parse(item["VALNoOfOtherRisks"].ToString());
                newItem.InsuredId = (GetItemObject(item, "VALInsdId") == null)
                                  ? 0
                                  : Int32.Parse(item["VALInsdId"].ToString());
                newItem.HitHightlightSummary = (GetItemObject(item, "HITHIGHLIGHTEDSUMMARY") == null)
                                           ? null
                                           : item["HITHIGHLIGHTEDSUMMARY"].ToString();
                newItem.View = _View;
                return string.IsNullOrEmpty(newItem.InsuredName) ? null : newItem;
            }
            return null;
        }
    }
}