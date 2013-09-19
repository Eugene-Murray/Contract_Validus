using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class CreateBrokerContactDto
    {
        [Required, DisplayName("New Broker Contact")]
        public string NewBrokerContact { get; set; }
    }
}