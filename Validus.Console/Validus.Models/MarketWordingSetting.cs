using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validus.Models
{
    public class MarketWordingSetting:ModelBase
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public MarketWording MarketWording { get; set; }
    }
}
