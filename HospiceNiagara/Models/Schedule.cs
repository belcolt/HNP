using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Schedule
    {
        public int ID { get; set; }

        public string Category { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }

        [DisplayName("Resource")]
        public int ResourceID { get; set; }
        public Resource Resource { get; set; }

    }
}