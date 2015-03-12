using HospiceNiagara.DAL;
using HospiceNiagara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var invites = db.Invitations.ToList();
            var elists = new List<EventListViewModel>();
            foreach (var item in events)
            {
                string count = (invites.Where(i=>i.RSVP==true).Where(i=>i.EventID==item.ID)).Count().ToString();
                elists.Add(new EventListViewModel{ID=item.ID,Name = item.Name, Location = item.Location, StartDate = item.StartDateTime, EndDate=item.EndDateTime,AttendanceCount=count});
            }
            ViewBag.EventsList = elists;
            return View();
        }

        public ActionResult CreateMeeting()
        {
            return PartialView("_CreateMeeting");
        }
    }
}