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

namespace HospiceNiagara.Controllers
{
    public class JobDescriptionsController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: JobDescriptions
        public ActionResult Index()
        {
            return View(db.JobDescriptions.ToList());
        }

        // GET: JobDescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDescription jobDescription = db.JobDescriptions.Find(id);
            if (jobDescription == null)
            {
                return HttpNotFound();
            }
            return View(jobDescription);
        }

        // GET: JobDescriptions/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: JobDescriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,JobName")] JobDescription jobDescription, string Text)
        {
            if (ModelState.IsValid)
            {
                List<string> points = new List<string>();
                foreach(string s in Text.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    points.Add("<li>" + s + "</li>");
                }
                string test = "<ol>"+String.Join("",points)+"</ol>";
                jobDescription.Description = test;
                db.JobDescriptions.Add(jobDescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobDescription);
        }

        public ActionResult ShowDesc(int id)
        {
            JobDescription job = db.JobDescriptions.Where(j => j.ID == id).Single();
            ViewBag.JobName = job.JobName;
            ViewBag.Desc = job.Description;
            return PartialView("ShowDesc");
        }

        // GET: JobDescriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDescription jobDescription = db.JobDescriptions.Find(id);
            ViewBag.Words = jobDescription.Description.Replace("\n\n", "\n").Replace("<ol>", "\n").Replace("</ol>", "").Replace("<li>", System.Environment.NewLine).Replace("</li>", "").Trim();
            
            if (jobDescription == null)
            {
                return HttpNotFound();
            }
            return View(jobDescription);
        }

        // POST: JobDescriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,JobName")] JobDescription jobDescription, string Text)
        {
            if (ModelState.IsValid)
            {
                List<string> points = new List<string>();
                foreach (string s in Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    points.Add("<li>" + s + "</li>");
                }
                string test = "<ol>" + String.Join("", points) + "</ol>";
                jobDescription.Description = test;
                db.Entry(jobDescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobDescription);
        }

        // GET: JobDescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDescription jobDescription = db.JobDescriptions.Find(id);
            if (jobDescription == null)
            {
                return HttpNotFound();
            }
            return View(jobDescription);
        }

        // POST: JobDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobDescription jobDescription = db.JobDescriptions.Find(id);
            db.JobDescriptions.Remove(jobDescription);
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
