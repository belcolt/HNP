﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class MeetingsController : Controller
    {
        // GET: Meetings
        public ActionResult Index()
        {
            return View();
        }
    }
}