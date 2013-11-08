using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Mvc;
using Validus.Console.VersionService;

namespace Validus.Console.Controllers
{
	[Authorize(Roles = @"ConsoleRead")]
    public class AboutController : Controller
    {
        private readonly IVersionService _service;

        public AboutController(IVersionService service)
        {
            _service = service;
        }
        //
        // GET: /About/
		[OutputCache(Duration = 86400)]
        public ActionResult _AboutModal()
        {
            ViewBag.ConsoleVersion = CurrentVersion();
            ViewBag.ModelVersion = Validus.Models.Version.CurrentVersion();
            Services.Models.Version version = GetServiceVersions();
            ViewBag.ServiceVersion = version.ServiceVersion;
            ViewBag.ServiceModelVersion = version.ModelVersion;
            Versions versions = _service.GetVersion();
            ViewBag.WCFDataContractVersion = versions.DataContractVersion;
            ViewBag.WCFDataAccessVersion = versions.DataAccessVersion;
            ViewBag.WCFVersion = versions.WCFVersion;
            ViewBag.WCFBusinessLogicVersion = versions.BusinessLogicVersion;

            return PartialView();
        }

        private Services.Models.Version GetServiceVersions()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri(Properties.Settings.Default.ServicesHostUrl);

            // Add an Accept header for JSON format. 
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = client.GetAsync("rest/api/version").Result;  // Blocking call! 
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking! 
                var version = response.Content.ReadAsAsync<Services.Models.Version>().Result;
                return version;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Return the Current Version from the AssemblyInfo.cs file.
        /// </summary>
        private static string CurrentVersion()
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version.ToString();
            }
            catch
            {
                return "?.?.?.?";
            }
        }

    }
}
