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
using System.Diagnostics;
using OfficeOpenXml;
using System.ComponentModel;

namespace HospiceNiagara.Controllers
{
    public class ContactsController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Contacts
        public ActionResult Index(string SearchString, int? TeamDomainID, int? JobDescriptionID)
        {

            var directors = db.BoardMembers.AsEnumerable();
            //if (!String.IsNullOrEmpty(SearchString))
            //{
            //    directors = directors.Where(d => d.LastName.ToUpper().Contains(SearchString.ToUpper())
            //                           || d.FirstName.ToUpper().Contains(SearchString.ToUpper()));
            //}
            var contacts = db.Contacts.Include(c => c.TeamDomain).Include(c => c.JobDescription);
            ViewBag.Directors = directors;

            
            if (TeamDomainID.HasValue)
                contacts = contacts.Where(p => p.TeamDomainID == TeamDomainID);
            //if (JobDescriptionID.HasValue)
            //    contacts = contacts.Where(p => p.JobDescriptionID == JobDescriptionID);
            ViewBag.Regular = contacts;



            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description");
            return View();
        }

        

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description");
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "ID", "JobName");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Position,Phone,Email,TeamDomainID,JobDescriptionID")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description", contact.TeamDomainID);
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "ID", "JobName", contact.JobDescriptionID);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description", contact.TeamDomainID);
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "ID", "JobName", contact.JobDescriptionID);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Position,Phone,Email,IsBoardDirector,TeamDomainID,JobDescriptionID")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description", contact.TeamDomainID);
            ViewBag.JobDescriptionID = new SelectList(db.JobDescriptions, "ID", "JobName", contact.JobDescriptionID);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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

        //public ActionResult ToXLS()
        //{
            //byte[] file = new byte[] { }; ;
            //ContactToXLSX(file);
            
            //return RedirectToAction("Index", "Contacts", new { area = "Index" });
        //}

        public FileContentResult Download()
        {
            byte[] file = ContactToXLSX();
            
            return File(file,"application/vnd.ms-excel","Contacts.xlsx");
        }

        public byte[] ContactToXLSX()
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Contact List");
                Contact contact = new Contact();
                int cols = 1;
                //foreach (var prop in contact.GetType().GetProperties())
                //{
                List<string> headings = new List<string>{"FirstName", "LastName", "Phone", "Email", "JobDescription"};
                 PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(contact);
                 foreach (PropertyDescriptor prop in properties)
                 {

                     if (headings.Contains(prop.Name))
                     {
                         if (prop.DisplayName == null)
                         {
                             worksheet.Cells[1, cols].Value = prop.Name;
                         }
                         else
                         {
                             worksheet.Cells[1, cols].Value = prop.DisplayName;
                         }
                         cols++;
                     }
                 }
                    //if(prop.PropertyType == typeof(string))
                    //{
                    //    worksheet.Cells[1, cols].Value = SplitHeader(prop.Name);
                    //    cols++;
                    //}
                    
                 //}
                worksheet.Cells[1, 1, 1, cols].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, cols].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //worksheet.Cells[1, 1, 1, cols].AutoFitColumns();
                var contacts = db.Contacts.Include(c => c.JobDescription).ToList();
                headings = new List<string> { "FirstName", "LastName", "Phone", "Email" };
                int rows = 2;
                foreach (var c in contacts)
                {
                    
                    int i = 1;
                    properties = TypeDescriptor.GetProperties(c);
                    
                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (headings.Contains(prop.Name))
                        {
                            
                            worksheet.Cells[rows, i].Value = prop.GetValue(c);
                            i++;
                        }
                        else if (prop.Name == "JobDescriptionID")
                        {
                            string con = contacts.Where(l => l.JobDescriptionID == c.JobDescriptionID).Select(l => l.JobDescription.JobName).First();
                            worksheet.Cells[rows, i].Value = con;
                            i++;
                        }
                        
                        
                    }
                    rows++;
                }
                //FileInfo f = new FileInfo("C:\\Users\\bjjkdci\\Desktop\\TestDestination\\contacts.xlsx");
                worksheet.Cells[1, 1, rows, cols].AutoFitColumns();
               byte[] file = pck.GetAsByteArray();
               return file;
                //pck.SaveAs(f);
                

            }
        }


        public void ContactToCSV()
        {

           
            Contact contact = new Contact();
           
            StreamWriter writer = new StreamWriter("C:\\Users\\bjjkdci\\Desktop\\TestDestination\\contacts.csv");
            
            List<string> headers = new List<string>();
            foreach (var prop in contact.GetType().GetProperties())
            {
                if(prop.PropertyType == typeof(string))
                {
                    string test = SplitHeader(prop.Name);
                    headers.Add(test);
                }
                
            }
            string[] h = headers.ToArray();
            writer.WriteLine(String.Join(",", h));
            var contacts = db.Contacts;
            headers.Clear();
            
            foreach (var val in contacts)
            {
                headers.Clear();
                foreach (var prop in val.GetType().GetProperties())
                {
                    if(prop.PropertyType == typeof(string))
                    {
                        headers.Add(prop.GetValue(val, null).ToString());
                    }
                }
                Array.Clear(h, 0, h.Length);
                h = headers.ToArray();
                writer.WriteLine(String.Join(",", h));
            }
            writer.Dispose();
        }

        public string SplitHeader(string header)
        {
            List<string> split = header.Select(c => c.ToString()).ToList();


            split = split.Select((t, i) => i != 0 & t.ToUpper() == t ? " " + t : t).ToList();

            return String.Join("", split);
        }
    }
}
