using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Validus.Console.DTO
{
    public class UserTeamLinkDto
    {
        public string CategoryName { get; set; }
        public List<Url> Urls { get; set; }

    }

    public class Url
    {
        [ScriptIgnore, JsonIgnore]
        public string LinkCategory { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
    }
}
