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
            //Change to pull viewbag from resources
            ViewBag.Images = Directory.EnumerateFiles(Server.MapPath("~/Content/Image/"))
                              .Select(fn => "~/Content/Image/" + Path.GetFileName(fn));

            ViewBag.AnnounceList = db.Announcements.OrderByDescending(anmt => anmt.PostDate).ToList();
            ViewBag.MeetingList = db.Events.OrderByDescending(mtg => mtg.StartDateTime).ToList();
            ViewBag.DNList = db.DeathNotices.OrderByDescending(dn => dn.Date).ToList();

            ViewBag.DNBadge = db.DeathNotices.Where(dn => dn.IsNew == true).Count();
            ViewBag.AnmtBadge = db.Announcements.Where(anmt => anmt.IsNew == true).Count();

            return View();
        }

        //[HttpPost]
        //public ActionResult Upload(Picture picture)
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