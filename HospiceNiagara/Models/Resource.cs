using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Resource
    {
        public int ID { get; set; }

        //File link, need to make sure, File does not get loaded
        [DisplayName("Description")]
        public string FileDesc { get; set; }

        //ResourceType
        [DisplayName("Resource Type")]
        public int ResourceTypeID { get; set; }
        public ResourceType ResourceType { get; set; }


        [ForeignKey("FileStore")]
        [DisplayName("Attach File")]
        public int FileStoreID { get; set; }
        public virtual FileStore FileStore { get; set; }

        public DateTime DateAdded { get; set; }

    }
}

