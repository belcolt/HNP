using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class AnnouncementsController : Controller
    {
        // GET: Announcements
        public ActionResult Index()
        {
            return View();
        }
    }
}