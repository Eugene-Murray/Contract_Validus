using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class BrokerSummaryDto
    {
        public string Broker { get; set; }
        public string BrokerRating { get; set; }
        public string BrokerScore { get; set; }
        public string BrokerCreditLimit { get; set; }
    }
}