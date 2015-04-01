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

namespace HospiceNiagara.Controllers
{
    public class DeathNoticesController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: DeathNotices
        public ActionResult Index(bool? showModal)
        {
                return View(db.DeathNotices.ToList());
        }

        // GET: DeathNotices/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DeathNotice deathNotice = db.DeathNotices.Find(id);
        //    if (deathNotice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(deathNotice);
        //}

        // GET: DeathNotices/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_CreateModal");
        }

        // POST: DeathNotices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Date,Location,Notes,URL")] DeathNotice deathNotice)
        {
            if (ModelState.IsValid)
            {
                db.DeathNotices.Add(deathNotice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Issues:
            //return View(deathNotice); = error returning create view, which doesn't exist
            //return View("Index", deathNotice); = Error converting over to IEnumerable
            return PartialView("_CreateModal",deathNotice);
        }

        // GET: DeathNotices/Edit/5
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
            return PartialView("_EditModal", deathNotice);
        }

        // POST: DeathNotices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName,Date,Location,Notes,URL")] DeathNotice deathNotice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deathNotice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deathNotice);
        }

        // GET: DeathNotices/Delete/5
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
            return PartialView("_DeleteModal", deathNotice);
        }

        // POST: DeathNotices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeathNotice deathNotice = db.DeathNotices.Find(id);
            db.DeathNotices.Remove(deathNotice);
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
