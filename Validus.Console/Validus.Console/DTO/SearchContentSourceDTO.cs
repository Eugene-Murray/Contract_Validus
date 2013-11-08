using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class SearchContentSourceDto
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public bool IsSearched { get; set; }
    }
}