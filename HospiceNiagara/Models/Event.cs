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

        [DisplayName("Event")]
        public string Name { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:t}")]
        [DisplayName("Start Time")]
        public string StartTime { get { return Convert.ToString(StartDateTime.TimeOfDay); } }

        [DisplayName("Start Day")]
        public string StartDay { get { return Convert.ToString(StartDateTime.DayOfYear); } }


        [DisplayName("End Time")]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public string EndTime { get { return Convert.ToString(EndDateTime.TimeOfDay); } }
        
        [Required]
        public string Location { get; set; }
        
        [DefaultValue(false)]
        public bool VolunteersNeeded { get; set; }

        [ForeignKey("Brochure")]
        public int? BrochureID { get; set; }
        public Resource Brochure { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}