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

namespace HospiceNiagara.Controllers
{
    public class AnnouncementsController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Announcements
        public ActionResult Index()
        {
            ViewBag.DNList = db.DeathNotices.ToList();
            ViewBag.AnnounceList = db.Announcements.ToList();
            return View();
        }
        // GET: Announcements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateDNotice([Bind(Include = "FirstName,MiddleName,LastName,Date,Location,Notes")] DeathNotice deathNotice)
        {
            if (ModelState.IsValid)
            {
                db.DeathNotices.Add(deathNotice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content")] Announcement announcement)
        {
            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            announcement.Date = DateTime.Now;
            byte[] fileData = new byte[fileLength];
            //hard coded until select list added
            var rType = (from rt in db.ResourceTypes
                         where rt.Description == "Announcement-Memo"
                         select rt).Single();
            var resource = new Resource { DateAdded = DateTime.Today, ResourceTypeID = rType.ID };
            //announcement dates and resource  initializing
            
            //fileread and associate file with resource
            if (!(fileName == "" || fileLength == 0))
            {
                Stream fileStream = Request.Files[0].InputStream;
                fileStream.Read(fileData, 0, fileLength);
                //resource type already existent
                resource.FileStore = new FileStore{
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName
                };
            }
            //add resource, assign ID to announcement's resource
            db.Resources.Add(resource);
            announcement.Resource = resource;
            if (ModelState.IsValid)
            {
                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        // GET: Resources/Edit/5
        [HttpGet]
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
            return PartialView("_EditModal",announcement);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Date,ResourceID")]Announcement announcement)
        {
            //string description = announcement.Resource.FileDesc;
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.AnnounceList = db.Announcements.ToList();
            return View("Index");
        }

        // GET: Resources/Delete/5
        [HttpGet]
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

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
            return View("Index");
        }
        
        //VIEW BAG DROP DOWNS AUTO GENERATED

        //ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", resource.FileStoreID);
        //ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description", resource.ResourceTypeID);

    }
}
