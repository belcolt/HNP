using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class MeetingResource
    {
        public int ID { get; set; }

        [Required]
        public int MeetingID { get; set; }
        [ForeignKey("MeetingID")]
        public virtual Meeting Meeting { get; set; }

        [Required]
        public int ResourceID { get; set; }
        [ForeignKey("ResourceID")]
        public virtual Resource Resource { get; set; }
    }
}