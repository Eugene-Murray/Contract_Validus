using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class COBDto
    {
        [StringLength(10)]
        public String Id { get; set; }

        [Required, StringLength(100)]
        public String Narrative { get; set; }
    }
}