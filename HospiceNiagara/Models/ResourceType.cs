using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class ResourceType
    {
        public int ID { get; set; }

        [DisplayName("ResourceType")]
        [MaxLength(50)]
        public string Description { get; set; }
    }
}