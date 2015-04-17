using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Announcement
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the title of the announcment blank.")]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "You cannot leave the expiry date of the post blank.")]
        [Display(Name = "Expiry Date of Post")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Posted On")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime PostDate { get; set; }

        public bool IsNew { get; set; }

        public int ResourceID { get; set; }
        public virtual Resource Resource { get; set; }
    }
}