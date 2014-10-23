using robocall.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace robocall.Models
{
    public class OutCallModel
    {
        public int CallCount { get; set; }
        public int MaxCallCount { get; set; }
        public string PhoneNumber { get; set; }
        public string CallerID { get; set; }
        public string AudioFileUrl { get; set; }
        public bool IsLastAttempt { get { return CallCount >= MaxCallCount; } }
        public bool AnswerOnMedia { get; set; }
        public string CampaignName { get; set; }

        public bool IsDone { get; set; }

        public OutCallModel(string campaignName)
        {
            CampaignName = campaignName ?? "Default";
            var service = new PhoneListService();
            var campaign = service.GetCampaign(CampaignName);
            if (campaign == null)
                throw new Exception("Unknown Campaign");
            var nextCall = service.GetNextPhoneListItem(CampaignName);
            if (nextCall == null)
            {
                IsDone = true;
                return;
            }
            CallCount = nextCall.CallCount;
            MaxCallCount = campaign.MaxCallCount;
            PhoneNumber = nextCall.PhoneNumber;
            AudioFileUrl = campaign.AudioFileUrl;
            CallerID = campaign.CallerID;
            AnswerOnMedia = campaign.AnswerOnMedia;
        }

    }
}