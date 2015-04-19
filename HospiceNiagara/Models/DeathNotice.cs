using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class DeathNotice
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(30, ErrorMessage = "First name cannot be more than 30 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30, ErrorMessage = "Middle name cannot be more than 30 characters long.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(50, ErrorMessage = "Last name cannot be more than 50 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You cannot leave the date of passing blank.")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime Date { get; set; }

        [StringLength(50, ErrorMessage = "Location cannot be more than 50 characters long.")]
        public string Location { get; set; }

        [StringLength(50, ErrorMessage = "Notes cannot be more than 50 characters long.")]
        public string Notes { get; set; }

        [StringLength(2048, ErrorMessage = "URL cannot be more than 2048 characters long.")]
        public string URL { get; set; }

        [Required(ErrorMessage = "You cannot leave the expiry date of the post blank.")]
        [Display(Name = "Expiry Date of Post")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ExpiryDate { get; set; }
        
        [Display(Name = "Posted On")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime PostDate { get; set; }

        public bool IsNew { get; set; }
    }
}