using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.Data
{
    public interface IPolicyData
    {
        List<RenewalPolicyDetailed> GetRenewalPolicies(Boolean bypassCache);
        Broker GetBroker(int brokerSequenceId);
        void RemovePolicyFromCache(string renewalPolicyId);


    }
}
