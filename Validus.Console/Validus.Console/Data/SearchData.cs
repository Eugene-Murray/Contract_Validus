using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using Validus.Console.DTO;
using Validus.Console.Properties;

namespace Validus.Console.Data
{
    public class SearchData : ISearchData
    {
        private static string GetContentSourcesSearchCondition(string contentSources)
        {
            var contentSourcesSearchConditions = string.Empty;
            var sources = contentSources.Split(',');

            foreach (var source in sources)
            {
                if (contentSourcesSearchConditions.Length > 0)
                    contentSourcesSearchConditions += " OR ";
                contentSourcesSearchConditions += Settings.Default.SP2013ContentSourceName + "=\"" + source + "\"";
            }

            if (contentSourcesSearchConditions.Length > 0)
                contentSourcesSearchConditions = " AND (" + contentSourcesSearchConditions + ")";

            return contentSourcesSearchConditions;
        }

        public SearchResponseDto GetSearchResults(string searchTerm, int skip, int take)
        {
            SearchResponseDto retVal = null;

            using (var ctx = new ClientContext(Settings.Default.SP2013SearchUrl))
            {
                // get the content sources for the query
                var contentSources = string.Empty;
                if (!string.IsNullOrEmpty(Settings.Default.SP2013ContentSources))
                {
                    contentSources = GetContentSourcesSearchCondition(Settings.Default.SP2013ContentSources);
                }

                var query = new KeywordQuery(ctx)
                {
	                QueryText = searchTerm + contentSources,
	                StartRow = skip,
	                RowLimit = take
                };
	            var returnProps = Settings.Default.SP2013Properties.Split(',');

                foreach (var prop in returnProps)
                {
                    query.SelectProperties.Add(prop);
                }

                var executor = new SearchExecutor(ctx);

                var results = executor.ExecuteQuery(query);

                ctx.ExecuteQuery();

                if (results.Value[0] != null)
                {
                    retVal = new SearchResponseDto(skip + 1, take, results.Value[0].TotalRowsIncludingDuplicates);
                    retVal.SearchResults.AddRange(
                        results.Value[0].ResultRows.Select(GetSearchContentDto).Where(contentDto => contentDto != null));
                    retVal.Properties = returnProps;
                }
            }

            return retVal;
        }

        private static SearchResultsBaseDto GetSearchContentDto(IDictionary<string,object> item)
        {
            if (item["VALContentType"] != null)
            {
                switch (item["VALContentType"].ToString().ToLower())
                {
                    case "policy":
                        return SearchResultSubscribeDto.GetInstance(item);
                    case "subscribebroker":
                        return SearchResultBrokerDto.GetInstance(item);
                    case "subscribeinsured":
                        return SearchResultInsuredDto.GetInstance(item);
                    case "claims":
                        return SearchResultClaimDto.GetInstance(item);
                }
            }
            return null;
        }

        /*
        public string GetATOMSearchResults(string searchTerm, int skip, int take)
        {
            string retVal = string.Empty;

            string url = String.Format(Settings.Default.SP2013RestUrl, HttpUtility.UrlEncode(searchTerm), Settings.Default.SP2013Properties);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UseDefaultCredentials = true;
            request.Method = "GET";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // process response..            
            XDocument oDataXML = XDocument.Load(response.GetResponseStream(), LoadOptions.None);
            XNamespace atom = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            List<XElement> items = oDataXML.Descendants(d + "query")
                .Elements(d + "PrimaryQueryResult")
                .Elements(d + "RelevantResults")
                .Elements(d + "Table")
                .Elements(d + "Rows")
                .Elements(d + "element")
                .ToList();      

            return retVal;
        }
        */
        public XmlDocument GetSearchResultsXML(String searchTerm, int skip, int take)
        {
            //SPQueryService.QueryService
            XmlDocument retDoc;
            using (var svc = new SPQueryService.QueryService())
            {
                svc.UseDefaultCredentials = true;

                // get the query xml
                var queryXml = new StringBuilder();

                var queryDoc = new XmlDocument();
                // TODO: Move the XML to resource ?
                queryDoc.Load(XmlReader.Create(HttpContext.Current.Server.MapPath("~/Content/xml/SearchQuery.xml")));

                queryXml.AppendFormat(queryDoc.OuterXml, searchTerm, skip + 1, take);

                var resp = svc.Query(queryXml.ToString());

                // load the xml document
                retDoc = new XmlDocument();
                retDoc.LoadXml(resp);

                // we need to add the number of records we expect to see in a row to the xml
                // get the range node
                var mgr = new XmlNamespaceManager(retDoc.NameTable);
                mgr.AddNamespace("resp", "urn:Microsoft.Search.Response");

                var rangeNode = retDoc.SelectSingleNode("resp:ResponsePacket/resp:Response/resp:Range", mgr);
                var statusNode = retDoc.SelectSingleNode("resp:ResponsePacket/resp:Response/resp:Status", mgr);

                if (rangeNode == null)
                {
                    if ((statusNode != null) && (statusNode.InnerText.ToLower().Contains("error")))
                    {
                        // do nothing this can be because there are no results in the search
                    }
                    else
                    {
                        throw new Exception("There was an error getting search results from SharePoint");
                    }
                }
                else
                {
                    var takeNode = retDoc.CreateElement("Take", "urn:Microsoft.Search.Response");
                    takeNode.InnerText = take.ToString(CultureInfo.InvariantCulture);
                    rangeNode.AppendChild(takeNode);
                }
            }
            return retDoc;
        }

        public string GetSearchResultsString(String searchTerm, int skip, int take)
        {
            string retVal;

            using (var svc = new SPQueryService.QueryService())
            {
                svc.UseDefaultCredentials = true;

                // get the query xml
                var queryXml = new StringBuilder();

                var queryDoc = new XmlDocument();
                queryDoc.Load(XmlReader.Create(@"/Content/xml/SearchQuery.xml"));

                queryXml.AppendFormat(queryDoc.OuterXml, searchTerm, skip + 1, take);

                // now execute the query
                retVal = svc.Query(queryXml.ToString());

            }

            return retVal;
        }
    }
}