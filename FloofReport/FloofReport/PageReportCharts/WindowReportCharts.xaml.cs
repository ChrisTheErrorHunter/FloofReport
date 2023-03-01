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

        private WindowReportChartsModel _model;
        private List<EventItem> _events = new List<EventItem>();
        public WindowReportCharts(WindowReportChartsModel model, List<EventItem> events)
        {
            InitializeComponent();
            _events = events;
            _model = model;
            DataContext = _model;
            InitlializePieChart(_events);
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
        private void InitlializePieChart(List<EventItem> eventList)
        {
            SeriesCollection seriesCollection = Piee.Series;
            foreach (EventItem item in eventList)
            {
                PieSeries series = new PieSeries();
                series.Title = item.AreaCode.ToString() + (item.IsActive ? " Aktywność" : " Bierność");
                series.DataLabels = false;
                series.LabelPoint = PointLabel;
                series.Values = new ChartValues<ObservableValue> { new ObservableValue(item.TimeSpan.TotalMilliseconds) };
                seriesCollection.Add(series);
            }
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;
            SeriesCollection sc = new();
            sc.Add(new PieSeries());
            List<PieSeries> pi = new();
            Piee.Series = sc;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
