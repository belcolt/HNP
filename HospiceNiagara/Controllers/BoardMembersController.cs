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
    public class BoardMembersController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: BoardMembers
        public ActionResult Index()
        {
            return View(db.BoardMembers.ToList());
        }

        public PartialViewResult BoardMemberList(string SearchString)
        {
            
            //var directors = db.BoardMembers.AsEnumerable();
            if (!String.IsNullOrEmpty(SearchString))
            {
                var directors = db.BoardMembers.Where(d => d.LastName.ToUpper().Contains(SearchString.ToUpper())
                                       || d.FirstName.ToUpper().Contains(SearchString.ToUpper()));
                ViewBag.Directors = directors;
                return PartialView();
            }
            else
            {
                 var directors = db.BoardMembers.ToList();
                 ViewBag.Directors = directors;
                 return PartialView();
            }

           
        }

        // GET: BoardMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardMember boardMember = db.BoardMembers.Find(id);
            if (boardMember == null)
            {
                return HttpNotFound();
            }
            return View(boardMember);
        }

        // GET: BoardMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoardMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Position,EmailAddress,HomeAddress,BusinessAddress,HomePhone,BusinessPhone,Fax,PartnerName")] BoardMember boardMember)
        {
            if (ModelState.IsValid)
            {
                db.BoardMembers.Add(boardMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(boardMember);
        }

        // GET: BoardMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardMember boardMember = db.BoardMembers.Find(id);
            if (boardMember == null)
            {
                return HttpNotFound();
            }
            return View(boardMember);
        }

        // POST: BoardMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Position,EmailAddress,HomeAddress,BusinessAddress,HomePhone,BusinessPhone,Fax,PartnerName")] BoardMember boardMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boardMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boardMember);
        }

        // GET: BoardMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardMember boardMember = db.BoardMembers.Find(id);
            if (boardMember == null)
            {
                return HttpNotFound();
            }
            return View(boardMember);
        }

        // POST: BoardMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BoardMember boardMember = db.BoardMembers.Find(id);
            db.BoardMembers.Remove(boardMember);
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
