using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class ResourceType
    {
        public int ID { get; set; }

        [DisplayName("ResourceType")]
        [MaxLength(50)]
        [Index("IX_ResourceType_Desc",IsUnique=true)]
        public string Description { get; set; }
    }
}