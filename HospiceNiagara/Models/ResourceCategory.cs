using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class ResourceCategory
    {
        public int ID { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage="Please enter a name for this category")]
        [Index("IX_ResouceDomCat_Unique",IsUnique=true,Order=1)]
        public string Name{ get; set; }

        [Required(ErrorMessage="Please select a domain for this category")]
        [Index("IX_ResouceDomCat_Unique", IsUnique = true,Order=2)]
        public int TeamDomainID { get; set; }
        public TeamDomain TeamDomain { get; set; }

        [DefaultValue(false)]
        public bool Panel { get; set; }
    }

    public class ResourceCategoryViewModel
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public string TeamDomain { get; set; }

        public bool Panel { get; set; }
    }
}