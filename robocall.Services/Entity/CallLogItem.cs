using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robocall.Services.Entity
{
    public class CallLogItem
    {
        public int CallLogId { get; set; }
        public int PhoneListId { get; set; }
        public string Result { get; set; }
        public DateTimeOffset AddedAt { get; set; }
    }

}
