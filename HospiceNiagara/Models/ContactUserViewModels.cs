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
        [Required(ErrorMessage = "Please enter a first name for this user.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a last name for this user.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required (ErrorMessage="Please enter an email for this user.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage = "Passwords do not match.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Job Description")]
        public int JobDescriptionID { get; set; }
        public JobDescription JobDescription { get; set; }

        [Required(ErrorMessage = "Please select a team domain.")]
        [DisplayName("Team")]
        public int TeamDomainID { get; set; }
        public virtual TeamDomain TeamDomain { get; set; }

        //extension or number
        [Required(ErrorMessage="Please enter a phone number for this user.")]
        [MaxLength(10)]
        public string Phone { get; set; }

        [DefaultValue(false)]
        [DisplayName("Board Director")]
        public bool IsBoardDirector { get; set; }

        public List<string> Roles { get; set; }
    }
}