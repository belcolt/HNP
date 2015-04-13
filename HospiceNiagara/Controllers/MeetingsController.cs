using HospiceNiagara.DAL;
using HospiceNiagara.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        enum Domains { Volunteer = 1, Staff = 2, Board = 3, Organizational = 4 };
        private HospiceNiagaraContext db = new HospiceNiagaraContext();
        // GET: Meetings
        public ActionResult Index()
        {
            var invites = db.Invitations.ToList();
            var events = db.Events.ToList();
            //Administrator index of all events
            if (User.IsInRole("Administrator"))
            {
                var contacts = db.Contacts.ToList();
                var elists = new List<EventListViewModel>();
                foreach (var item in events)
                {
                    string count = (invites.Where(i => i.RSVP == true).Where(i => i.EventID == item.ID)).Count().ToString();
                    elists.Add(new EventListViewModel { ID = item.ID, Name = item.Name, Location = item.Location, StartDate = item.StartDateTime, EndDate = item.EndDateTime, AttendanceCount = count });
                }
                ViewBag.EventsList = elists;
            }

            //Invitation index for any user
            string id = User.Identity.GetUserId();
            if (id != null)
            {
                int contactID = db.Users.Find(id).ContactID;
                var singleInvites = new List<UserInviteViewModel>();
                var eventIDs = invites.Where(i => i.ContactID == contactID).ToList();
                foreach (var invite in eventIDs)
                {
                    var oneInvite = events.Where(e => e.ID == invite.EventID).Single();
                    var userInvite = new UserInviteViewModel { EventID = invite.ID, InviteID = invite.ID, Name = oneInvite.Name, StartDate = oneInvite.StartDateTime, EndDate = oneInvite.EndDateTime };
                    if (invite.RSVP == true)
                    {
                        userInvite.Attend = "Attending";
                    }
                    else if (invite.RSVP == false & invite.HasResponded == false)
                    {
                        userInvite.Attend = "RSVP";
                    }
                    else if (invite.RSVP == false & invite.HasResponded == true)
                    {
                        userInvite.Attend = "Not Attending";
                    }
                    singleInvites.Add(userInvite);
                }
                ViewBag.UserInvitesList = singleInvites;
            }
            return View();
        }
        [HttpPost]
        public void AttendResponse(int inviteID, string attend)
        {
            var invitation = db.Invitations.Where(i => i.ID == inviteID).Single();
            if (attend == "Attending")
            {
                invitation.RSVP = true;

            }
            else if (attend == "Not Attending")
            {
                invitation.RSVP = false;
            }
            if (invitation.HasResponded == false)
            {
                invitation.HasResponded = true;
            }
            db.SaveChanges();
        }
        public ActionResult LoadDropDown(int? dID, string cIDs ="")
        {
            List<int> choosen = new List<int>();
            List<int> choosenDomains = new List<int>();
            string[] contactID = cIDs.Split(new string[] { "," }, StringSplitOptions.None);
            if (cIDs != "All")
            {
                foreach (string s in contactID)
                {
                    int num;
                    if (int.TryParse(s, out num))
                    {
                        choosen.Add(num);
                    }
                    else
                    {
                        if (s != "")
                        {
                            Domains d = (Domains)Enum.Parse(typeof(Domains), s);
                            choosenDomains.Add((int)d);
                        }
                    }
                }

                if (choosen.Count == 0)
                {
                    choosen.Add(0);
                }

                if (dID > 0)
                {

                    if (choosenDomains.Contains((int)dID))
                    {
                        var temp = db.Contacts.Where(c => c.TeamDomainID == dID & !choosen.Contains(c.ID));
                        ViewBag.ContactDD = new MultiSelectList(temp.Where(c => !choosenDomains.Contains(c.TeamDomainID)).ToList(), "ID", "FullName");
                    }
                    else
                    {
                        ViewBag.ContactDD = new MultiSelectList(db.Contacts.Where(c => c.TeamDomainID == dID & !choosen.Contains(c.ID)).ToList(), "ID", "FullName");
                    }

                    return PartialView("ContactDropDown");
                }
                else
                {
                    ViewBag.ContactDD = new MultiSelectList(db.Contacts.Where(c => !choosenDomains.Contains(c.TeamDomainID) & !choosen.Contains(c.ID)).ToList(), "ID", "FullName");
                    return PartialView("ContactDropDown");
                }
            }
            else
            {
                ViewBag.ContactDD = new MultiSelectList(Enumerable.Empty<SelectListItem>());
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
            ViewBag.ChoosenID = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMeeting([Bind(Include = "ID,Name,Date,StartDateTime,EndDateTime,Location,Requirements,Notes,StaffLead")] Meeting @meet, FormCollection fc)
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
            //var valueProvider = fc.ToValueProvider();
            //foreach (var val in fc.Keys)
            //{
            //    string test = fc[val.ToString()];
            //}
            
            string ids = fc["ChoosenID"];
            List<int> choosen = new List<int>();
            List<int> choosenDomains = new List<int>();
            string[] contactID = ids.Split(new string[] { "," }, StringSplitOptions.None);
            foreach (string s in contactID)
            {
                int num = 0;
                if (int.TryParse(s, out num))
                {
                    choosen.Add(num);
                }
                else
                {
                    if (s != "")
                    {
                        if (s == "All")
                        {
                            choosenDomains.Add(0);
                        }
                        else
                        {
                            Domains d = (Domains)Enum.Parse(typeof(Domains), s);
                            choosenDomains.Add((int)d);
                        }
                        
                    }
                }
                
            }
            List<Contact> invites;

            //if (choosen.Count > 0)
            //{
            if (choosenDomains.Contains(0))
            {
                invites = db.Contacts.ToList();
            }
            else
            {
               
                invites = db.Contacts.Where(i => choosenDomains.Contains(i.TeamDomainID) | choosen.Contains(i.ID)).ToList();
            }
                
            //}
            //if (ContactsAddID != null)
            //{
            //    invites = db.Contacts.Where(c => c.TeamDomainID == ContactsAddID).Select(c => c).ToList();
            //}
            //else
            //{
            //    invites = db.Contacts.ToList();
            //}
            
            if (ModelState.IsValid)
            {
                foreach (var contact in invites)
                {
                    db.Invitations.Add(new Invitation { Contact = contact, Event = @meet });
                    //SendConfirmation(@meet, contact);
                }
                db.Events.Add(@meet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContactsAddID = new SelectList(db.TeamDomains, "ID", "Description");
            ViewBag.ChoosenID = new SelectList(Enumerable.Empty<SelectListItem>());
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
                    singleEventList.Attending.Add(item.Contact.FirstName + " " + item.Contact.LastName);
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

        private void SendConfirmation(Meeting meet, Contact contact)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("smtptestdestination@gmail.com");
            mail.To.Add(contact.Email);
            mail.Subject = @meet.Name;
            mail.Body = "Dear " + contact.FirstName+": \n\n"+ "Your attendence has been requested for the "+@meet.Name+" located at "+ @meet.Location+". The meeting is from " + @meet.StartDateTime.ToString()+" untill "+@meet.EndDateTime.ToString() + ".\n Meeting Notes: " + @meet.Notes +"\n Meeting Requirements: "+@meet.Requirements;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("smtptestdestination@gmail.com", "eightchar");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            
        }
    }
}