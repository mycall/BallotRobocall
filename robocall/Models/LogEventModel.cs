using robocall.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace robocall.Models
{
    public class LogEventModel
    {
        public FormCollection FormCollection { get; set; }

        public LogEventModel(NameValueCollection QueryString)
        {
            FormCollection = new FormCollection(QueryString);
        }

        public LogEventModel(FormCollection FormCollection)
        {
            this.FormCollection = FormCollection;
        }

        public void SaveLog()
        {
            var service = new PhoneListService();
            var campaign = service.GetCampaign(CampaignName);
            if (campaign == null)
                throw new Exception("Unknown Campaign");

            foreach (string item in FormCollection)
            {
                service.SetCallResult(campaign.Name, PhoneNumber, item + ": " + FormCollection[item]);
            }

        }

        private string CampaignName { get { return FormCollection["CampaignName"]; } }

        private string PhoneNumber { get { return FormCollection["RealNumber"]; } }

    }
}