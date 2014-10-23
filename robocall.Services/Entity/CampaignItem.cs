using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robocall.Services.Entity
{
    public class CampaignItem
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public int MaxCallCount { get; set; }
        public string AudioFileUrl { get; set; }
        public string CallerID { get; set; }
        public bool AnswerOnMedia { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTime DayStartTime { get; set; }
        public DateTime DayEndTime { get; set; }
        public string DaysInWeekActive { get; set; }
        public string StartTokenUrl { get; set; }
    }
}
