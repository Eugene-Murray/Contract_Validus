using System;

namespace Validus.Console.DTO
{
    public class MarketWordingSettingDto
    {
        public Int32 SettingId { get; set; }

        public Int32 Id { get; set; }

        public Int32 DisplayOrder { get; set; }

        public bool IsStrikeThrough { get; set; }

        public string WordingRefNumber { get; set; }

        public string Title { get; set; }
    }
}