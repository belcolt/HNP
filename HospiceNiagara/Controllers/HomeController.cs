using HospiceNiagara.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class HomeController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.AnnounceList = db.Announcements.OrderByDescending(anmt => anmt.Content).ToList();
            ViewBag.MeetingList = db.Events.OrderByDescending(mtg => mtg.StartDateTime).ToList();
            ViewBag.DNList = db.DeathNotices.OrderByDescending(dn => dn.Date).ToList();
            return View();
        }
    }
}