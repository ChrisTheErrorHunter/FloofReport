using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloofReport
{
    public class EventItem
    {
        public TimeSpan TimeSpan { get; set; }
        public int AreaCode { get; set; }
        public bool IsActive { get; set; }
        public string? AreaName { get; set; }

        public EventItem(TimeSpan time, int areaCode, bool isActive, string? areaName = null)
        {
            TimeSpan = time;
            AreaCode = areaCode;
            IsActive = isActive;
            AreaName = areaName;
        }
    }
}
