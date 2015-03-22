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
        [Required]
        [Index("IX_ResouceDomCat_Unique",IsUnique=true,Order=1)]
        public string Name{ get; set; }

        [Required]
        [Index("IX_ResouceDomCat_Unique", IsUnique = true,Order=2)]
        public int TeamDomainID { get; set; }
        public TeamDomain TeamDomain { get; set; }
    }
}