using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class TeamTermsNConditionWordingsDto
    {
        public int TeamId { get; set; }
        public string OfficeId { get; set; }
        public List<TermsNConditionWordingSettingDto> TermsNConditionWordingSettingDtoList { get; set; }
    }
}