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
using HospiceNiagara.HospiceUserExtensions;
using System.Security.Principal;
using System.Web.Security;

namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Announcements
        [SessionTracking.Logging]
        public ActionResult Index(bool? newUser)
        {
            string username = User.Identity.Name;
            var user = (from u in db.Users
                        where u.UserName.ToString() == username
                        select u).Single();

            foreach (var anmt in db.Announcements.ToList())
            {
                //Expiry date check
                if (DateTime.Now > anmt.ExpiryDate)
                {
                    db.Announcements.Remove(anmt);
                }

                //Lets user know if a post is new
                if (user.LastLoggedIn <= anmt.PostDate)
                {
                    anmt.IsNew = true;
                }
                else
                {
                    anmt.IsNew = false;
                }
            }
            db.SaveChanges();
            if (newUser.HasValue)
            {
                if (newUser.Value)
                {
                    int team = User.GetHospiceContact().TeamDomainID;
                    string teamName = ((Domains)team).ToString();
                    string clickHere = "This is the annoucements page. This page will serve as a heads-up for any releveant Hospice Niagara updates and news.";
                    ViewBag.NewUserMessage = clickHere;
                }
            }
            return View(db.Announcements.OrderByDescending(anmt => anmt.PostDate).ToList());
        }

        // GET: ResourceSubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement anmt = db.Announcements.Find(id);
            if (anmt == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details", anmt);
        }

        // GET: Announcements/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            var rcs = db.ResourceCategories.OrderBy(rc => rc.TeamDomainID).ToList();
            var teamNames = db.TeamDomains.OrderBy(td => td.ID).Select(td => td.Description).ToList();
            List<ResourceCatDD> rcats = new List<ResourceCatDD>();
            foreach (var rc in rcs)
            {
                rcats.Add(new ResourceCatDD { ResourceCategoryID = rc.ID, RCatName = rc.Name, TeamDomainName = teamNames[rc.TeamDomainID - 1] });
            }
            ViewBag.ResourceCategoryID = new SelectList(rcats, "ResourceCategoryID", "RCatName", "TeamDomainName", null, null, null);
            return PartialView("_CreateModal");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content,ExpiryDate")] Announcement announcement, int ResourceCategoryID, string ResourceDescription)
        {
            announcement.PostDate = DateTime.Now;

            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            byte[] fileData = new byte[fileLength];
            //hard coded until select list added
            var rCat = (from rc in db.ResourceCategories
                            where rc.ID == ResourceCategoryID
                            select rc).Single();
            var resource = new Resource { DateAdded = DateTime.Today, ResourceCategoryID = rCat.ID,FileDesc=ResourceDescription };
            //announcement dates and resource  initializing

            //fileread and associate file with resource
            if (!(fileName == "" || fileLength == 0))
            {
                Stream fileStream = Request.Files[0].InputStream;
                fileStream.Read(fileData, 0, fileLength);
                //resource type already existent
                resource.FileStore = new FileStore
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName
                };
                db.Resources.Add(resource);
                announcement.Resource = resource;
            }
            //add resource, assign ID to announcement's resource
                
            
            if (ModelState.IsValid)
            {
                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("_CreateModal", announcement);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnnounceList = db.Announcements.ToList();
            var rcs = db.ResourceCategories.OrderBy(rc => rc.TeamDomainID).ToList();
            var teamNames = db.TeamDomains.OrderBy(td => td.ID).Select(td => td.Description).ToList();
            List<ResourceCatDD> rcats = new List<ResourceCatDD>();
            foreach (var rc in rcs)
            {
                rcats.Add(new ResourceCatDD { ResourceCategoryID = rc.ID, RCatName = rc.Name, TeamDomainName = teamNames[rc.TeamDomainID - 1] });
            }
            int rCatID = announcement.Resource.ResourceCategoryID;
            ViewBag.ResourceCategories= new SelectList(rcats, "ResourceCategoryID", "RCatName", "TeamDomainName", rCatID);
            
            return PartialView("_EditModal",announcement);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,ExpiryDate,PostDate,ResourceID")]Announcement announcement, [Bind(Include = "FileDesc")]Resource resource, int ResourceCategoryID)
        {
            //store user input before querying the object refreshes values
            string fileDesc = resource.FileDesc;
            int rCatID = ResourceCategoryID;

            //match the announcement record to the resource record
            resource = (from rc in db.Resources
                            where rc.ID == announcement.ResourceID
                            select rc).Single();
            
            if(TryUpdateModel(resource, new string[] {"FileDesc,ResourceCategoryID"}))
            {
                //populate user values to related fields
                resource.FileDesc = fileDesc;
                resource.ResourceCategoryID = rCatID;

                //update the resource record
                db.Entry(resource).State = EntityState.Modified;
            }

            if (ModelState.IsValid)
            {
                //yeah
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.AnnounceList = db.Announcements.ToList();
            return PartialView("_EditModal", announcement);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DeleteModal", announcement);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileContentResult Download(int id)
        {
            var theFile = db.FileStores.Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent, theFile.MimeType, theFile.FileName);
        }
    }
}
