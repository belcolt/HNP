using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class ResourceSubCategory
    {
        //Might not need this 
        public int ID { get; set; }

        [MaxLength(50)]
        [Required]
        [Index("IX_ResouceSubCat_Unique", IsUnique = true)]
        public string Name { get; set; }

        [Required]
        public int ResourceCategoryID{get; set;}
        public ResourceCategory ResourceCategory {get; set;}
    }
}