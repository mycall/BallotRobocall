using robocall.Services.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace robocall.Services
{
    public class PhoneListService : BaseService
    {
        public int GetCampaignId(string name)
        {
            var result = connection.Query<int>("select CampaignId from Campaigns where Name=@Name", new { Name = name });
            return result.First();
        }

        public CampaignItem GetCampaign(string campaignName)
        {
            var result = connection.Query<CampaignItem>("select * from Campaigns where Name=@Name", new { Name = campaignName });
            return result.First();
        }

        public PhoneListItem GetPhoneListItem(string campaignName, string phoneNumber)
        {
            var campaignId = GetCampaignId(campaignName);
            var result = connection.Query<PhoneListItem>("select * from PhoneList where CampaignId=@CampaignId and PhoneNumber=@PhoneNumber", new { CampaignId = campaignId, PhoneNumber = phoneNumber });
            return result.FirstOrDefault();
        }

        public List<PhoneListItem> GetPhoneList(string campaignName)
        {
            var campaignId = GetCampaignId(campaignName);
            var result = connection.Query<PhoneListItem>("select * from PhoneList where CampaignId=@CampaignId", new { CampaignId = campaignId });
            return result.ToList();
        }

        public PhoneListItem GetNextPhoneListItem(string campaignName)
        {
            var campaignId = GetCampaignId(campaignName);
            var result = connection.Query<PhoneListItem>("select top 1 * from PhoneList where CampaignId=@CampaignId and (Status is null or Status != 'done') order by ISNULL(CallCount, 0) asc", new { CampaignId = campaignId });
            var item = result.FirstOrDefault();
            if (item == null) return null;
            var updateResult = connection.Execute("update PhoneList set CallCount=ISNULL(CallCount, 0) + 1, Status = 'calling' where PhoneListId=@PhoneListId", new { PhoneListId = item.PhoneListId });
            if (updateResult != 1) return null;
            return item;
        }

        public PhoneListItem SetCallResult(string campaignName, string phoneNumber, string status)
        {
            var phoneListItem = GetPhoneListItem(campaignName, phoneNumber);
            if (phoneListItem == null) return null;
            var updateResult = connection.Execute("update PhoneList set Status = @Status, CalledAt = @CalledAt where PhoneListId=@PhoneListId",
                new { Status = status, PhoneListId = phoneListItem.PhoneListId, CalledAt = DateTime.Now });
            if (updateResult != 1) return null;
            var insertResult = connection.Execute("insert into CallLog (PhoneListId, Result, AddedAt) values (@PhoneListId, @Status, @AddedAt)",
                new { Status = status, PhoneListId = phoneListItem.PhoneListId, AddedAt = DateTime.Now });
            return phoneListItem;
        }
    }
}
