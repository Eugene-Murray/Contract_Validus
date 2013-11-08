using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class Team : ModelBase
	{
		[DisplayName("Id")]
        public Int32 Id { get; set; }

        [Required, DisplayName("Title"), StringLength(256)]
        public String Title { get; set; }

		[DisplayName("Memebrships")]
        public ICollection<TeamMembership> Memberships { get; set; }

		[DisplayName("Links")]
        public ICollection<Link> Links { get; set; }

        [DisplayName("Related COBs")]
        public ICollection<COB> RelatedCOBs { get; set; }

        [DisplayName("Related Offices")]
        public ICollection<Office> RelatedOffices { get; set; }

        [DisplayName("Default MOA"), StringLength(10)]
		public String DefaultMOA { get; set; }

        [DisplayName("Default Domicile"), StringLength(10)]
        public String DefaultDomicile { get; set; }

        [Required, DisplayName("Default Policy Type"), StringLength(100)]
        public String DefaultPolicyType { get; set; }

		[DisplayName("Pricing Actuary")]
		public User PricingActuary { get; set; }

		[DisplayName("Default Quote Expiry Days"), Range(1, 365)]
		public Int32 QuoteExpiryDaysDefault { get; set; }

        [DisplayName("Submission Type Id")]
        public String SubmissionTypeId { get; set; }

        [DisplayName("Available Quote Templates")]
        public ICollection<QuoteTemplate> RelatedQuoteTemplates { get; set; }

        [DisplayName("Available Application TeamOfficeSettings")]
        public ICollection<TeamOfficeSetting> TeamOfficeSettings { get; set; }

        [DisplayName("Available Application Accelerators")]
        public ICollection<AppAccelerator> AppAccelerators { get; set; }

        [DisplayName("Related Risks")]
        public ICollection<RiskCode> RelatedRisks { get; set; }
    }
}