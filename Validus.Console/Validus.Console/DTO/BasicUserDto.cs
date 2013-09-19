using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class BasicUserDto
    {
        public Int32 Id { get; set; }

        [Required, DisplayName("Logon Name"), StringLength(256)]
        public String DomainLogon { get; set; }

        public bool IsCurrentMembership { get; set; }
    }
}