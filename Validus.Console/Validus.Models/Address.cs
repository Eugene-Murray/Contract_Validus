using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Validus.Models
{
    //http://uxmatters.com/mt/archives/2008/06/international-address-fields-in-web-forms.php
    public class Address : ModelBase
	{
		[DisplayName("Id")]
        public Int32 Id { get; set; }

        [DisplayName("Line 1"), StringLength(256)]
        public String AddressLine1 { get; set; }

        [DisplayName("Line 2"), StringLength(256)]
		public String AddressLine2 { get; set; }

        [DisplayName("City"), StringLength(256)]
		public String City { get; set; }

        [DisplayName("State/Province/Region"), StringLength(256)]
		public String StateProvinceRegion { get; set; }

        [DisplayName("Zip/Postal Code"), StringLength(20)]
		public String ZipPostalCode { get; set; }

		[DisplayName("Country"), StringLength(256)]
		public String Country { get; set; }

		[DisplayName("Phone Number"), StringLength(256)]
		public String Phone { get; set; }

		[DisplayName("Fax Number"), StringLength(256)]
		public String Fax { get; set; }

		[DisplayName("Url"), StringLength(256)]
		public String Url { get; set; }
    }
}