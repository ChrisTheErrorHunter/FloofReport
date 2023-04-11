using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloofReport
{
    public class EventItemsXExamine
    {
        public string Date { get; set; }
        public List<EventItem> Items { get; set;}

        public EventItemsXExamine(string date, List<EventItem> list) 
        {
            Date = date;
            Items = list;
        }
    }
}
