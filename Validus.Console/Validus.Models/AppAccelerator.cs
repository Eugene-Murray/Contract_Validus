using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Models
{
    public class AppAccelerator : ModelBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), StringLength(20)]
        public string Id { get; set; }

        [Required, DisplayName("HomepageUrl"), StringLength(256)]
        public string HomepageUrl { get; set; }

        [Required, DisplayName("DisplayName"), StringLength(256)]
        public string DisplayName { get; set; }

        [Required, DisplayName("DisplayIcon"), StringLength(256)]
        public string DisplayIcon { get; set; }

        [Required, DisplayName("ActivityCategory"), StringLength(256)]
        public string ActivityCategory { get; set; }

        [Required, DisplayName("ActivityActionPreview"), StringLength(256)]
        public string ActivityActionPreview { get; set; }

        [Required, DisplayName("ActivityActionExecute"), StringLength(256)]
        public string ActivityActionExecute { get; set; }

    }
}
