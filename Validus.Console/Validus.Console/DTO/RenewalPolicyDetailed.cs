using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Validus.Console.DTO
{
    public class RenewalPolicyDetailed : RenewalPolicy
    {
        [DisplayName("Renewal Notes")]
        public String RenewalNotes { get; set; }

        [DisplayName("Renewal Position")]
        public String RenewalPosition { get; set; }

        public String ToBroker { get; set; }
        public String ToBrokerContact { get; set; }
        public String ToPolicyId { get; set; }
        public String ToStatus { get; set; }
        public String ToSubmissionStatus { get; set; }
        public String ToEntryStatus { get; set; }
        public RenewalDataDto RenewalData { get; set; }
    }
}