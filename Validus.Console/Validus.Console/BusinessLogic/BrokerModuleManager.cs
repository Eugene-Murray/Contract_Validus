using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Console.Data;
using Validus.Core.LogHandling;
using Validus.Console.BrokerService;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public class BrokerModuleManager : IBrokerModuleManager
    {
        public readonly IConsoleRepository _consoleRepository;
        public readonly ILogHandler _logHandler;
        public readonly IBrokerData _brokerData;

        public BrokerModuleManager(IBrokerData brokerData, ILogHandler logHandler, IConsoleRepository ConsoleRepository)
        {
            _logHandler = logHandler;
            _brokerData = brokerData;
            _consoleRepository = ConsoleRepository;
        }

        public Broker GetBrokerBySeqId(int bkrSeqId)
        {
            return _brokerData.GetBroker(bkrSeqId);
        }

        public List<BrokerDetails> GetBrokerDetailsById(string brokerCd)
        {
            return _brokerData.GetBrokerDetailsById(brokerCd);
        }

        public BrokerMeasures GetBrokerMeasuresById(string brokerCd)
        {
            return _brokerData.GetBrokerMeasuresById(brokerCd);
        }

        public List<BrokerMeasures> ListBrokerMeasures()
        {
            return _brokerData.ListBrokerMeasures();
        }

        public BrokerSummaryDto GetBrokerSummaryById(string brokerCd)
        {
            var brokerSummary = _brokerData.GetBrokerSummaryById(brokerCd);

            return new BrokerSummaryDto 
                        { Broker = brokerSummary.Broker,
                          BrokerCreditLimit = String.Format("{0:0,0}", brokerSummary.CreditLimit), 
                          BrokerScore = brokerSummary.BrokerScore, 
                          BrokerRating = brokerSummary.BrokerRating };
        }

        public BrokerDevelopmentStatsDto GetBrokerDevelopmentStatsById(string brokerCd)
        {
            var brokerLossRatioDto = new BrokerDevelopmentStatsDto { LossRatioForMonthList = new List<LossRatioForMonth>() };

            // get all of the cob's for the team 
            // get the user
            var userName = HttpContext.Current.User.Identity.Name;
            // get the users teams
            var currentUserTeams = _consoleRepository.Query<TeamMembership>().Where(tm => tm.User.DomainLogon.Contains(userName) && tm.IsCurrent).Select(t => t.Team);
            if (currentUserTeams != null)
            {
                // get the cobs
                List<COB> cobs = currentUserTeams.SelectMany(t => t.RelatedCOBs).Distinct().ToList();
                if ((cobs != null) && (cobs.Count > 0))
                {
                    // convert the list of cobs to a string
                    var coblist = "";
                    foreach (COB cob in cobs)
                    {
                        if (coblist.Length > 0)
                            coblist += ",";
                        coblist += cob.Id + " - " + cob.Narrative;
                    }

                    if (!string.IsNullOrEmpty(coblist))
                    {
                        var allValsByYear =
                            _brokerData.GetBrokerDevelopmentStatsById(brokerCd, coblist)
                                       .OrderByDescending(b => b.AccountYear)
                                       .GroupBy(b => b.AccountYear)
                                       .ToList();

                        if (allValsByYear != null)
                        {
                            brokerLossRatioDto.LossRatioForMonthList = allValsByYear.Select(y =>
                                {
                                    return new LossRatioForMonth
                                        {
                                            Year = y.Key,
                                            LossRatios =
                                                y.Select(val => (val.LossRatio.HasValue ? val.LossRatio.Value.ToString("#.##") : "0"))
                                                 .ToList(),
                                            Months = y.Select(val => val.DevelopmentMonth.Replace("M", "")).ToList()
                                        };
                                }).ToList();

                            if (brokerLossRatioDto.LossRatioForMonthList.Count > 0)
                            {
                                var highestMonthCount =
                                    brokerLossRatioDto.LossRatioForMonthList.Select(lr => lr.Months.Count()).Max();

                                var yearWithHighestMonths =
                                    brokerLossRatioDto.LossRatioForMonthList.FirstOrDefault(
                                        lr => lr.Months.Count() == highestMonthCount);
                                brokerLossRatioDto.Months =
                                    brokerLossRatioDto.LossRatioForMonthList.FirstOrDefault(
                                        lr => lr.Year == yearWithHighestMonths.Year).Months;

                                return brokerLossRatioDto;
                            }
                        }
                    }
                }
            }
            return brokerLossRatioDto;
        }
    }
}