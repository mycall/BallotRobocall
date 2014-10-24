using robocall.Services.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Specialized;

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
            var result = connection.Query<PhoneListItem>(@"
                select top 1 pl.*
                from PhoneList pl
                inner join Campaigns c on c.CampaignId = pl.CampaignId
                where pl.CampaignId=@CampaignId 
	                and ISNULL(pl.CallCount, 0) < c.MaxCallCount
	                and SYSDATETIMEOFFSET() between c.StartDate and c.EndDate 
	                and (cast(GETDATE() as float) - floor(cast(GETDATE() as float))) - (cast(c.DayStartTime as float) - floor(cast(c.DayStartTime as float)))  >= 0
	                and (cast(c.DayEndTime as float) - floor(cast(c.DayEndTime as float))) - (cast(GETDATE() as float) - floor(cast(GETDATE() as float)))  >= 0
	                and CHARINDEX(datename(dw,getdate()), c.DaysInWeekActive) > 0
	                and (pl.[Status] is null or pl.[Status] not in ('done', 'calling')) order by ISNULL(pl.CallCount, 0) desc, pl.PhoneListId
                ", new { CampaignId = campaignId });
            var item = result.FirstOrDefault();
            if (item == null) return null;
            var updateResult = connection.Execute("update PhoneList set CallCount=ISNULL(CallCount, 0) + 1, Status = 'calling' where PhoneListId=@PhoneListId", new { PhoneListId = item.PhoneListId });
            if (updateResult != 1) return null;
            return item;
        }

        public PhoneListItem SetCallResult(string campaignName, string phoneNumber, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                status = "called";
            var phoneListItem = GetPhoneListItem(campaignName, phoneNumber);
            if (phoneListItem == null) return null;
            var updateResult = connection.Execute("update PhoneList set Status = @Status, CalledAt = @CalledAt where PhoneListId=@PhoneListId",
                new { Status = status, PhoneListId = phoneListItem.PhoneListId, CalledAt = DateTime.Now });
            if (updateResult != 1) return null;
            return phoneListItem;
        }

        public void SaveCallLog(string campaignName, string phoneNumber, NameValueCollection formCollection)
        {
            if (string.IsNullOrEmpty(campaignName) || string.IsNullOrEmpty(phoneNumber) || formCollection == null) return;

            var phoneListItem = GetPhoneListItem(campaignName, phoneNumber);
            if (phoneListItem == null) return;

            var insertResult = connection.Execute("insert into CallLog (PhoneListId, Result, AddedAt) values (@PhoneListId, @Status, @AddedAt)",
                new
                {
                    Status = string.Join(",", formCollection.AllKeys.OrderBy(k=>k).Select(key => "[" + key + ":" + formCollection[key] + "]")).ToString(),
                    PhoneListId = phoneListItem.PhoneListId,
                    AddedAt = DateTime.Now
                });
        }

    }
}
