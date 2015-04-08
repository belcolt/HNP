﻿using System;
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
            ViewBag.AnnounceList = db.Announcements.OrderByDescending(anmt => anmt.Content).ToList();
            return View();
        }

        // GET: Announcements/Create
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
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content")] Announcement announcement, int ResourceCategoryID, string ResourceDescription)
        {
            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            announcement.Date = DateTime.Now;
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
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Date,ResourceID")]Announcement announcement, [Bind(Include = "FileDesc")]Resource resource, int ResourceCategoryID)
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
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
            return View("Index");
        }

        public FileContentResult Download(int id)
        {
            var theFile = db.FileStores.Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent, theFile.MimeType, theFile.FileName);
        }
        
        //VIEW BAG DROP DOWNS AUTO GENERATED
    }
}
