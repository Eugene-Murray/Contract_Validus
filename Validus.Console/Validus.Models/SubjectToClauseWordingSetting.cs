using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validus.Models
{
    public class SubjectToClauseWordingSetting: ModelBase
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsStrikeThrough { get; set; }
        public SubjectToClauseWording SubjectToClauseWording { get; set; }
    }
}
