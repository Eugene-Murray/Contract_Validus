using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class AppAcceleratorDto:IComparable<AppAcceleratorDto>
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Category { get; set; }

        public int CompareTo(AppAcceleratorDto other)
        {
            return String.Compare(other.Id, Id, StringComparison.Ordinal);
        }
    }
}