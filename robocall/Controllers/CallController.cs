using robocall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace robocall.Controllers
{
    public class CallController : Controller
    {
        public ActionResult StartCalls(string id)
        {
            var model = new StartCallsModel(id);
            model.Start();
            return null;
        }

        public ActionResult OutCall(string id)
        {
            var model = new OutCallModel(id);
            if (model.IsDone) return null;
            HttpContext.Response.ContentType = "application/x-callxml";
            return View(model);
        }

        public ActionResult LogEvent()
        {
            var model = new LogEventModel(Request.QueryString);
            HttpContext.Response.ContentType = "application/x-callxml";
            return View();
        }

        [HttpPost]
        public ActionResult LogEvent(FormCollection formCollection)
        {
            var model = new LogEventModel(formCollection);
            HttpContext.Response.ContentType = "application/x-callxml";
            return View();
        }
    }
}
