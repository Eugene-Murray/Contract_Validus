using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class TermsNConditionWordingSettingDto
    {
        public Int32 Id { get; set; }

        public Int32 DisplayOrder { get; set; }

        public bool IsStrikeThrough { get; set; }

        public string WordingRefNumber { get; set; }

        public string Title { get; set; }

        public int SettingId { get; set; }
    }
}