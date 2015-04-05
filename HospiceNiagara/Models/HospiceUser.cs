using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace HospiceNiagara.Models
{
    public class HospiceCurrentUserRoles
    {
        public bool IsVolunteer{get; set;}
    }

    public class HospiceSubRoles
    {
        public List<String> VolunteerSubRoles { get; set; }
        public List<String> BoardSubRoles { get; set; }
        public List<String> StaffSubRoles { get; set; }
    }
}