using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class LossRatioForMonth
    {
        public string Year { get; set; }
        public List<string> LossRatios { get; set; }
        public List<string> Months { get; set; }
    }
}