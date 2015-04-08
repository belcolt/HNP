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
using System.Globalization;
using HospiceNiagara.HospiceUserExtensions;
namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();
        enum getmonths { January, February, March, April, May, June, July, August, September, October, November, December };
        // GET: Schedules
        public ActionResult Index()
        {

            
            //string m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
           
            var schedules = db.Schedules.Include(s => s.Resource);
            
            List<Schedule> res = schedules.Where(s => s.Category == "Residential Schedule" & s.IsActiveSchedule == true).ToList();
            List<Schedule> pet = schedules.Where(s => s.Category == "Pet Therapy Schedule" & s.IsActiveSchedule == true).ToList();
            List<Schedule> day = schedules.Where(s => s.Category == "Welland Day Hospice Schedule" & s.IsActiveSchedule == true).ToList();
            List<Schedule> events = schedules.Where(s => s.Category == "Event Schedule" & s.IsActiveSchedule == true).ToList();
            
            ViewBag.ResidentialDD = ActiveSchedDropDown(res);
            ViewBag.PetDD = ActiveSchedDropDown(pet);
            ViewBag.DayDD = ActiveSchedDropDown(day);
            ViewBag.EventDD = ActiveSchedDropDown(events);
            return View(schedules.ToList());
        }
        [Authorize(Roles="CantGetHere")]
        public ActionResult GoodIndex()
        {
            var schedules = db.Schedules.Include(s => s.Resource);
            var res = schedules.Where(s => s.Category == "Residential Schedule" & s.IsActiveSchedule == true);
            var pet = schedules.Where(s => s.Category == "Pet Therapy Schedule" & s.IsActiveSchedule == true);
            var day = schedules.Where(s => s.Category == "Welland Day Hospice Schedule" & s.IsActiveSchedule == true);
            var events = schedules.Where(s => s.Category == "Event Schedule" & s.IsActiveSchedule == true);

            ViewBag.ResidentialDD = new SelectList(res, "ResourceID", "Month", "Select a Month");

            return View(schedules.ToList());
        }
       
        public ActionResult ShowSched(int id)
        {

            var fileTitle = db.Resources.Where(u => u.FileStoreID == id).SingleOrDefault();
            ViewBag.Title = fileTitle.FileDesc;
            var theFile = db.FileStores.Where(f => f.ID == id).SingleOrDefault();
            var length = theFile.FileContent.Length;

            string string64 = Convert.ToBase64String(theFile.FileContent);
            ViewBag.Mime = theFile.MimeType;
            ViewBag.Name = theFile.FileName;
            ViewBag.FileBits = string64;
            return PartialView("ShowSched");
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
         [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (TempData["message"] == null)
            {
                var schedSC = db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == 8);
                ViewBag.ResourceSubcategory = new SelectList(schedSC, "Name", "Name");
                MonthsDropdown();
                YearDropDown();
                ViewBag.Message = "test";
                return View();
            }
            //29 is Schedules category, might need to make this morning dynamic in future
            else
            {
                var schedSC = db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == 8);
                ViewBag.ResourceSubcategory = new SelectList(schedSC, "Name", "Name");
                ViewBag.Message = TempData["message"];
                MonthsDropdown();
                YearDropDown();
                return View();
            }
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Year,IsActiveSchedule")] Schedule schedule, string ResourceSubcategory, string SchedMonth, string SchedYear)
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
                int y = Convert.ToInt32(SchedYear);
                var check = db.Schedules.Where(s => s.Month == SchedMonth & s.Year == y & s.Category == ResourceSubcategory & s.IsActiveSchedule == true).ToList();
                if (check.Count > 0)
                {
                    check[0].IsActiveSchedule = false;
                    db.Entry(check[0]).State = EntityState.Modified;
                }
                string fileName = Path.GetFileName(Request.Files[0].FileName);
                int fileLength = Request.Files[0].ContentLength;
                schedule.IsActiveSchedule = true;
                schedule.Category = ResourceSubcategory;
                schedule.Month = SchedMonth;
                schedule.Year = y;
                schedule.DataAdded = DateTime.Now;
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
        [Authorize(Roles = "Admin")]
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
            if (schedule.IsActiveSchedule)
            {
                
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Category,Year,ResourceID,IsActiveSchedule")] Schedule schedule, string SchedMonth, string SchedYear)
        {
            if (SchedYear.Length == 0)
            {
                ModelState.AddModelError("Year", "Please select a year from the dropdown list.");
            }
            if (SchedMonth.Length == 0)
            {
                ModelState.AddModelError("Month", "Please select a month from the dropdown list.");
            }
            Schedule target = default(Schedule);
            
            if (ModelState.IsValid)
            {
                schedule.Month = SchedMonth;
                schedule.Year = Convert.ToInt32(SchedYear);
                schedule.DataAdded = DateTime.Now;
                if (schedule.IsActiveSchedule)
                {
                    Schedule compare = db.Schedules.AsNoTracking().Where(s => s.ID == schedule.ID).FirstOrDefault();
                    if (compare != default(Schedule) & compare.IsActiveSchedule == false)
                    {
                        target = db.Schedules.Select(s => s).Where(s => s.Month == schedule.Month & s.Year == schedule.Year & s.Category == schedule.Category & s.IsActiveSchedule == true).FirstOrDefault();
                        if (target != default(Schedule))
                        {
                            
                            target.IsActiveSchedule = false;
                            db.Entry(target).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    Schedule compare = db.Schedules.AsNoTracking().Where(s => s.ID == schedule.ID).FirstOrDefault();
                    if (compare.IsActiveSchedule)
                    {
                        var getNext = db.Schedules.Select(s => s).Where(s => s.Month == schedule.Month & s.Year == schedule.Year & s.Category == schedule.Category & s.IsActiveSchedule == false).OrderByDescending(s => s.DataAdded).FirstOrDefault();
                        if (getNext != default(Schedule))
                        {
                            getNext.IsActiveSchedule = true;
                            db.Entry(getNext).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                
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
        [Authorize(Roles = "Admin")]
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
            if (schedule.IsActiveSchedule == true)
            {
                var check = db.Schedules.Where(s => s.Month == schedule.Month & s.Year == schedule.Year & s.Category == schedule.Category & s.IsActiveSchedule == false & s.ID != schedule.ID).ToList();
                if (check.Count > 0)
                {
                    ViewBag.Warning = "Warning: This is the active " + schedule.Category + " for " + schedule.Month + " " + schedule.Year + ". Continue to make the next available schedule the active one.";
                }
            }
           
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            var check = db.Schedules.Select(s => s).Where(s => s.Month == schedule.Month & s.Year == schedule.Year & s.Category == schedule.Category & s.IsActiveSchedule == false & s.ID != schedule.ID).OrderByDescending(s => s.DataAdded).FirstOrDefault();
            if (check != default(Schedule))
            {

               
                check.IsActiveSchedule = true;
                db.Entry(check).State = EntityState.Modified;
            }
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
        public ActionResult CheckSchedule(string month, string year, string category)
        {
            int y = Convert.ToInt32(year);
            var check = db.Schedules.Where(s => s.Month == month & s.Year == y & s.Category == category & s.IsActiveSchedule == true).ToList();
            if (check.Count > 0)
            {
                ViewBag.Category = category;
                ViewBag.Month = month;
                ViewBag.Year = year;
                return PartialView("ScheduleCheck");
            }
            else
            {
                return null;
            }
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CheckEditSchedule(string id, string month, string year, string category, string start)
        {
            int y = Convert.ToInt32(year);
            
            if (start == "true")
            {
                var check = db.Schedules.Select(s => s).Where(s => s.Month == month & s.Year == y & s.Category == category & s.IsActiveSchedule == false).OrderByDescending(s => s.DataAdded).FirstOrDefault();
                if (check != default(Schedule))
                {
                    ViewBag.Warning = "This is currently the active schedule. Changing so will make the next " + check.Category + " for " + check.Month + " " + check.Year + " uplaoded on " + check.DataAdded + " active.";
                    return PartialView("ScheduleEditCheck");
                }
            
            }
            else
            {
                var check = db.Schedules.Select(s => s).Where(s => s.Month == month & s.Year == y & s.Category == category & s.IsActiveSchedule == true).FirstOrDefault();
                if (check != default(Schedule))
                {
                    ViewBag.Warning = "Making this the active schedule will override the " + check.Category + " for " + check.Month + " " + check.Year + " uploaded on " + check.DataAdded;
                    return PartialView("ScheduleEditCheck");
                }
            }


            return null;
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

        private SelectList ActiveSchedDropDown(List<Schedule> scheds)
        {
            int startmonth = DateTime.Now.Month - 2;
            int year = DateTime.Now.Year;
            List<Schedule> add = new List<Schedule>();
            for (int x = 0; x < 5; x++)
            {
                if (startmonth == -1)
                {
                    int y = year - 1;
                    getmonths g = (getmonths)11;
                    string m = g.ToString();
                    try
                    {
                        add.Add(scheds.Where(s => s.Month == m & s.Year == y).Single());
                    }
                    catch { }
                }
                else if (startmonth == 0)
                {
                    int y = year - 1;
                    getmonths g = (getmonths)12;
                    string m = g.ToString();
                    try
                    {
                        add.Add(scheds.Where(s => s.Month == m & s.Year == y).Single());
                    }
                    catch { }
                    
                }
                else
                {
                    int y = DateTime.Now.Year;
                    getmonths g = (getmonths)startmonth;
                    string m = g.ToString();
                    try
                    {
                        add.Add(scheds.Where(s => s.Month == m & s.Year == y).Single());
                        
                    }
                    catch { }
                }
                startmonth++;
            }
            if (add.Count != 0)
            {
                SelectList sel = new SelectList(add, "ResourceID", "Month", "Select a Month");
                return sel;
            }
            else
            {
                SelectList sel = new SelectList(add, "ResourceID", "Month", "None Available");
                return sel;
            }
            
        }
    }
}
