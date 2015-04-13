using HospiceNiagara.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
namespace HospiceNiagara.Controllers
{
    [Authorize(Roles="Administrator")]
    public class AdminController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Admin
        public ActionResult UserRegister()
        {
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description");
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "ID", "JobName");
            return View();
        }
        [HttpGet]
        public JsonResult LoadRoles(int? domain)
        {
            var roles = db.Roles.ToList();
            List<ApplicationRole> aRoles = new List<ApplicationRole>();
            //if (filterID != null)
            //{
            foreach (var role in roles)
            {
                ApplicationRole aRole = (ApplicationRole)role;
                if (aRole.TeamDomainID == domain)
                {
                    aRoles.Add(aRole);
                }
            }
            var sCats = aRoles.Select(sc => new { sc.Id, sc.Name }).ToList();
            return Json(aRoles, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> NewUserForm([Bind(Include = "ID,FirstName,LastName,JobDescriptionID,Phone,Email,TeamDomainID,Password,ConfirmPassword")]RegisterNewUserViewModel regUser, FormCollection fc)
        {
            
            //Roles
            var checks = fc.GetValues(9);
            List<string> roleIDs = new List<string>();
            foreach (var value in checks)
            {
                roleIDs.Add(value);
            }
            if (ModelState.IsValid)
            {
                var contact = db.Contacts.Add(new Contact
                {
                    FirstName = regUser.FirstName,
                    LastName = regUser.LastName,
                    JobDescriptionID = regUser.JobDescriptionID,
                    Phone = regUser.Phone,
                    Email = regUser.Email,
                    TeamDomainID = regUser.TeamDomainID
                });

                try
                {
                    ApplicationUser newUser = new ApplicationUser { ContactID = contact.ID, Email = regUser.Email, UserName = regUser.Email };
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var result = await manager.CreateAsync(newUser, regUser.Password);
                    if (result.Succeeded)
                    {
                        foreach (var role in roleIDs)
                        {
                            manager.AddToRole<ApplicationUser, string>(newUser.Id, role);
                        }
                        db.SaveChanges();
                    }
                    
                    return View("UserRegister");
                }
                catch
                {
                    return PartialView("UserRegister", regUser);
                }
            }
            else
            {
                return PartialView("UserRegister");
            }
        }

        [HttpGet]
        public ActionResult RoleIndex(int? filterID)
        {
            var roles = db.Roles.ToList();
            List<ApplicationRole> aRoles= new List<ApplicationRole>();
            //if (filterID != null)
            //{
                foreach (var role in roles)
                {
                    ApplicationRole aRole = (ApplicationRole)role;
                    aRoles.Add(aRole);
                }
            //}
            return View(aRoles.ToList());
        }
        public ActionResult RoleCreate()
        {
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description");
            return View("RoleCreate");
        }

        [HttpPost]
        public ActionResult RoleCreate([Bind(Include="ID,Name,TeamDomainID")]ApplicationRole role)
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            try
            {
                roleManager.Create(role);
                return RedirectToAction("RoleIndex");
            }
            catch
            {
                return View("RoleCreate");
            }
            
        }
    }
}