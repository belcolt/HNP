using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.Controllers;
using HospiceNiagara.DAL;
namespace HospiceNiagara.SessionTracking
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //How to cast to specific controller:
            //var controller = ((ContactsController)filterContext.Controller);

            //filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
            //    filterContext.ActionDescriptor.ActionName);
            
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (filterContext.Exception != null)
            //    filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");
            if (ActiveSessions.Sessions.ContainsKey(filterContext.HttpContext.Session.SessionID))
            {
                ActiveSessions.Sessions[filterContext.HttpContext.Session.SessionID].CurrentPage = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName +"/"+filterContext.ActionDescriptor.ActionName;
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public class TrackDownloadAttribute : ActionFilterAttribute
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object id;
            filterContext.ActionParameters.TryGetValue("id", out id);
            var res = db.Resources.Find(Convert.ToInt32(id));
            res.DownloadCount++;
            db.SaveChanges();
            base.OnActionExecuting(filterContext);
        }
    }
}