using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class TeamMembershipDto
    {
        public Int32 Id { get; set; }

        public Int32 TeamId { get; set; }
        public TeamDto Team { get; set; }

        public Int32 UserId { get; set; }
        public UserDto User { get; set; }

        public Boolean PrimaryTeamMembership { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public Boolean IsCurrent { get; set; }
    }
}