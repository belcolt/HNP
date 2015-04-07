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

        [Required(ErrorMessage = "You cannot leave the title/description blank.")]
        [DisplayName("Title or Description")]
        public string Content { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime Date { get; set; }

        public int ResourceID { get; set; }
        public virtual Resource Resource { get; set; }
    }
}