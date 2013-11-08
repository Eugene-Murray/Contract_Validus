using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;
using Validus.Models;

namespace Validus.Console.DTO
{
    public class TeamDto
    {
        public Int32 Id { get; set; }

        [Required, StringLength(256)]
        public String Title { get; set; }

        public String SubmissionTypeId { get; set; }

        //public IList<TeamMembershipDto> Memberships { get; set; }

        public IList<BasicUserDto> Users { get; set; }
        public IList<LinkDto> Links { get; set; }
        public IList<COBDto> RelatedCOBs { get; set; }
        public IList<OfficeDto> RelatedOffices { get; set; }

        public IList<BasicUserDto> AllUsers { get; set; }
        public IList<LinkDto> AllLinks { get; set; }
        public IList<COBDto> AllRelatedCOBs { get; set; }
        public IList<OfficeDto> AllRelatedOffices { get; set; }

        [DisplayName("Default MOA"), StringLength(10)]
		public String DefaultMOA { get; set; }

        [DisplayName("Default Domicile"), StringLength(10)]
		public String DefaultDomicile { get; set; }

		[DisplayName("Pricing Actuary")]
        public User PricingActuary { get; set; }

        [DisplayName("Default Policy Type")]
        public String DefaultPolicyType { get; set; }

        //public Int32 PricingActuaryId { get; set; }
        [Integer(ErrorMessage = "Integer value required")]
		[Required, DisplayName("Default Quote Expiry Days *"), Range(1, 365)]
        public Int32 QuoteExpiryDaysDefault { get; set; }

    }
}