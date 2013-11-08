using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Validus.Models
{
    public class Broker : ModelBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 BrokerSequenceId { get; set; }

		[DisplayName("GroupCode"), StringLength(10)]
        public String GroupCode { get; set; }

		[DisplayName("Code"), StringLength(10)]
        public String Code { get; set; }

		[DisplayName("Psu"), StringLength(256)]
		public String Name { get; set; }

		[DisplayName("Psu"), StringLength(10)]
		public String Psu { get; set; }
    }
}
