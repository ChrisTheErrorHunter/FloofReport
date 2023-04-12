using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace FloofReport
{
    /// <summary>
    /// Interaction logic for WindowReportCharts.xaml
    /// </summary>
    public partial class WindowReportCharts : Window
    {
        private List<EventItem> _eventsWrapped = new List<EventItem>();
        
        public WindowReportCharts(List<EventItem> eventsWrapped, List<EventItem> eventsFull)
        {
            InitializeComponent();
            _eventsWrapped = eventsWrapped;
            InitlializePieChart(_eventsWrapped);
            InitializeActivityChart(eventsFull);
            FillReportTextBox();
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
        private void InitlializePieChart(List<EventItem> eventList)
        {
            SeriesCollection seriesCollection = Piee.Series;
            foreach (EventItem item in eventList)
            {
                PieSeries series = new PieSeries();
                series.Title = item.AreaName?.ToString() + (item.IsActive ? " Aktywność" : " Bierność");
                series.DataLabels = false;
                series.LabelPoint = PointLabel;
                series.Values = new ChartValues<ObservableValue> { new ObservableValue(item.TimeSpan.TotalMilliseconds) };
                seriesCollection.Add(series);
            }
        }

        private void InitializeActivityChart(List<EventItem> eventList)
        {
            SeriesCollection seriesCollection = chartActivity.Series;
            ChartValues<ObservablePoint> ints = new ChartValues<ObservablePoint>();
            StepLineSeries series = new StepLineSeries();
            series.Title = "Aktywność: \n1 - Aktywność \n0 - Bierność";
            series.DataLabels = false;
            series.LabelPoint = PointLabel;
            TimeSpan totalSpan = TimeSpan.Zero;
            bool previousActivity = !eventList[0].IsActive;
            foreach (EventItem item in eventList)
            {
                if (previousActivity == item.IsActive) // this is done to simplify the data so we don't get consecutive points on charts (they don't tell us much anyway)
                {
                    totalSpan += item.TimeSpan;
                    continue;
                }
                ints.Add(new ObservablePoint(totalSpan.TotalHours, item.IsActive ? 1 : 0));
                totalSpan += item.TimeSpan;
                previousActivity = item.IsActive;
            }
            series.Values = ints;
            seriesCollection.Add(series);
        }

        private TimeSpan ApproxRunningTime()
        {
            EventItem wheelActivity = (from e in _eventsWrapped where ((e.AreaName.StartsWith("Kół") || e.AreaName.StartsWith("Kolo") || e.AreaName.StartsWith("Kolko")) && e.IsActive == true) select e).FirstOrDefault();
            if (wheelActivity != null)
            {
                return wheelActivity.TimeSpan;
            }
            return TimeSpan.Zero;
        }

        private TimeSpan ApproxSleepTime()
        {
            EventItem houseSleep = (from e in _eventsWrapped where (e.AreaName.StartsWith("Dom") && e.IsActive == false) select e).FirstOrDefault();
            if (houseSleep != null)
            {
                return houseSleep.TimeSpan;
            }
            return TimeSpan.Zero;
        }

        private TimeSpan ApproxDrinkingTime()
        {
            EventItem waterActivity = (from e in _eventsWrapped where (e.AreaName.StartsWith("Poid") && e.IsActive == true) select e).FirstOrDefault();
            if (waterActivity != null)
            {
                return waterActivity.TimeSpan;
            }
            return TimeSpan.Zero;
        }

        private void FillReportTextBox()
        {
            foreach (EventItem e in _eventsWrapped)
            {
                txbReport.AppendText( "\nObszar: " + e.AreaName?.ToString() + (e.IsActive ? " Aktywność" : " Bierność") + " Czas: " + e.TimeSpan.ToString(@"hh\:mm\:ss"));
            }
            txbReport.AppendText("\nPrawodopodobny czas biegania na kołowrtoku określono na: " + ApproxRunningTime().ToString(@"hh\:mm\:ss"));
            txbReport.AppendText("\nPrawodopodobny czas snu w domku określono na: " + ApproxSleepTime().ToString(@"hh\:mm\:ss"));
            txbReport.AppendText("\nPrawodopodobny czas picia wody określono na: " + ApproxDrinkingTime().ToString(@"hh\:mm\:ss"));
        }

        private void btnExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            btnExportToPdf.Visibility = Visibility.Hidden;
            FrameworkElement element = windowReport;
            string outputFile = @"C:\Users\Krzysztof\Documents\Floof\page.pdf";
            PdfExporter.ExportToPdf(element, outputFile);
            btnExportToPdf.Visibility = Visibility.Visible;
        }
    }
}
