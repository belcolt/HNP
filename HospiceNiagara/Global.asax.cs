using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HospiceNiagara.SessionTracking;

namespace HospiceNiagara
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
          
            if (this.User.Identity.Name != "")
            {
                TrackInfo tI = new TrackInfo();
                tI.User = this.User.Identity.Name;
                ActiveSessions.Sessions.Add(Session.SessionID, tI);
            }
            else
            {
                TrackInfo tI = new TrackInfo();
                tI.User = "Anonymous";
                ActiveSessions.Sessions.Add(Session.SessionID, tI);
            }
            
        }
        protected void Session_End(object sender, EventArgs e)
        {
            //To do: add check for Session.IsNewSession
            ActiveSessions.Sessions.Remove(Session.SessionID);
        }
    }
}
