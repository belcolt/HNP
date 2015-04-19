using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public abstract class HospiceDate
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a title for this meeting.")]
        [DisplayName("Event")]
        public string Name { get; set; }

        //Dates
        [Required(ErrorMessage = "Please enter a start time for this meeting.")]
        [DisplayName("Starts")]
        public DateTime? StartDateTime { get; set; }

        [Required(ErrorMessage = "Please enter an end time for this meeting.")]
        [DisplayName("Ends")]
        public DateTime? EndDateTime { get; set; }

        [Required(ErrorMessage = "Please enter a location for this meeting.")]
        public string Location { get; set; }

        public string Notes { get; set; }

        [DisplayName("Staff Lead")]
        public string StaffLead { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
    public class Meeting : HospiceDate
    {

        //Meeting Resources
        public int? AgendaID { get; set; }
        public MeetingResource Agenda { get; set; }

        public int? MinutesID { get; set; }
        public MeetingResource Minutes { get; set; }

        public int? AttendanceID { get; set; }
        public MeetingResource Attendance { get; set; }

        public string Requirements { get; set; }
    }
    public class Event:HospiceDate
    {
        [DefaultValue(false)]
        public bool VolunteersNeeded { get; set; }

        public int? BrochureId { get; set; }
        public MeetingResource Brochure { get; set; }

    }
}