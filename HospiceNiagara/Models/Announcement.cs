using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Announcement
    {
        public int ID { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }

        public int? ResourceID;
        public virtual Resource Resource { get; set; }
    }
}