using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Validus.Console.BrokerService;
using Validus.Console.DTO;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.Data
{
    public class BrokerData : IBrokerData
    {
        public readonly ILogHandler LogHandler;
        public readonly IBrokerService BrokerService;
        private readonly IConsoleRepository _repository;

        public BrokerData(IConsoleRepository repository, IBrokerService brokerService, ILogHandler logHandler)
        {
            this.LogHandler = logHandler;
            this.BrokerService = brokerService;
            this._repository = repository;
        }

        public Broker GetBroker(int brokerSequenceId)
        {
            return this._repository.Query<Broker>().FirstOrDefault(b => b.BrokerSequenceId == brokerSequenceId);
        }

        public BrokerMeasures GetBrokerMeasuresById(string brokerCd)
        {
            return BrokerService.GetBrokerMeasuresById(brokerCd);
        }

        public List<BrokerMeasures> ListBrokerMeasures()
        {
            return BrokerService.ListBrokerMeasures();
        }

        public List<BrokerDetails> GetBrokerDetailsById(string brokerCd)
        {
            return BrokerService.GetBrokerDetailsById(brokerCd);
        }

        public List<BrokerDevelopmentStatistic> GetBrokerDevelopmentStatsById(string brokerCd, string cobs)
        {
            return BrokerService.GetBrokerDevelopmentStatsById(brokerCd, cobs);
        }

        public BrokerSummary GetBrokerSummaryById(string brokerCd)
        {
            return BrokerService.GetBrokerSummaryById(brokerCd);
        }
    } 
}