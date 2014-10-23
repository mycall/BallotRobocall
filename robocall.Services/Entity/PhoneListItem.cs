using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robocall.Services.Entity
{
    public class PhoneListItem
    {
        public int PhoneListId { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public int CallCount { get; set; }
        public DateTimeOffset CalledAt { get; set; }
        public int CampaignId { get; set; }
    }
}
