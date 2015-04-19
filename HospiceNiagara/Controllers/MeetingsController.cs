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
            var elists = new List<EventListViewModel>();
            //Administrator index of all events
            if (User.IsInRole("Administrator"))
            {
                var contacts = db.Contacts.ToList();
                foreach (var item in events)
                {
                    string count = (invites.Where(i => i.RSVP == true).Where(i => i.EventMeetingID == item.ID)).Count().ToString();
                    var newHosDate = new EventListViewModel { ID = item.ID, Name = item.Name, Location = item.Location, StartDate = item.StartDateTime, EndDate = item.EndDateTime, AttendanceCount = count};
                    string baseType = item.GetType().BaseType.ToString();
                    string type = baseType.Remove(0, "HospiceNiagara.Models.".Length);
                    newHosDate.Type = type;
                    elists.Add(newHosDate);
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
                    var oneInvite = events.Where(e => e.ID == invite.EventMeetingID).Single();
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
            return View(elists);
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
        public ActionResult LoadDropDown(int? dID, string cIDs = "")
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
            //invitations
            //var valueProvider = fc.ToValueProvider();
            //foreach (var val in fc.Keys)
            //{
            //    string test = fc[val.ToString()];
            //}
            
            //Not dynamic, but useful way to handle pre-defined model uploads
            //pass current model instance and the name of the input file type control
            
            MeetingResource ag = new MeetingResource();
            MeetingResource att = new MeetingResource();
            MeetingResource min = new MeetingResource();
            try
            {
                ag.ResourceID = FileToEventResource(meet, "AgendaUpload", "AgendaID");
                att.ResourceID = FileToEventResource(meet, "AttendanceUpload", "AttendanceID");
                min.ResourceID = FileToEventResource(meet, "MinutesUpload", "MinutesID");
            }
            catch
            {
                return View("CreateMeeting");
            }
            var fileKeys = Request.Files.AllKeys;
            string ids = fc["ChoosenID"];
            if (ids != null)
            {
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
            }
            if (ModelState.IsValid)
            {
                //foreach (var contact in invites)
                //{
                //    db.Invitations.Add(new Invitation { Contact = contact, EventMeeting = @meet });
                //    //SendConfirmation(@meet, contact);
                //}
                db.Events.Add(@meet);
                db.SaveChanges();
                
                ag.MeetingID = meet.ID;
                db.MeetingResources.Add(ag);
                db.SaveChanges();
                
                meet.AgendaID = ag.ID;
                
                att.MeetingID = meet.ID;
                db.MeetingResources.Add(att);
                db.SaveChanges();
                
                meet.AttendanceID = att.ID;
                
                min.MeetingID = meet.ID;
                db.MeetingResources.Add(min);
                db.SaveChanges();
                
                meet.MinutesID = min.ID;
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
            var invites = db.Invitations.Include("Contact").Where(i => i.EventMeetingID == ID).ToList();
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
        public ActionResult EditMeeting(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string agenda="";
            string attendance="";
            string minutes="";
            Meeting @event = (Meeting)db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            var attendFile = db.MeetingResources.Find(@event.AttendanceID);
            if (attendFile != null)
            {
                attendance = db.MeetingResources.Find(@event.AttendanceID).Resource.FileDesc.ToString();
            }
            var agendaFile = db.MeetingResources.Find(@event.AgendaID);
            if (agendaFile != null)
            {
                agenda = db.MeetingResources.Find(@event.AgendaID).Resource.FileDesc.ToString();
            }
            var minuteFile = db.MeetingResources.Find(@event.MinutesID);
            if (minuteFile!=null)
            {
               minutes = db.MeetingResources.Find(@event.MinutesID).Resource.FileDesc.ToString();
            }
                EditMeetingViewModel eM = new EditMeetingViewModel{ID=@event.ID, Name=@event.Name,StartDateTime=@event.StartDateTime,EndDateTime=@event.EndDateTime,Location=@event.Location,
                                                               Notes = @event.Notes,Requirements=@event.Requirements, StaffLead=@event.StaffLead,Attendance=attendance,Agenda=agenda,Minutes=minutes};
            return View(eM);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMeeting([Bind(Include = "ID,Name,Date,StartDateTime,EndDateTime,Location,Requirements,Notes,StaffLead")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.BrochureID = new SelectList(db.Resources, "ID", "FileDesc", @event.BrochureID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospiceDate @event = db.Events.Find(id);
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
            HospiceDate @event = db.Events.Find(id);
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

        private int FileToEventResource(HospiceDate hd, string fileControlName, string propertyName)
        {
            string type = hd.GetType().ToString().Remove(0,"HospiceNiagara.Models.".Length);
            var fileRequest = Request.Files.Get(fileControlName);
            if (fileRequest!=null)
            {
                //file properties
                int fileLength = fileRequest.ContentLength;
                string fileName = fileRequest.FileName;
                string mimeType = fileRequest.ContentType;
                string resourceName = propertyName.Substring(0,propertyName.Length-(propertyName.IndexOf("ID")));
                
                if (!(fileLength==0||fileName==""))
                {
                    byte[] fileData = new byte[fileLength];
                    fileRequest.InputStream.Read(fileData,0,fileLength);
                    var newMRes = new MeetingResource();
                    var newRes = new Resource
                        {
                            FileDesc = resourceName+"for :"+hd.Name,
                            DateAdded = DateTime.Now,
                            ResourceCategoryID = 22,
                            FileStore = new FileStore
                            {
                                FileContent = fileData,
                                MimeType = mimeType,
                                FileName = fileName
                            }
                        };
                    db.Resources.Add(newRes);
                    db.SaveChanges();
                    return newRes.ID;
                 }
                
                return -1;
                }
            else
            {
                return -1;
            }
        }

        private void FileToResource()
        {

        }
    }
}