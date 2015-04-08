using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Home
    {
        public IEnumerable<HospiceNiagara.Models.Announcement> Announcement { get; set; }
        public IEnumerable<HospiceNiagara.Models.DeathNotice> DeathNotice { get; set; }
        public IEnumerable<HospiceNiagara.Models.Event> Event { get; set; }
    }
}