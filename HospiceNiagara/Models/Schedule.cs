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

        [DisplayName("Resource Type")]
        public int ResourceTypeID { get; set; }
        public ResourceType ResourceType { get; set; }

        

        [ForeignKey("FileStore")]
        [DisplayName("Attach File")]
        public int FileStoreID { get; set; }
        public virtual FileStore FileStore { get; set; }



    }
}