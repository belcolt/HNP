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
        public DateTime StartDate { get; set; }

        [DisplayName("End")]
        public DateTime EndDate { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }
        [DisplayName("RSVPs")]
        public string AttendanceCount { get; set; }
    }
}