using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class JobDescription
    {
        public int ID { get; set; }

        [DisplayName("Position")]
        public string JobName { get; set; }

        public string Description { get; set; }
    }
}