using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    //View model for person as new contact and user
    public class RegisterNewUserViewModel
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("JobDescription")]
        public int JobDescriptionID { get; set; }
        public JobDescription JobDescription { get; set; }

        //extension or number
        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }

        [DefaultValue(false)]
        [DisplayName("Board Director")]
        public bool IsBoardDirector { get; set; }

        [Required]
        public int TeamDomainID { get; set; }
        [DisplayName("Team")]
        public virtual TeamDomain TeamDomain { get; set; }

        public List<string> Roles { get; set; }
    }
}