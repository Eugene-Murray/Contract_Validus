using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Validus.Console.DTO
{
    public class RenewalPolicy
    {
        public String PolicyId { get; set; }
        public String COB { get; set; }

        [DisplayName("OrigOff")]
        public String OriginatingOffice { get; set; }

        [DisplayName("UWR")]
        public String Underwriter { get; set; }

        [DisplayName("UMR")]
        public String Umr { get; set; }

        [DisplayName("Insured")]
        public String InsuredName { get; set; }

        [DisplayName("Inception Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime InceptionDate { get; set; }

        [DisplayName("Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode=true)]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        public String Broker { get; set; }

        [DisplayName("Broker Contact")]
        public String BrokerContact { get; set; }

        public String Leader { get; set; }

        public String Description { get; set; }

        [DisplayName("Status")]
        public String St { get; set; }

        [DisplayName("CalcLn")]
        public Decimal Line { get; set; }

        [DisplayName("CCY")]
        public String Currency { get; set; }

        public Decimal MarketGrossPremium { get; set; }
        public Decimal SyndicateGrossPremium { get; set; }
        public Decimal SyndicateNetPremium { get; set; }
        public Decimal SignedPremium { get; set; }
        public Decimal PercentageOfEPI { get; set; }

        public String HasClaims { get; set; }

        public string WebPolicyURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebPolicyURL"] + PolicyId;
            }
        }
    }
}