using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Validus.Console.BusinessLogic;
using Validus.Console.Data;
using Validus.Core.LogHandling;
using Validus.Models;
using Validus.Services.Models;

namespace Validus.Console.Init
{
    public static class DatabaseInit
    {
        private static IConsoleRepository _consoleRepository = new ConsoleRepository();
        private static ILogHandler _logHandler = new LogHandler();

        //  How to inject interface here?
        public static void SyncSemiStaticData()
        {
            _logHandler.WriteLog("SyncSemiStaticData()", LogSeverity.Information, LogCategory.DataAccess);
            
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri(Properties.Settings.Default.ServicesHostUrl);

            // Add an Accept header for JSON format. 
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));


            using (_consoleRepository)
            {
                HttpResponseMessage response = client.GetAsync("rest/api/cob").Result;  // Blocking call! 
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking! 
                    var cobs = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.COB>>().Result;


                    foreach (var p in cobs)
                    {
                        //bm.AddCOBIfNotAlreadyPresent(c);

                        if (!(_consoleRepository.Query<Validus.Models.COB>().Where(s => s.Id == p.Code).Any()))
                        {
                            Validus.Models.COB c = new Validus.Models.COB() { Id = p.Code, Narrative = p.Name };
                            _consoleRepository.Add<Validus.Models.COB>(c);
                        }
                    }
                }
                else
                {
                    _logHandler.WriteLog("Get rest/api/cob failed", LogSeverity.Information, LogCategory.DataAccess);
                }

                response = client.GetAsync("rest/api/Office").Result;  // Blocking call! 
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking! 
                    var Offices = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.Office>>().Result;


                    foreach (var p in Offices)
                    {
                        //bm.AddOfficeIfNotAlreadyPresent(c);

                        if (!(_consoleRepository.Query<Validus.Models.Office>().Any(s => s.Id == p.Code)))
                        {
                            Validus.Models.Office c = new Validus.Models.Office() { Id = p.Code, Name = p.Name };
                            _consoleRepository.Add<Validus.Models.Office>(c);
                        }
                    }

                }
                else
                {
                    _logHandler.WriteLog("Get rest/api/Office failed", LogSeverity.Information, LogCategory.DataAccess);
                }


                response = client.GetAsync("rest/api/Broker").Result;  // Blocking call! 
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking! 
                    var brokers = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.Broker>>().Result;

                    foreach (var broker in brokers)
                    {
                        if (!(_consoleRepository.Query<Validus.Models.Broker>().Any(b => b.BrokerSequenceId == broker.Id)))
                        {
                            _consoleRepository.Add(new Validus.Models.Broker { BrokerSequenceId = broker.Id, Name = broker.Name, Code = broker.Code, Psu = broker.Psu, GroupCode = broker.GrpCd });
                        }
                        else
                        {
                            var updateBroker =
                                _consoleRepository.Query<Validus.Models.Broker>().FirstOrDefault(b => b.BrokerSequenceId == broker.Id);

                            if (updateBroker != null)
                            {
                                updateBroker.Code = broker.Code;
                                updateBroker.GroupCode = broker.GrpCd;
                                updateBroker.Name = broker.Name;
                                updateBroker.Psu = broker.Psu;
                                _consoleRepository.Attach<Validus.Models.Broker>(updateBroker);
                            }
                        }
                    }
                }
                else
                {
                    _logHandler.WriteLog("Get rest/api/Broker failed", LogSeverity.Information, LogCategory.DataAccess);
                }

                response = client.GetAsync("rest/api/Underwriter").Result;  // Blocking call! 
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking! 
                    var underwriters = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.Underwriter>>().Result;

                    foreach (var underwriter in underwriters)
                    {
                        if (!(_consoleRepository.Query<Validus.Models.Underwriter>().Any(u => u.Code == underwriter.Code)))
                        {
                            _consoleRepository.Add(new Validus.Models.Underwriter { Code = underwriter.Code, Name = underwriter.Name });
                        }
                        else
                        {
                            var updateUnderwriter =
                                _consoleRepository.Query<Validus.Models.Underwriter>().FirstOrDefault(b => b.Code == underwriter.Code);

                            if (updateUnderwriter != null)
                            {
                                updateUnderwriter.Code = underwriter.Code;
                                updateUnderwriter.Name = underwriter.Name;
                                _consoleRepository.Attach<Validus.Models.Underwriter>(updateUnderwriter);
                            }
                        }
                    }

                }
                else
                {
                    _logHandler.WriteLog("Get rest/api/Underwriter failed", LogSeverity.Information, LogCategory.DataAccess);
                }

                response = client.GetAsync("rest/api/riskcode").Result;  // Blocking call! 
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking! 
                    var risks = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.RiskCode>>().Result;

                    foreach (var risk in risks)
                    {
                        if (!(_consoleRepository.Query<Validus.Models.RiskCode>().Any(u => u.Code == risk.Code)))
                        {
                            _consoleRepository.Add(new Validus.Models.RiskCode { Code = risk.Code, Name = risk.Name });
                        }
                        else
                        {
                            var updaterisk =
                                _consoleRepository.Query<Validus.Models.RiskCode>().FirstOrDefault(b => b.Code == risk.Code);

                            if (updaterisk != null)
                            {
                                updaterisk.Code = risk.Code;
                                updaterisk.Name = risk.Name;
                                _consoleRepository.Attach<Validus.Models.RiskCode>(updaterisk);
                            }
                        }
                    }

                }
                else
                {
                    _logHandler.WriteLog("Get rest/api/risk failed", LogSeverity.Information, LogCategory.DataAccess);
                }

                _consoleRepository.SaveChanges();
            }
        }
    }

}