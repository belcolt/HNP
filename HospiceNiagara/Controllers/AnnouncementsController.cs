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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content")] Announcement announcement)
        {
            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            var resource = new Resource();
            var rType = (from rt in db.ResourceTypes
                         where rt.Description == "Annoucement-Memo"
                         select rt).Single();

            if (!(fileName == "" || fileLength == 0))
            {
                Stream fileStream = Request.Files[0].InputStream;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);
                //resource type already existent
                announcement.Date = DateTime.Now;
                resource.DateAdded = DateTime.Today;
                resource.ResourceType = rType;
                resource.FileStore = new FileStore
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName
                };
            }

            //var resource = db.ResourceFileStoreCreate(announcement.Content);
            announcement.ResourceID = resource.ID;
            if (ModelState.IsValid)
            {
                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(db.Announcements.ToList());
        }

        // GET: Resources/Edit/5
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
            //ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", resource.FileStoreID);
            //ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description", resource.ResourceTypeID);
            return View("Index");
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", resource.FileStoreID);
            //ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description", resource.ResourceTypeID);
            return View("Index");
        }
    }
}
