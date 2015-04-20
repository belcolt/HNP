using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace HospiceNiagara.Models
{
    public class Picture : Controller
    {
        public HttpPostedFileBase File { get; set; }

    }
    
}