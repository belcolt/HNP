using HospiceNiagara.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using HospiceNiagara.Models;
namespace HospiceNiagara.Controllers
{
    public class HomeController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        // GET: Home
        [SessionTracking.Logging]
        public ActionResult Index()
        {
            int cID = db.Users.Where(u => u.UserName == User.Identity.Name).Select(u => u.ContactID).SingleOrDefault();
            Contact c = db.Contacts.Find(cID);
            if (c.DateHired.Date.Month == DateTime.Now.Date.Month & c.DateHired.Date.Day == DateTime.Now.Date.Day)
            {
                if (c.YearsOfService > 1)
                {
                    ViewBag.Anniversary = "Congratulations on your " + c.YearsOfService.ToString() + " years of service!  Our success would not be possible without dedicated people such as yourself. Thank you.";                   
                }
                else if (c.YearsOfService == 1)
                {
                    ViewBag.Anniversary = "Congratulations on your " + c.YearsOfService.ToString() + "st year of service!  Our success would not be possible without dedicated people such as yourself. Thank you.";
                }
                else
                {
                    ViewBag.Anniversary = "Congratulations on joining Hospice Niagara!  We all look forward to working with you!";
                }                
            }
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