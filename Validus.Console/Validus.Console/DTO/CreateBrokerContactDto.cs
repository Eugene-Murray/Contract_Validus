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
        [Required, DisplayName("Name")]
        public string NewBrokerContactName { get; set; }

        [Required, DisplayName("Company")]
        public string NewBrokerContactCompany { get; set; }

        [Required, DisplayName("Email")]
        public string NewBrokerContactEmail { get; set; }

        [Required, DisplayName("Phone Number")]
        public string NewBrokerContactPhoneNumber { get; set; }
    }
}