using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Validus.Console.Data;
using Validus.Core.Data;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.Init
{
	public static class DatabaseInit
	{
		private static readonly ILogHandler _LogHandler = new LogHandler();

		public static void SyncSemiStaticData()
		{
			DatabaseInit._LogHandler.WriteLog("SyncSemiStaticData()", LogSeverity.Information, LogCategory.DataAccess);

			using (var httpHandler = new HttpClientHandler())
			{
				httpHandler.UseDefaultCredentials = true;

				using (var httpClient = new HttpClient(httpHandler))
				{
					httpClient.BaseAddress = new Uri(Properties.Settings.Default.ServicesHostUrl);
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

					using (var consoleRepository = new ConsoleRepository())
					{
						DatabaseInit.SyncBrokers(httpClient, consoleRepository);
						DatabaseInit.SyncCOBs(httpClient, consoleRepository);
						DatabaseInit.SyncOffices(httpClient, consoleRepository);
						DatabaseInit.SyncUnderwriters(httpClient, consoleRepository);
						DatabaseInit.SyncRiskCodes(httpClient, consoleRepository);

						consoleRepository.SaveChanges();
					}
				}
			}
		}

		private static void SyncBrokers(HttpClient httpClient, IRepository consoleRepository)
		{
			DatabaseInit._LogHandler.WriteLog("SyncBrokers()", LogSeverity.Information, LogCategory.DataAccess);

			var response = httpClient.GetAsync("rest/api/broker").Result;

			if (response.IsSuccessStatusCode)
			{
				var serviceBrokers = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.Broker>>().Result;

				foreach (var serviceBroker in serviceBrokers)
				{
					if (!consoleRepository.Query<Broker>().Any(cb => cb.BrokerSequenceId == serviceBroker.Id))
					{
						consoleRepository.Add(new Broker
						{
							BrokerSequenceId = serviceBroker.Id,
							Name = serviceBroker.Name,
							Code = serviceBroker.Code,
							Psu = serviceBroker.Psu,
							GroupCode = serviceBroker.GrpCd
						});
					}
					else
					{
						var consoleBroker = consoleRepository.Query<Broker>()
						                                     .FirstOrDefault(cb => cb.BrokerSequenceId == serviceBroker.Id);

						if (consoleBroker != null)
						{
							consoleBroker.Code = serviceBroker.Code;
							consoleBroker.GroupCode = serviceBroker.GrpCd;
							consoleBroker.Name = serviceBroker.Name;
							consoleBroker.Psu = serviceBroker.Psu;

							consoleRepository.Attach(consoleBroker);
						}
					}
				}
			}
			else
			{
				DatabaseInit._LogHandler.WriteLog("Get rest/api/broker failed", LogSeverity.Warning, LogCategory.DataAccess);
			}
		}

		private static void SyncCOBs(HttpClient httpClient, IRepository consoleRepository)
		{
			DatabaseInit._LogHandler.WriteLog("SyncCOBs()", LogSeverity.Information, LogCategory.DataAccess);

			var response = httpClient.GetAsync("rest/api/cob").Result;

			if (response.IsSuccessStatusCode)
			{
				var serviceCOBs = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.COB>>().Result;

				foreach (var serviceCOB in serviceCOBs.Where(sc => !consoleRepository.Query<COB>()
				                                                                     .Any(cc => cc.Id == sc.Code)))
				{
					consoleRepository.Add(new COB
					{
						Id = serviceCOB.Code,
						Narrative = serviceCOB.Name
					});
				}
			}
			else
			{
				DatabaseInit._LogHandler.WriteLog("Get rest/api/cob failed", LogSeverity.Warning, LogCategory.DataAccess);
			}
		}

		private static void SyncOffices(HttpClient httpClient, IRepository consoleRepository)
		{
			DatabaseInit._LogHandler.WriteLog("SyncOffices()", LogSeverity.Information, LogCategory.DataAccess);

			var response = httpClient.GetAsync("rest/api/office").Result;

			if (response.IsSuccessStatusCode)
			{
				var serviceOffices = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.Office>>().Result;

				foreach (var serviceOffice in serviceOffices.Where(so => !consoleRepository.Query<Office>()
				                                                                           .Any(co => co.Id == so.Code)))
				{
					consoleRepository.Add(new Office
					{
						Id = serviceOffice.Code,
						Name = serviceOffice.Name,
						Title = serviceOffice.Name
					});
				}
			}
			else
			{
				DatabaseInit._LogHandler.WriteLog("Get rest/api/office failed", LogSeverity.Warning, LogCategory.DataAccess);
			}
		}

		private static void SyncUnderwriters(HttpClient httpClient, IRepository consoleRepository)
		{
			DatabaseInit._LogHandler.WriteLog("SyncUnderwriters()", LogSeverity.Information, LogCategory.DataAccess);

			var response = httpClient.GetAsync("rest/api/underwriter").Result;

			if (response.IsSuccessStatusCode)
			{
				var serviceUnderwriters = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.Underwriter>>().Result;

				foreach (var serviceUnderwriter in serviceUnderwriters)
				{
					if (!consoleRepository.Query<Underwriter>().Any(cu => cu.Code == serviceUnderwriter.Code))
					{
						consoleRepository.Add(new Underwriter
						{
							Code = serviceUnderwriter.Code,
							Name = serviceUnderwriter.Name
						});
					}
					else
					{
						var consoleUnderwriter = consoleRepository.Query<Underwriter>()
						                                          .FirstOrDefault(b => b.Code == serviceUnderwriter.Code);

						if (consoleUnderwriter != null)
						{
							consoleUnderwriter.Code = serviceUnderwriter.Code;
							consoleUnderwriter.Name = serviceUnderwriter.Name;

							consoleRepository.Attach(consoleUnderwriter);
						}
					}
				}
			}
			else
			{
				DatabaseInit._LogHandler.WriteLog("Get rest/api/underwriter failed", LogSeverity.Warning, LogCategory.DataAccess);
			}
		}

		private static void SyncRiskCodes(HttpClient httpClient, IRepository consoleRepository)
		{
			DatabaseInit._LogHandler.WriteLog("SyncRiskCodes()", LogSeverity.Information, LogCategory.DataAccess);

			var response = httpClient.GetAsync("rest/api/riskcode").Result;

			if (response.IsSuccessStatusCode)
			{
				var serviceRisks = response.Content.ReadAsAsync<IEnumerable<Validus.Services.Models.RiskCode>>().Result;

				foreach (var serviceRisk in serviceRisks)
				{
					if (!consoleRepository.Query<RiskCode>()
					                      .Any(crc => crc.Code == serviceRisk.Code))
					{
						consoleRepository.Add(new RiskCode
						{
							Code = serviceRisk.Code,
							Name = serviceRisk.Name
						});
					}
					else
					{
						var consoleRisk = consoleRepository.Query<RiskCode>()
						                                   .FirstOrDefault(crc => crc.Code == serviceRisk.Code);

						if (consoleRisk != null)
						{
							consoleRisk.Code = serviceRisk.Code;
							consoleRisk.Name = serviceRisk.Name;

							consoleRepository.Attach(consoleRisk);
						}
					}
				}
			}
			else
			{
				DatabaseInit._LogHandler.WriteLog("Get rest/api/riskcode failed", LogSeverity.Warning, LogCategory.DataAccess);
			}
		}
	}
}