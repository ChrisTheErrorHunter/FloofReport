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
        public ReportCalculator() { }

        public List<EventItem> ManufactureReportTimeFrames(DateTime targetDate, Cage cage)
        {

            List<Visualevent> visualevents = (from c in _context.Visualevents
                                              where (c.Registrationtime.Date == targetDate.Date && c.Cageid == cage.Id)
                                              select c).OrderBy(ev => ev.Registrationtime).ToList();
            List<EventItem> events = new List<EventItem>();
            int listLength = visualevents.Count;
            Visualevent currentEvent, nextEvent;
            for (int i = 0; i < listLength - 1; i++)
            {
                currentEvent = visualevents[i];
                nextEvent = visualevents[i + 1];
                TimeSpan ts = nextEvent.Registrationtime - currentEvent.Registrationtime;
                if(ts < TimeSpan.FromSeconds(90))
                {
                    events.Add(new EventItem(ts, currentEvent.Areaid, true));
                }
                else
                {
                    events.Add(new EventItem(ts, currentEvent.Areaid, false));
                }
            }
            return events;
        }

        public List<EventItem> GetWrappedEvents(List<EventItem> eventItemsUnwrapped)
        {
            List<EventItem> eventsWrapped = new();
            var areas = (from c in eventItemsUnwrapped select c.AreaCode).Distinct();
            foreach(int area in areas)
            {
                string ca = (from c in _context.Cageareas where c.Id == area select c.Name).FirstOrDefault() ?? "Error";
                eventsWrapped.Add(new EventItem(TimeSpan.Zero, area, true, ca));
                eventsWrapped.Add(new EventItem(TimeSpan.Zero, area, false, ca));
            }
            foreach(EventItem eventItem in eventItemsUnwrapped)
            {
                foreach(EventItem item in eventsWrapped)
                {
                    if(item.AreaCode == eventItem.AreaCode && item.IsActive == eventItem.IsActive)
                    {
                        item.TimeSpan += eventItem.TimeSpan;
                    }
                }
            }
            return eventsWrapped;
        }
    }
}
