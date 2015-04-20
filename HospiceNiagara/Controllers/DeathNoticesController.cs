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

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class DeathNoticesController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: DeathNotices
        [Authorize(Roles="Administrator,Staff,Board,Organizational,Volunteer,Day Hospice,Welcome Desk,Residential,New Volunteers,Community,Bereavement")]
        public ActionResult Index(bool? showModal)
        {
            string username = User.Identity.Name;
            var user = (from u in db.Users
                        where u.UserName.ToString() == username
                        select u).Single();   

            foreach(var dn in db.DeathNotices.ToList())
            {
                //Expiry date check
                if(DateTime.Now > dn.ExpiryDate)
                {
                    db.DeathNotices.Remove(dn);
                }

                //Lets user know if a post is new
                if ((user.LastLoggedIn <= dn.PostDate))
                {
                    dn.IsNew = true;
                }
                else
                {
                    dn.IsNew = false;
                }
            }
            db.SaveChanges();

            return View(db.DeathNotices.OrderByDescending(dn => dn.Date).ToList());
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
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return PartialView("_CreateModal");
        }

        // POST: DeathNotices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrator")]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Date,Location,Notes,URL,ExpiryDate")] DeathNotice deathNotice)
        {
            if(!(deathNotice.URL.StartsWith("http://")))
                deathNotice.URL = "http://" + deathNotice.URL;

            deathNotice.PostDate = DateTime.Now;

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
        [Authorize(Roles="Administrator")]
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
        [Authorize(Roles="Administrator")]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName,Date,Location,Notes,URL,ExpiryDate,PostDate")] DeathNotice deathNotice)
        {
            if (!(deathNotice.URL == null) && !(deathNotice.URL.StartsWith("http://")))
                deathNotice.URL = "http://" + deathNotice.URL;

            if (ModelState.IsValid)
            {
                db.Entry(deathNotice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("_EditModal", deathNotice);
        }

        // GET: DeathNotices/Delete/5
        [Authorize(Roles="Administrator")]
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
        [Authorize(Roles="Administrator")]
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
