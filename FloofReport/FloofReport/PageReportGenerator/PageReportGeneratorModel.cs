using FloofReport.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FloofReport
{
    public class PageReportGeneratorModel : VMHelper
    {
        private HamsterBookContext _context;

        private List<EventItem> _wrappedData = new();
        public List<EventItem> WrappedData
        {
            get
            {
                return _wrappedData;
            }
            private set
            {
                _wrappedData = value;
            }
        }
        public ObservableCollection<Cage> Cages { get; set; }

        ReportCalculator calculator = new();

        public ObservableCollection<string> AvailableTimes { get; set; } = new();

        private string _selectedDate;
        public string SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; NotifyPropertyChanged(this, nameof(SelectedDate)); }
        }

        public PageReportGeneratorModel(HamsterBookContext context) 
        {
            _context = context;
            
            InitCages();
            
            //GetAllDatesForCage();
        }
        private void InitCages()
        {
            var cages = from c in _context.Cages select c;
            Cages = new ObservableCollection<Cage>(cages.ToList());
        }

        public void GetAllDatesForCage(Cage cage)
        {
            AvailableTimes.Clear();
            var dates = _context.Visualevents.AsEnumerable().AsQueryable();
            var distinctDays = dates
                .Where(x => x.Cageid == cage.Id)
                .Select(x => x.Registrationtime.Date)
                .Distinct().OrderBy(a => a.Date).ToList();
            foreach (var d in distinctDays)
            {
                AvailableTimes.Add(d.ToString("dd/MM/yyyy") ?? "");
            }
        }

        public void GenerateRaport()
        {
            string reportShort = "";
            DateTime dt = new DateTime();
            dt = DateTime.Parse(SelectedDate);
            List<EventItem> ev = calculator.ManufactureReportTimeFrames(dt);
            List<EventItem> itemsToDisplay = calculator.GetWrappedEvents(ev);
            TimeSpan sum = new();
            foreach (EventItem e in ev)
            {
                //debugTxbt.Text += (e.AreaCode.ToString() + " at time: " + e.TimeSpan.ToString());
                sum += e.TimeSpan;
            }
            foreach (EventItem e in itemsToDisplay)
            {
                reportShort += "\nAreaCode: " + e.AreaCode.ToString() + " Time: " + e.TimeSpan.ToString() + " Activity: " + e.IsActive.ToString();
            }
            MessageBox.Show(reportShort);
            WrappedData = itemsToDisplay;
            WindowReportChartsModel chartModel = new(WrappedData);
            WindowReportCharts chartWindow = new(chartModel, WrappedData, ev);
            chartWindow.Show();
        }
    }
}
