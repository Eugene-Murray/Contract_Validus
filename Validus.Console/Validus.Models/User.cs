using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class User : ModelBase
    {
        public User()
        {
            IsActive = true;
        }

		[DisplayName("Id")]
        public Int32 Id { get; set; }

        [Required, DisplayName("Logon Name"), StringLength(256)]
        public String DomainLogon { get; set; }

		[DisplayName("Team Memberships")]
        public ICollection<TeamMembership> TeamMemberships { get; set; }

		[DisplayName("Filter COBs")]
		public ICollection<COB> FilterCOBs { get; set; }

		[DisplayName("Filter Offices")]
		public ICollection<Office> FilterOffices { get; set; }

		[DisplayName("Filter Members")]
		public ICollection<User> FilterMembers { get; set; }

		[DisplayName("Additional COBs")]
		public ICollection<COB> AdditionalCOBs { get; set; }

		[DisplayName("Additional Offices")]
		public ICollection<Office> AdditionalOffices { get; set; }

		[DisplayName("Additional Users")]
		public ICollection<User> AdditionalUsers { get; set; }

		[DisplayName("Open Tabs")]
		public ICollection<Tab> OpenTabs { get; set; }

		[DisplayName("Underwriter"), StringLength(10)]
		public String UnderwriterCode { get; set; }

		[DisplayName("Underwriter")]
		public Underwriter Underwriter { get; set; }

        [DisplayName("NonLondonBroker")]
        public string NonLondonBroker { get; set; }

		[DisplayName("Primary Office Id")]
		public String PrimaryOfficeId { get; set; }

		[DisplayName("Primary Office")]
		public Office PrimaryOffice { get; set; }

		[DisplayName("Default Originating Office Id")]
		public String DefaultOrigOfficeId { get; set; }

		[DisplayName("Default Originating Office")]
		public Office DefaultOrigOffice { get; set; }

		[DisplayName("Default Underwriter Id")]
		public Int32? DefaultUWId { get; set; }

        [DisplayName("Default Underwriter")]
		public User DefaultUW { get; set; }

        [DisplayName("Active?")]
        public Boolean IsActive { get; set; }
    }
}