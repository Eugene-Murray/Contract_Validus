using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.BrokerService;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.Data
{
    public interface IBrokerData
    {
        BrokerMeasures GetBrokerMeasuresById(string brokerCd);
        List<BrokerMeasures> ListBrokerMeasures();
        List<BrokerDetails> GetBrokerDetailsById(string brokerCd);
        List<BrokerDevelopmentStatistic> GetBrokerDevelopmentStatsById(string brokerCd, string cobs);
        BrokerSummary GetBrokerSummaryById(string brokerCd);
        Broker GetBroker(int brokerSequenceId);
    }
}
