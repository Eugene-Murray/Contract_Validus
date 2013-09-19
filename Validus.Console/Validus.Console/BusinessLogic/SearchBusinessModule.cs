using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using Validus.Console.DTO;
using Validus.Console.Data;
using Validus.Core.LogHandling;

namespace Validus.Console.BusinessLogic
{
    public class SearchBusinessModule : ISearchBusinessModule
    {

        ISearchData _searchData = null;
        ILogHandler _logHandler;

        public SearchBusinessModule(ISearchData rep, ILogHandler logHandler)
        {
            _searchData = rep;
            _logHandler = logHandler;
        }

        public SearchResponseDto GetSearchResults(string searchTerm, int skip, int take)
        {
            return _searchData.GetSearchResults(searchTerm, skip, take);
        }

        public string GetSearchResultsHtml(string searchTerm, int skip, int take)
        {
            XsltSettings xslSettings = new XsltSettings(true, true);
            XmlUrlResolver xslResolver = new XmlUrlResolver();
            xslResolver.Credentials = CredentialCache.DefaultCredentials;
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.OmitXmlDeclaration = true;
            XmlWriter xmlWriter = null;
            XmlReader xmlReader = null;
            XmlReader xslReader = null;
            XslCompiledTransform xFormer = null;

            try
            {
                // get the search restuls
                XmlDocument searchResultsDoc = _searchData.GetSearchResultsXML(searchTerm, skip, take);
                if (searchResultsDoc != null)
                {
                    xmlReader = XmlReader.Create(new StringReader(searchResultsDoc.OuterXml));
                    xslReader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Content/xml/SearchTransform.xslt"));
                    
                    xFormer = new XslCompiledTransform();

                    xFormer.Load(xslReader, xslSettings, xslResolver);

                    StringBuilder stringBuilder = new StringBuilder();
                    xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
                    xFormer.Transform(xmlReader, xmlWriter);                   
                    
                    return stringBuilder.ToString();
                }
            }
            /*catch
            {
                // todo
            }*/
            finally
            {
                if(xmlWriter!=null){
                    xmlWriter.Close();
                    xmlWriter = null;
                }
                if (xslReader != null)
                {
                    xslReader.Close();
                    xslReader = null;
                }
                if (xmlReader != null)
                {
                    xmlReader.Close();
                    xmlReader = null;
                }
                xslSettings = null;
                xslResolver = null;
                xFormer = null;
            }
            return string.Empty;
        }
    }
}