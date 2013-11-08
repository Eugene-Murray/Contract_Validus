using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Validus.Console.DTO
{
    public class QuoteSheetDto
    {
        [Required, StringLength(256)]
        public String Title { get; set; }

        [Required]
        public String ObjectStore { get; set; }

        [Required]
        public Guid Guid { get; set; }

        [DisplayName("Issued Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedDate { get; set; }

        [DisplayName("Issued By")]
        public string IssuedBy { get; set; }

        public string ReportUrl
        {
            get
            {
                //return String.Format(ConfigurationManager.AppSettings["UWDmsFileDownloadURL"], Guid);
                return ConfigurationManager.AppSettings["UWDmsFileDownloadURL"] + "?FileID=" + Guid + "&ObjectStore=Underwriting";
            }
        }
    }
}