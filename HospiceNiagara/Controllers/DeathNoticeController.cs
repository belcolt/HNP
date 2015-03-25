using HospiceNiagara.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.Models;
using System.IO;
using System.Data;
using System.Net;
using System.Data.Entity;

namespace HospiceNiagara.Controllers
{
    public class DeathNoticeController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();
 
        // GET: DeathNotice
        public ActionResult Index()
        {
            ViewBag.DNList = db.DeathNotices.ToList();
            return View("Index");
        }

        //// GET: DeathNotice/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: DeathNotice/Create
        public ActionResult Create()
        {
            return View("Index");
        }

        // POST: DeathNotice/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName,MiddleName,LastName,Date,Location,Notes")] DeathNotice deathNotice)
        {
            if(ModelState.IsValid)
            {
                db.DeathNotices.Add(deathNotice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        // GET: DeathNotice/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeathNotice deathNotice = db.DeathNotices.Find(id);
            if (deathNotice == null)
            {
                return HttpNotFound();
            }
            return View("Index");
        }

        // POST: DeathNotice/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName,Date,Location,Notes")] DeathNotice deathNotice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deathNotice).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.DNList = db.DeathNotices.ToList();
            return View("Index");
        }

        // GET: DeathNotice/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeathNotice deathNotice = db.DeathNotices.Find(id);
            if (deathNotice == null)
            {
                return HttpNotFound();
            }
            return View("Index");
        }

        // POST: DeathNotice/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            DeathNotice deathNotice = db.DeathNotices.Find(id);
            db.DeathNotices.Remove(deathNotice);
            db.SaveChanges();
            return View("Index");
        }
    }
}
