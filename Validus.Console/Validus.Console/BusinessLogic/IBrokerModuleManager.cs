using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validus.Console.BrokerService;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public interface IBrokerModuleManager
    {
        List<BrokerDetails> GetBrokerDetailsById(string brokerCd);
        BrokerMeasures GetBrokerMeasuresById(string brokerCd);
        List<BrokerMeasures> ListBrokerMeasures();
        BrokerDevelopmentStatsDto GetBrokerDevelopmentStatsById(string brokerCd);
        BrokerSummaryDto GetBrokerSummaryById(string brokerCd);
        Broker GetBrokerBySeqId(int bkrSeqId);
    }
}
