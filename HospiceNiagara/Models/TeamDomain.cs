using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class TeamDomain
    {
        public int ID { get; set; }

        [MaxLength(50)]
        [Required]
        [Index("IX_TeamDomain_Unique", IsUnique = true)]
        public string Description { get; set; }
    }
}