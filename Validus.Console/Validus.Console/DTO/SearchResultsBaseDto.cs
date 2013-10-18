using System.Collections.Generic;

namespace Validus.Console.DTO
{
    public abstract class SearchResultsBaseDto
    {
        public string HitHightlightSummary { get; set; }
        public string View { get; set; }

        public bool HasHitHighlightSummary
        {
            get { return !string.IsNullOrEmpty(HitHightlightSummary); }
        }

        public string HitHighlightSummaryHtml 
        {
            get 
            { 
                string retVal = HitHightlightSummary;
                retVal = retVal.Replace("<c0>", "<strong>");
                retVal = retVal.Replace("</c0>", "</strong>");
                retVal = retVal.Replace("<ddd />", "... ");
                retVal = retVal.Replace("<ddd/>", "... ");
                return retVal;
            }
        }

        public static object GetItemObject(IDictionary<string, object> item, string name)
        {
            return (item.ContainsKey(name) ? item[name] : null);
        }
    }
}