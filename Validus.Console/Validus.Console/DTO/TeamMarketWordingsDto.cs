using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class TeamMarketWordingsDto
    {
        public int TeamId { get; set; }
        public string OfficeId { get; set; }
        public List<MarketWordingSettingDto> MarketWordingSettingDtoList { get; set; }
    }
}