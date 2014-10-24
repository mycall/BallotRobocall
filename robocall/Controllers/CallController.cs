using robocall.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            HttpContext.Response.ContentType = "application/x-callxml";
            return View(model);
        }

        public ActionResult LogEvent()
        {
            var model = new LogEventModel(Request.QueryString);
            model.SaveLog();
            TriggerNextCall(model.CampaignName);
            HttpContext.Response.ContentType = "application/x-callxml";
            return View(model);
        }

        [HttpPost]
        public ActionResult LogEvent(FormCollection formCollection)
        {
            var model = new LogEventModel(formCollection);
            model.SaveLog();
            TriggerNextCall(model.CampaignName);
            HttpContext.Response.ContentType = "application/x-callxml";
            return View(model);
        }

        private void TriggerNextCall(string campaignName)
        {
            if (ConfigurationManager.AppSettings["AutoTriggerNextCall"].ToLower() == "true")
                StartCalls(campaignName);
        }
    }
}
