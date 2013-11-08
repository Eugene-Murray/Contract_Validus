using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class BrokerDevelopmentStatsDto
    {
        public string BrokerRating { get; set; }
        public string BrokerScore { get; set; }
        public string BrokerCreditLimit { get; set; }
        public List<string> Months { get; set; }
        public List<LossRatioForMonth> LossRatioForMonthList { get; set; }
    }
}