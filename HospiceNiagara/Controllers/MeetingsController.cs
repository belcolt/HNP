using HospiceNiagara.DAL;
using HospiceNiagara.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class MeetingsController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();
        // GET: Meetings
        public ActionResult Index()
        {
            var events = db.Events.ToList();
            var contacts = db.Contacts.ToList();

            var invites = db.Invitations.ToList();
            var elists = new List<EventListViewModel>();
            foreach (var item in events)
            {
                string count = (invites.Where(i => i.RSVP == true).Where(i => i.EventID == item.ID)).Count().ToString();
                elists.Add(new EventListViewModel { ID = item.ID, Name = item.Name, Location = item.Location, StartDate = item.StartDateTime, EndDate = item.EndDateTime, AttendanceCount = count });
            }
            ViewBag.EventsList = elists;
            return View();
        }
       public ActionResult LoadDropDown(int? id)
       {
           if(id > 0)
           {
               ViewBag.ContactDD = new MultiSelectList(db.Contacts.Where(c => c.TeamDomainID == id).ToList(), "ID", "LastName");
               return PartialView("ContactDropDown");
           }
           else
           {
               ViewBag.ContactDD = new MultiSelectList(db.Contacts.ToList(), "ID", "LastName");
               return PartialView("ContactDropDown");
           }
          
       }
        public ActionResult CreateEvent()
        {
            return View();
        }
        public ActionResult CreateMeeting()
        {
            ViewBag.ContactsAddID = new SelectList(db.TeamDomains, "ID", "Description");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMeeting([Bind(Include = "ID,Name,Date,StartDateTime,EndDateTime,Location,Requirements,Notes,StaffLead")] Meeting @meet, int? ContactsAddID)
        {
            //string mimeType = Request.Files[0].ContentType;
            //string fileName = Path.GetFileName(Request.Files[0].FileName);
            //int fileLength = Request.Files[0].ContentLength;
            //var type = db.ResourceCategories.Where(rc => rc.Name == "Test Agenda").Single();
            //if (!(fileName == "" || fileLength == 0))
            //{
            //    Stream fileStream = Request.Files[0].InputStream;
            //    byte[] fileData = new byte[fileLength];
            //    fileStream.Read(fileData, 0, fileLength);

            //    //resource type already existent
            //    var newRes = new Resource
            //    {
            //        FileDesc = "Agenda for"+@meet.Name,
            //        ResourceCategory = type,
            //        FileStore = new FileStore
            //        {
            //            FileContent = fileData,
            //            MimeType = mimeType,
            //            FileName = fileName
            //        }
            //    };
            //    @meet.Agenda = newRes;
            //}
            //invitations
            List<Contact> invites;
                if (ContactsAddID != null)
            {
                invites = db.Contacts.Where(c => c.TeamDomainID == ContactsAddID).Select(c=>c).ToList();
            }
            else
            {
                invites = db.Contacts.ToList();
            }
            foreach(var contact in invites)
            {
                db.Invitations.Add(new Invitation { Contact = contact, Event = @meet });
            }
            if (ModelState.IsValid)
            {
                db.Events.Add(@meet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrochureID = new SelectList(db.Resources, "ID", "FileDesc", @meet.BrochureID);
            return View(@meet);
        }

        //public ActionResult CreateMeeting()
        //{
        //    return PartialView("_CreateMeeting");
        //}

        public ActionResult Invites(int? ID)
        {
            var singleEventList = new InvitationsSingleViewModel();
            var invites = db.Invitations.Include("Contact").Where(i => i.EventID == ID).ToList();
            singleEventList.EventName = db.Events.Where(e => e.ID == ID).Select(e => e.Name).SingleOrDefault();
            foreach (var item in invites)
            {
                if (item.RSVP == true)
                {
                    singleEventList.Attending.Add(item.Contact.FirstName+" "+item.Contact.LastName);
                }
                else if (item.RSVP == false && item.HasResponded == true)
                {
                    singleEventList.NotAttending.Add(item.Contact.FirstName + " " + item.Contact.LastName);
                }
                else
                {
                    singleEventList.NotResponded.Add(item.Contact.FirstName + " " + item.Contact.LastName);
                }
            }
            return View("Invites", singleEventList);
        }
        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrochureID = new SelectList(db.Resources, "ID", "FileDesc", @event.BrochureID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Date,StartDateTime,EndDateTime,Location,Requirements,Notes,StaffLead")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrochureID = new SelectList(db.Resources, "ID", "FileDesc", @event.BrochureID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}