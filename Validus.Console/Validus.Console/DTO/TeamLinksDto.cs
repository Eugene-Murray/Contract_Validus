using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class TeamLinksDto
    {
        public int TeamId { get; set; }
        public List<int> TeamLinksIdList { get; set; }
    }
}