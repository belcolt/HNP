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
        enum Domains { Volunteer=1, Staff=2, Board=3, Organizational=4};
        private static HospiceNiagaraContext db = new HospiceNiagaraContext();

        public static bool IsInDomain(this IPrincipal User, String checkDomain)
        {
            
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

        public static bool HasDNView(this IPrincipal User)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string id = User.Identity.GetUserId();
            int contactID = manager.FindById(id).ContactID;
            string nonClientRoles = "";
            var aRoles = db.Roles.ToList();
            foreach(ApplicationRole aRole in aRoles)
            {
                if (aRole.NonClient)
                {
                    nonClientRoles+= aRole.Name+",";
                }
            }
            //var contact = db.Contacts.Find(contactID);
            string ud = ((Domains)db.Contacts.Find(contactID).TeamDomainID).ToString();
            if (ud=="Volunteer")
            {
                var roles = manager.GetRoles(id);
                foreach (string role in roles)
                {
                    if(!(nonClientRoles.Contains(role)))
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
    }
}