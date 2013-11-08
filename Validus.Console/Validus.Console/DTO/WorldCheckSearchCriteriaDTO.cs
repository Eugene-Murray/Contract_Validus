using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class WorldCheckSearchCriteriaDto
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Keywords")]
        public string Keywords { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("UID")]
        public int Uid { get; set; }
    }
}