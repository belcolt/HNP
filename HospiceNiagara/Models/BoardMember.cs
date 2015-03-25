using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class BoardMember
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("Home Address")]
        public string HomeAddress { get; set; }

        [Required]
        [DisplayName("Business Address")]
        public string BusinessAddress { get; set; }

        [Required]
        [MaxLength(10)]
        [DisplayName("Home Phone")]
        public string HomePhone { get; set; }
        
        [Required]
        [MaxLength(10)]
        [DisplayName("Business Phone")]
        public string BusinessPhone { get; set; }

        [Required]
        [MaxLength(10)]
        public string Fax { get; set; }

        [Required]
        [DisplayName("Partner Name")]
        public string PartnerName { get; set; }

        public List<string> BoardList { get; set; }
    }
}