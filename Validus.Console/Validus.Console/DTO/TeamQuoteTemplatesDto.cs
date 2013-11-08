using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class TeamQuoteTemplatesDto
    {
        public int TeamId { get; set; }
        public List<int> TeamQuoteTemplatesIdList { get; set; }
    }
}