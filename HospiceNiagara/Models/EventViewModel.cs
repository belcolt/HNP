using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class EventListViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        [DisplayName("Start")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End")]
        public DateTime? EndDate { get; set; }

        public string Location { get; set; }
        [DisplayName("RSVPs")]
        public string AttendanceCount { get; set; }

        public string Type { get; set; }
    }
    public class UserInviteViewModel
    {
        //for the Invitation
        public int InviteID { get; set; }

        public int EventID { get; set; }

        public string Name { get; set; }

        [DisplayName("Start")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End")]
        public DateTime? EndDate { get; set; }

        public string Location { get; set; }

        public string Attend { get; set; }
    }

    public class InvitationsSingleViewModel
    {
        private List<String> attend;
        private List<String> notattend;
        private List<String> noresp;

        public InvitationsSingleViewModel()
        {
            attend = new List<string>();
            notattend = new List<string>();
            noresp = new List<String>();
        }
        public List<string> Attending { get { return attend; } set { attend.Add(value.ToString()); } }
        public List<string> NotAttending { get { return notattend; } set { notattend.Add(value.ToString()); } }
        public List<string> NotResponded { get { return noresp; } set { noresp.Add(value.ToString()); } }

        public string EventName { get; set; }
    }

    //almost the same as meeting, but with strings in place of the MeetingResource nav properties
    public class EditMeetingViewModel
    {
        public int ID;
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

        public string Requirements { get; set; }

        [DisplayName("Staff Lead")]
        public string StaffLead { get; set; }

        public string Agenda { get; set; }

        public string Minutes { get; set; }

        public string Attendance { get; set; }

    }
}