using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class LinkDto
    {
        public Int32 Id { get; set; }

        [Required, StringLength(256)]
        public String Title { get; set; }

        [Required, DataType(DataType.Url), StringLength(2048)]
        public String Url { get; set; }

        [StringLength(256)]
        public String Category { get; set; }
    }
}