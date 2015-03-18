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
    public class Schedules1Controller : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Schedules1
        public ActionResult Index(int? ID)
        {
            var schedules = db.Schedules.Include(s => s.FileStore).Include(s => s.ResourceType);
            SelectList selectList = new SelectList(db.Schedules, "FileStoreID", "Month");
            ViewBag.ID = new SelectList(db.Schedules, "FileStoreID", "Month");
            ViewBag.SelectList = selectList;
            return View(schedules.ToList());
        }

        // GET: Schedules1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules1/Create
        public ActionResult Create()
        {
            ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType");
            ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description");
            return View();
        }

        // POST: Schedules1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Category,Month,Year,ResourceTypeID")] Schedule schedule)
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
                
                schedule.FileStore = new FileStore
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName
                };
            };

            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", schedule.FileStoreID);
            ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description", schedule.ResourceTypeID);
            return View(schedule);
        }

        // GET: Schedules1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", schedule.FileStoreID);
            ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description", schedule.ResourceTypeID);
            return View(schedule);
        }

        // POST: Schedules1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Category,Month,Year,ResourceTypeID,FileStoreID")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FileStoreID = new SelectList(db.FileStores, "ID", "MimeType", schedule.FileStoreID);
            ViewBag.ResourceTypeID = new SelectList(db.ResourceTypes, "ID", "Description", schedule.ResourceTypeID);
            return View(schedule);
        }

        // GET: Schedules1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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
