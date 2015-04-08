using HospiceNiagara.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace HospiceNiagara.Controllers
{
    public class HomeController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Images = Directory.EnumerateFiles(Server.MapPath("~/Content/Image/"))
                              .Select(fn => "~/Content/Image/" + Path.GetFileName(fn));

            ViewBag.AnnounceList = db.Announcements.OrderByDescending(anmt => anmt.Content).ToList();
            ViewBag.MeetingList = db.Events.OrderByDescending(mtg => mtg.StartDateTime).ToList();
            ViewBag.DNList = db.DeathNotices.OrderByDescending(dn => dn.Date).ToList();
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(Picture picture)
        //{
        //    if (picture.File.ContentLength > 0)
        //    {
        //        var filename = Path.GetFileName(picture.File.FileName);
        //        var path = Path.Combine(Server.MapPath("~/Content/Image"), filename);
        //        picture.File.SaveAs(path);
        //    }
        //    else
        //    {
        //        return JavaScript("<script>alert(\"some message\")</script>");
        //    }
        //    return RedirectToAction("Index");

        //}
    }
}