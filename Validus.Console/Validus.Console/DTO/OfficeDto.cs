using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class OfficeDto
    {
        [StringLength(256)]
        public String Id { get; set; }

        [Required, StringLength(256)]
        public String Title { get; set; }
    }
}