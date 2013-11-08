using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using Validus.Console.DTO;
using Validus.Console.Data;
using System.Collections.Generic;

namespace Validus.Console.BusinessLogic
{
    public class SearchBusinessModule : ISearchBusinessModule
	{
		// TODO: Use a configuration setting
		private const string XSLTTransformFile = "~/content/xml/searchtransform.xslt";

		private readonly ISearchData SearchData;

        public SearchBusinessModule(ISearchData searchData)
		{
			this.SearchData = searchData;
        }

        public SearchResponseDto GetSearchResults(string term, Dictionary<string, string> sources, Dictionary<String, String> refiners, int skip, int take)
		{
			return this.SearchData.GetSearchResults(term, sources, refiners, skip, take);
		}

		public string GetSearchResultsHtml(string term, int skip, int take)
		{
			var response = new StringBuilder();

			var xmlResults = this.SearchData.GetSearchResultsXML(term, skip, take);

			if (xmlResults != null)
			{
				using (var xmlReader = XmlReader.Create(new StringReader(xmlResults.OuterXml)))
				{
					using (var xslReader = XmlReader.Create(HttpContext.Current.Server.MapPath(XSLTTransformFile)))
					{
						using (var xmlWriter = XmlWriter.Create(response, new XmlWriterSettings
						{
							OmitXmlDeclaration = true
						}))
						{
							var xslTransformer = new XslCompiledTransform();

							xslTransformer.Load(xslReader, new XsltSettings(true, true), new XmlUrlResolver
							{
								Credentials = CredentialCache.DefaultCredentials
							});

							xslTransformer.Transform(xmlReader, xmlWriter);
						}
					}
				}
			}

			return response.ToString();
		}
    }
}