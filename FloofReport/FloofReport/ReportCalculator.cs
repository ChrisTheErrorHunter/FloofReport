using FloofReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace FloofReport
{
    public class ReportCalculator
    {
        HamsterBookContext _context = new HamsterBookContext();
        public ReportCalculator()
        {

        }

        public List<EventItem> ManufactureReportTimeFrames(DateTime targetDate)
        {

            List<Visualevent> visualevents = (from c in _context.Visualevents where c.Registrationtime.Day == targetDate.Day select c).ToList();
            List<EventItem> events = new List<EventItem>();
            int listLength = visualevents.Count;
            Visualevent currentEvent, nextEvent;
            for (int i = 0; i < listLength - 1; i++)
            {
                currentEvent = visualevents[i];
                nextEvent = visualevents[i + 1];
                TimeSpan ts = nextEvent.Registrationtime - currentEvent.Registrationtime;
                events.Add(new EventItem(ts, currentEvent.Areaid));
            }
            foreach (Visualevent vEvent in visualevents)
            {
                Console.WriteLine(vEvent.Registrationtime.ToString());
            }
            return events;
        }
    }

    public class EventItem
    {
        public TimeSpan TimeSpan { get; set; }
        public int AreaCode { get; set; }

        public EventItem(TimeSpan time, int areaCode)
        {
            TimeSpan = time;
            AreaCode = areaCode;
        }
    }
}
