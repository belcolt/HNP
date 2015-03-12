using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Meeting : Event
    {

        public string Requirements { get; set; }

        public string Notes { get; set; }

        public int? AgendaId { get; set; }
        public MeetingResource Agenda { get; set; }

        public int? MinutesID { get; set; }
        public MeetingResource Minutes { get; set; }

        public string StaffLead { get; set; }

        public int? AttendanceID { get; set; }
        public MeetingResource Attendance { get; set; }
    }
}