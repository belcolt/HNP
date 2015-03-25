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
using System.Drawing;
using System.Drawing.Imaging;

namespace HospiceNiagara.Controllers
{
    public class SchedulesController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.Resource);
            return View(schedules.ToList());
        }

        [ActionName("ShowSchedule")]
        public ActionResult Index(int id)
        {
            var schedules = db.Schedules.Include(s => s.Resource);
            var fileID = db.Resources.Where(i => i.ID == id).Select(i => i).Take(1);
            var theFile = db.FileStores.Where(f => f.ID == fileID.FirstOrDefault().FileStoreID).SingleOrDefault();
            var length = theFile.FileContent.Length;

            string string64 = Convert.ToBase64String(theFile.FileContent);
            ViewBag.Mime = theFile.MimeType;
            ViewBag.Name = theFile.FileName;
            ViewBag.FileBits = string64;
            return View();
        }



        // GET: Schedules/Details/5
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

        // GET: Schedules/Create
        public ActionResult Create()
        {
            //29 is Schedules category, might need to make this morning dynamic in future
            var schedSC = db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == 8);
            ViewBag.ResourceSubcategory = new SelectList(schedSC, "Name", "Name");
            MonthsDropdown();
            YearDropDown();
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Year")] Schedule schedule, string ResourceSubcategory, string SchedMonth, string SchedYear)
        {
            string mimeType = Request.Files[0].ContentType;

            if (SchedMonth.Length == 0)
            {
                ModelState.AddModelError("Month", "Please select a month from the dropdown list.");
            }
            if (ResourceSubcategory.Length == 0)
            {
                ModelState.AddModelError("Resource", "Please select a category from the dropdown list.");
            }
            if (SchedYear.Length == 0)
            {
                ModelState.AddModelError("Year", "Please select a year from the dropdown list.");
            }

            if (!mimeType.Contains("image/jpeg"))
            {
                ModelState.AddModelError("Resource.FileStore.MimeType", "The schedule must be in JPEG format.");
            }
            if (ModelState.IsValid)
            {

                string fileName = Path.GetFileName(Request.Files[0].FileName);
                int fileLength = Request.Files[0].ContentLength;

                schedule.Category = ResourceSubcategory;
                schedule.Month = SchedMonth;
                schedule.Year = Convert.ToInt32(SchedYear);
                string schedDescript = ResourceSubcategory + ": " + @schedule.Month + " " + @schedule.Year;
                var subCat = db.ResourceSubCategories.Where(rsc => rsc.Name == ResourceSubcategory).Single();
                var resource = new Resource { FileDesc = schedDescript, DateAdded = DateTime.Now, ResourceCategoryID = 8, ResourceSubCategory = subCat };
                if (!(fileName == "" || fileLength == 0))
                {
                    Stream fileStream = Request.Files[0].InputStream;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    resource.FileStore = new FileStore
                    {
                        FileContent = fileData,
                        MimeType = mimeType,
                        FileName = fileName
                    };
                    db.Resources.Add(resource);
                    db.SaveChanges();
                    schedule.Resource = resource;
                };
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ResourceID = new SelectList(db.Resources, "ID", "FileDesc", schedule.ResourceID);
            YearDropDown();
            MonthsDropdown();
            var schedSC = db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == 8);
            ViewBag.ResourceSubcategory = new SelectList(schedSC, "Name", "Name");
            return View(schedule);
        }

        // GET: Schedules/Edit/5
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
            MonthsDropdown();
            YearDropDown(schedule.Year);
            ViewBag.ResourceID = new SelectList(db.Resources, "ID", "FileDesc", schedule.ResourceID);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Category,Year,ResourceID")] Schedule schedule, string SchedMonth, string SchedYear)
        {
            if (SchedYear.Length == 0)
            {
                ModelState.AddModelError("Year", "Please select a year from the dropdown list.");
            }
            if (SchedMonth.Length == 0)
            {
                ModelState.AddModelError("Month", "Please select a month from the dropdown list.");
            }
            if (ModelState.IsValid)
            {
                schedule.Month = SchedMonth;
                schedule.Year = Convert.ToInt32(SchedYear);
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            MonthsDropdown();
            YearDropDown(schedule.Year);
            ViewBag.ResourceID = new SelectList(db.Resources, "ID", "FileDesc", schedule.ResourceID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
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

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            Resource delRes = db.Resources.Find(schedule.ResourceID);
            db.Resources.Remove(delRes);
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

        private void MonthsDropdown()
        {
            List<string> months = new List<String>{"January","February","March","April","May","June","July","August",
                "Sepetember","October","November","December"};
            ViewBag.SchedMonth = new SelectList(months);
        }

        private void YearDropDown(object selectedYear = null)
        {

            int thisYear = DateTime.Now.Year;
            List<string> years = new List<string>();
            for (int i = 0; i <= 10; i++)
            {

                years.Add(thisYear.ToString());
                thisYear++;
            }
            ViewBag.SchedYear = new SelectList(years, selectedYear);


        }
    }
}
