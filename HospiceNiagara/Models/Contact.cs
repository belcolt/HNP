using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Models
{
    public class Contact
    {
        public int ID { get; set; }

        [DisplayName("Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [DisplayName("Years of Service")]
        public int YearsOfService
        {
            get
            {
                return DateTime.Now.Year - DateHired.Year;
            }
        }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //[Required]
        //[DisplayName("Position")]
        //public string Position { get; set; }

        //extension or number
        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }

        public string Email { get; set; }

        [DisplayName("Date Hired")]
        public DateTime DateHired
        {
            get;
            set;
        }

        
        //[DefaultValue(false)]
        //[DisplayName("Board Director")]
        //public bool IsBoardDirector { get; set; }

        [Required]
        public int TeamDomainID { get; set; }
        [DisplayName("Team")]
        public virtual TeamDomain TeamDomain { get; set; }

        [Required]
        public int JobDescriptionID { get; set; }
        [DisplayName("Position")]
        public virtual JobDescription JobDescription { get; set; }

        //public ICollection<Meeting> Invitations { get; set; }
        [DisplayName]
        public virtual ICollection<Invitation> Invitations { get; set; }

        public List<string> ContactList { get; set; }
    }
}