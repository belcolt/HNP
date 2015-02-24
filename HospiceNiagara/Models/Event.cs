using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Event
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:t}")]
        [DisplayName("Start")]
        public DateTime StartTime { get; set; }

        [Required]
        [DisplayName("End")]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime EndTime { get; set; }

        //Works?
        [ForeignKey("Brochure")]
        public int? BrochureID { get; set; }

        public Resource Brochure { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}