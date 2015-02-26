using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Invitation
    {
        public int ID { get; set; }

        [DefaultValue(false)]
        [DisplayName("Attending")]
        public bool RSVP { get; set; }

        [DefaultValue(false)]
        public bool HasResponded { get; set; }
        
        //keys
        [Required]
        public int ContactID { get; set; }
        [Required]
        public int EventID { get; set; }
        //navigation property
        public virtual Contact Contact { get; set; }
        public virtual Event Event { get; set; }
    }
}