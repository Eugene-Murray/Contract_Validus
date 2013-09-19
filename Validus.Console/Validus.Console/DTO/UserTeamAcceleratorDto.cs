using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Validus.Console.DTO
{
    public class UserTeamAcceleratorDto
    {

        public string CategoryName { get; set; }
        public List<AppAcceleratorDto> AppAcceleratorDtos { get; set; }

    }
}