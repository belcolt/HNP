using System;
using System.Collections.Generic;
using System.ComponentModel;
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
}