using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.DAL;
using HospiceNiagara.Models;
using System.IO;
using System.Web.Services;
using Microsoft.AspNet.Identity;
namespace HospiceNiagara.Controllers
{
    public class ResourcesController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        //Enumeration for quick matching of Domain IDs as in the Hospice Database.
        //Example: Check for specific DomainID on the ResourceCategory with db.ResourceCategories.Where(rc=>rc.TeamDomainID==(int)Domains.[nameOfDomain] and so forth
        private string volunteerRoles;

        // GET: Resources
        
        [Authorize(Roles="Administrator")]
        public ActionResult Index(string SearchString)
        {
            var resources = db.Resources.Include(r => r.FileStore).Include(r => r.ResourceCategory);
            ViewBag.VolunteerResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 1).ToList();
            ViewBag.StaffResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 2).ToList();
            ViewBag.OrgResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 4).ToList();
            ViewBag.BoardResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 3).ToList();
            if (!String.IsNullOrEmpty(SearchString))
            {
                resources = resources.Where(d => d.FileDesc.ToUpper().Contains(SearchString.ToUpper()));
                ViewBag.Resources = resources.ToList();
                return View(resources);
            }
            ViewBag.Resources = resources.ToList();
            //ViewBag.VolunteerResources = vRes;
            return View(resources);
        }

        public FileContentResult Download(int id)
        {
            var theFile = db.FileStores.Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent, theFile.MimeType, theFile.FileName);
        }
        //Details Views

        // GET: Resources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // GET: Resources/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            var rcs = db.ResourceCategories.OrderBy(rc => rc.TeamDomainID).ToList();
            var teamNames = db.TeamDomains.OrderBy(td => td.ID).Select(td => td.Description).ToList();
            ViewBag.AllSubCats = db.ResourceSubCategories.ToList();
            List<ResourceCatDD> rcats = new List<ResourceCatDD>();
            foreach (var rc in rcs)
            {
                rcats.Add(new ResourceCatDD { ResourceCategoryID = rc.ID, RCatName = rc.Name, TeamDomainName = teamNames[rc.TeamDomainID - 1] });
            }
            ViewBag.ResourceCategoryID = new SelectList(rcats, "ResourceCategoryID", "RCatName", "TeamDomainName", null, null, null);
            return View();
        }

        public JsonResult LoadSubcategories(int catID)
        {
            var sCats = db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == catID).Select(sc => new { sc.ID, sc.Name });
            return Json(sCats, JsonRequestBehavior.AllowGet);

        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "ID,FileDesc,ResourceCategoryID,ResourceSubCategoryID")] Resource resource)
        {
            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            if (!(fileName == "" || fileLength == 0))
            {
                Stream fileStream = Request.Files[0].InputStream;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);
                //resource type already existent
                resource.DateAdded = Convert.ToDateTime(DateTime.Now);
                resource.FileStore = new FileStore
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName
                };
            };
            if (resource.ResourceSubCategoryID==-1)
            {
                resource.ResourceSubCategoryID = null;
            }
            if (ModelState.IsValid)
            {
                db.Resources.Add(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResourceCategoryD = new SelectList(db.ResourceCategories, "ID", "Name", resource.ResourceCategoryID);
            return View(resource);
        }

        // GET: Resources/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", resource.FileStoreID);
            ViewBag.ResourceCategoryID = new SelectList(db.ResourceCategories, "ID", "Name", resource.ResourceCategoryID);
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "ID,FileDesc,ResourceTypeID,FileStoreID")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", resource.FileStoreID);
            ViewBag.ResourceTypeID = new SelectList(db.ResourceCategories, "ID", "Name", resource.ResourceCategoryID);
            return View(resource);
        }

        // GET: Resources/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        public string VolunteerRoles
        {
            get
            {
                List<String> roles = new List<String>();
                string rolesSingle = "";
                db.Roles.Where(r => r.TeamDomainID == (int)Domain.Volunteer).Select(r => r.Name).ToList().ForEach(s=>roles.Add(s));
                foreach(string s in roles)
                {
                    rolesSingle += s + ",";
                }
                return rolesSingle;
            }
        }
        //        public string HospiceSubRoles(Domain domain)
        //{
        //    List<String> roles = new List<String>();
        //    string rolesSingle = "";
        //    db.Roles.Where(r => r.TeamDomainID == (int)domain).Select(r => r.Name).ToList().ForEach(s=>roles.Add(s));
        //    foreach(string s in roles)
        //    {
        //        rolesSingle += s + ",";
        //    }
        //    return rolesSingle;
        //}

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = db.Resources.Find(id);
            db.Resources.Remove(resource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}