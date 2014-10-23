using robocall.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace robocall.Models
{
    public class StartCallsModel
    {
        public string CampaignName { get; set; }
        public string StartTokenUrl { get; set; }

        public StartCallsModel(string campaignName)
        {
            CampaignName = campaignName ?? "Default";
            var service = new PhoneListService();
            var campaign = service.GetCampaign(CampaignName);
            if (campaign == null)
                throw new Exception("Unknown Campaign");
            StartTokenUrl = campaign.StartTokenUrl;
        }

        public void Start()
        {
            var response = WebClient.Request(StartTokenUrl, null, "GET", null);
        }
    }
}