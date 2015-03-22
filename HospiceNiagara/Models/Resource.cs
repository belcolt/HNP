using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Resource
    {
        public int ID { get; set; }

        //File link, need to make sure, File does not get loaded
        [Required]
        [DisplayName("Description")]
        public string FileDesc { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }

        //ResourceType
        [Required]
        [DisplayName("Resource Category")]
        public int ResourceCategoryID { get; set; }
        public virtual ResourceCategory ResourceCategory { get; set; }

        [DisplayName("Resource SubCategory")]
        public int? ResourceSubCategoryID { get; set; }
        public virtual ResourceSubCategory ResourceSubCategory { get; set; }

        [ForeignKey("FileStore")]
        [DisplayName("Attach File")]
        public int FileStoreID { get; set; }
        public virtual FileStore FileStore { get; set; }

        

    }
}