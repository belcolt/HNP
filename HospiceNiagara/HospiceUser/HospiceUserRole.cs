using HospiceNiagara.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using HospiceNiagara.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
namespace HospiceNiagara.HospiceUserExtensions
{

    public static class HospiceUserExtensions
    {
        //enum Domains { Volunteer=1, Staff=2, Board=3, Organizational=4 };
        enum Domains { Volunteer=1, Staff, Board, Organizational };
        public static bool IsInDomain(this IPrincipal User, String checkDomain)
        {
            HospiceNiagaraContext db = new HospiceNiagaraContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string id = User.Identity.GetUserId();
            int contactID = manager.FindById(id).ContactID;
            var contact = db.Contacts.Find(contactID);
            int userDomID = contact.TeamDomainID;
                Domains ud = (Domains)userDomID;
                string domain = ud.ToString();
                if (checkDomain == domain)
                {
                    return true;
                }
                else
                    return false;
        }
    }
}