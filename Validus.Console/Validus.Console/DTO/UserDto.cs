using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class UserDto
    {
        public Int32 Id { get; set; }

        [Required, DisplayName("Logon Name"), StringLength(256)]
        public String DomainLogon { get; set; }

        public ICollection<TeamMembershipDto> TeamMemberships { get; set; }
        public ICollection<COBDto> FilterCOBs { get; set; }
        public ICollection<OfficeDto> FilterOffices { get; set; }
        public ICollection<UserDto> FilterMembers { get; set; }
        public ICollection<COBDto> AdditionalCOBs { get; set; }
        public ICollection<OfficeDto> AdditionalOffices { get; set; }
        public ICollection<UserDto> AdditionalUsers { get; set; }

        public ICollection<TeamMembershipDto> AllTeamMemberships { get; set; }
        public ICollection<COBDto> AllFilterCOBs { get; set; }
        public ICollection<OfficeDto> AllFilterOffices { get; set; }
        public ICollection<UserDto> AllFilterMembers { get; set; }
        public ICollection<COBDto> AllAdditionalCOBs { get; set; }
        public ICollection<OfficeDto> AllAdditionalOffices { get; set; }
        public ICollection<UserDto> AllAdditionalUsers { get; set; }

        public List<OfficeDto> PrimaryOfficeList { get; set; }
        [DisplayName("Primary Office")]
        public OfficeDto PrimaryOffice { get; set; }

        public List<OfficeDto> DefaultOrigOfficeList { get; set; }
        [DisplayName("Default Originating Office")]
        public virtual OfficeDto DefaultOrigOffice { get; set; }

        public List<UserDto> DefaultUWList { get; set; }
        [DisplayName("Default Underwriter")]
        public UserDto DefaultUW { get; set; }

        //public virtual ICollection<Tab> OpenTabs { get; set; }

        [DisplayName("Active?")]
        public Boolean IsActive { get; set; }

        [StringLength(3)]
        public String UnderwriterId { get; set; }

        public bool IsCurrentMembership { get; set; }
    }
}