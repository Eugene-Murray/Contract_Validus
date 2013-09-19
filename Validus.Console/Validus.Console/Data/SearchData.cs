using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
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
            string contentSourcesSearchConditions = string.Empty;
            string[] sources = contentSources.Split(',');

            foreach (string source in sources)
            {
                if (contentSourcesSearchConditions.Length > 0)
                    contentSourcesSearchConditions += " OR ";
                contentSourcesSearchConditions += "contentsource=\"" + source + "\"";
            }

            if (contentSourcesSearchConditions.Length > 0)
                contentSourcesSearchConditions = " AND (" + contentSourcesSearchConditions + ")";

            return contentSourcesSearchConditions;
        }

        public SearchResponseDto GetSearchResults(string searchTerm, int skip, int take)
        {
            SearchResponseDto retVal = null;

            using (ClientContext ctx = new ClientContext(Settings.Default.SP2013SearchUrl))
            {
                // get the content sources for the query
                var contentSources = string.Empty;
                if (!string.IsNullOrEmpty(Settings.Default.SP2013ContentSources))
                {
                    contentSources = GetContentSourcesSearchCondition(Settings.Default.SP2013ContentSources);
                }

                KeywordQuery query = new KeywordQuery(ctx);
                query.QueryText = searchTerm + contentSources;
                query.StartRow = skip;
                query.RowLimit = take;
                string[] returnProps = Settings.Default.SP2013Properties.Split(',');

                foreach (string prop in returnProps)
                {
                    query.SelectProperties.Add(prop);
                }

                SearchExecutor executor = new SearchExecutor(ctx);

                ClientResult<ResultTableCollection> results = executor.ExecuteQuery(query);

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
            using (SPQueryService.QueryService svc = new SPQueryService.QueryService())
            {
                svc.UseDefaultCredentials = true;

                // get the query xml
                StringBuilder queryXml = new StringBuilder();

                XmlDocument queryDoc = new XmlDocument();
                // todo move the xml to resource?
                queryDoc.Load(XmlReader.Create(HttpContext.Current.Server.MapPath("~/Content/xml/SearchQuery.xml")));

                queryXml.AppendFormat(queryDoc.OuterXml, searchTerm, skip + 1, take);

                string resp = svc.Query(queryXml.ToString());

                // load the xml document
                retDoc = new XmlDocument();
                retDoc.LoadXml(resp);

                // we need to add the number of records we expect to see in a row to the xml
                // get the range node
                XmlNamespaceManager mgr = new XmlNamespaceManager(retDoc.NameTable);
                mgr.AddNamespace("resp", "urn:Microsoft.Search.Response");

                XmlNode rangeNode = retDoc.SelectSingleNode("resp:ResponsePacket/resp:Response/resp:Range", mgr);
                XmlNode statusNode = retDoc.SelectSingleNode("resp:ResponsePacket/resp:Response/resp:Status", mgr);

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

                    XmlNode takeNode = retDoc.CreateElement("Take", "urn:Microsoft.Search.Response");
                    takeNode.InnerText = take.ToString(CultureInfo.InvariantCulture);
                    rangeNode.AppendChild(takeNode);
                }
            }
            return retDoc;
        }

        public string GetSearchResultsString(String searchTerm, int skip, int take)
        {
            string retVal;

            using (SPQueryService.QueryService svc = new SPQueryService.QueryService())
            {
                svc.UseDefaultCredentials = true;

                // get the query xml
                StringBuilder queryXml = new StringBuilder();

                XmlDocument queryDoc = new XmlDocument();
                queryDoc.Load(XmlReader.Create(@"/Content/xml/SearchQuery.xml"));

                queryXml.AppendFormat(queryDoc.OuterXml, searchTerm, skip + 1, take);

                // now execute the query
                retVal = svc.Query(queryXml.ToString());

            }
            return retVal;
        }
    }
}