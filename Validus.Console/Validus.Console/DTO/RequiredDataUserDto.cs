using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validus.Console.DTO
{
    public class RequiredDataUserDto
    {
        // Team List
        public List<TeamMembershipDto> AllTeamMemberships { get; set; }
        public List<COBDto> AllFilterCOBs { get; set; }
        public List<OfficeDto> AllFilterOffices { get; set; }
        public List<UserDto> AllFilterMembers { get; set; }
        public List<COBDto> AllAdditionalCOBs { get; set; }
        public List<OfficeDto> AllAdditionalOffices { get; set; }
        public List<UserDto> AllAdditionalUsers { get; set; }

        public List<OfficeDto> AllPrimaryOffices { get; set; }
        public List<OfficeDto> AllOriginatingOffices { get; set; }
        public List<UserDto> AllDefaultUnderwriters { get; set; }
    }
}
