using System;
using System.Collections.Generic;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class OptionVersionDto
    {
        public Int32 OptionId { get; set; }
        public Int32 VersionNumber { get; set; }
        public String Title { get; set; }
        public String Comments { get; set; }
        public Boolean IsExperiment { get; set; }
        public Boolean IsLocked { get; set; }
        public ICollection<QuoteDto> Quotes { get; set; }
        public ICollection<QuoteSheet> QuoteSheets { get; set; }
        public Byte[] Timestamp { get; set; }

        // PV
        public String TSICurrency { get; set; }
        public Decimal? TSIPD { get; set; }
        public Decimal? TSIBI { get; set; }
        public Decimal? TSITotal { get; set; }
    }
}
