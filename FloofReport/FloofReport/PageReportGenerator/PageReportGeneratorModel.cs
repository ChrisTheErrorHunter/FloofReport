using FloofReport.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

        public ObservableCollection<string> XExamineDates { get; set; } = new();

        private ReportCalculator _calculator = new();

        public ObservableCollection<string> AvailableTimes { get; set; } = new();

        private DateTime _selectedDate;

        private ObservableCollection<string> _years = new();
        public ObservableCollection<string> Years { get { return _years; } set { _years = value; NotifyPropertyChanged(this, nameof(Years)); } }

        private ObservableCollection<string> _months = new();
        public ObservableCollection<string> Months { get { return _months; } set { _months = value; NotifyPropertyChanged(this, nameof(Months)); } }

        private ObservableCollection<string> _days = new();
        public ObservableCollection<string> Days { get { return _days; } set { _days = value; NotifyPropertyChanged(this, nameof(Days)); } }

        private string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; NotifyPropertyChanged(this, nameof(SelectedYear)); YearChosen(); }
        }

        private string _selectedMonth;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set { _selectedMonth = value; NotifyPropertyChanged(this, nameof(SelectedMonth)); MonthChosen(); }
        }

        private string _selectedDay;
        public string SelectedDay
        {
            get { return _selectedDay; }
            set { _selectedDay = value; NotifyPropertyChanged(this, nameof(SelectedDay)); DayChosen(); }
        }
        public PageReportGeneratorModel(HamsterBookContext context) 
        {
            _context = context;
            InitCages();
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
            InitYears();
        }

        public void GenerateRaport()
        {
            if (string.IsNullOrWhiteSpace(SelectedDay))
            {
                MessageBox.Show("Wybierz datę!");
                return;
            }
            List<EventItem> ev = _calculator.ManufactureReportTimeFrames(_selectedDate);
            List<EventItem> itemsToDisplay = _calculator.GetWrappedEvents(ev);
            WrappedData = itemsToDisplay;
            WindowReportCharts chartWindow = new(WrappedData, ev);
            chartWindow.Show();
        }

        private void InitYears()
        {
            var dates = AvailableTimes.Select(dateString => DateTime.ParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture));
            var distinctYears = dates.Select(date => date.Year.ToString()).Distinct().OrderByDescending(date => date).ToList();
            Years = new ObservableCollection<string>(distinctYears);
        }

        private void YearChosen()
        {
            var dates = AvailableTimes.Select(dateString => DateTime.ParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture));
            Months.Clear();
            Days.Clear();
            SelectedMonth = "";
            SelectedDay = "";
            var distinctMonthsInYear = dates
            .Where(date => date.Year.ToString() == SelectedYear)
            .Select(date => date.Month.ToString())
            .OrderByDescending(dates => dates)
            .Distinct()
            .ToList();
            Months = new ObservableCollection<string>(distinctMonthsInYear);
        }

        private void MonthChosen()
        {
            var dates = AvailableTimes.Select(dateString => DateTime.ParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture));
            Days.Clear();
            SelectedDay = "";
            var distinctDaysInMonthAndYear = dates
            .Where(date => date.Year.ToString() == SelectedYear && date.Month.ToString() == SelectedMonth)
            .Select(date => date.Day.ToString())
            .OrderByDescending(dates => dates)
            .Distinct()
            .ToList();
            Days = new ObservableCollection<string>(distinctDaysInMonthAndYear);
        }

        private void DayChosen()
        {
            if (string.IsNullOrWhiteSpace(SelectedDay) || string.IsNullOrWhiteSpace(SelectedMonth)) return;
            _selectedDate = new DateTime(int.Parse(SelectedYear), int.Parse(SelectedMonth), int.Parse(SelectedDay));
        }

        public void AddSelectedDateToXExamine()
        {
            if (string.IsNullOrWhiteSpace(SelectedDay))
            {
                MessageBox.Show("Wybierz datę!");
                return;
            }
            XExamineDates.Add(_selectedDate.ToString("dd/MM/yyyy"));
        }

        public List<EventItemsXExamine> GenerateXExamineData()
        {
            List<EventItemsXExamine> eventItemsList = new List<EventItemsXExamine>();
            foreach(string date in XExamineDates)
            {
                List<EventItem> events = _calculator.ManufactureReportTimeFrames(DateTime.Parse(date));
                List<EventItem> wrapped = _calculator.GetWrappedEvents(events);
                eventItemsList.Add(new EventItemsXExamine(date, wrapped));
            }
            return eventItemsList;
        }

        public void GenerateXExamineReport()
        {
            if(XExamineDates.Count < 2 || XExamineDates.Count > 30)
            {
                MessageBox.Show("Liczba dat do porównania musi być pomiędzy 2 a 30");
                return;
            }
            WindowXExamineReport window = new(GenerateXExamineData());
            window.Show();
        }

        public void DeleteSelectedDate(int index)
        {
            if (index < 0) return;
            XExamineDates.RemoveAt(index);
        }
    }
}
