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
    /// Interaction logic for WindowXExamineReport.xaml
    /// </summary>
    public partial class WindowXExamineReport : Window
    {
        private List<EventItemsXExamine> _eventItemsList;
        public WindowXExamineReport(List<EventItemsXExamine> eventItemsList)
        {
            InitializeComponent();
            _eventItemsList = eventItemsList;
            InitializeActivityChart();
            InitializeWheelChart();
            InitializeHouseChart();
            InitializeWaterTankChart();
        }

        private void InitializeActivityChart()
        {
            SeriesCollection seriesCollection = chartActivity.Series;
            foreach(EventItemsXExamine eventItems in _eventItemsList)
            {
                ColumnSeries series = new();
                TimeSpan ts = TimeSpan.Zero;
                foreach(EventItem item in eventItems.Items)
                {
                    if(item.IsActive)
                    {
                        ts += item.TimeSpan;
                    }
                }
                series.Title = eventItems.Date;
                series.DataLabels = false;
                series.Values = new ChartValues<ObservableValue> { new ObservableValue(ts.TotalSeconds) };
                seriesCollection.Add(series);
            }
        }

        private void InitializeWheelChart()
        {
            SeriesCollection seriesCollection = chartWheel.Series;
            foreach (EventItemsXExamine eventItems in _eventItemsList)
            {
                ColumnSeries series = new();
                TimeSpan ts = TimeSpan.Zero;
                EventItem wheelItem = eventItems.Items.Where(x => (x.AreaName ?? "").StartsWith("Kół") && x.IsActive == true).Select(x => x).First();
                series.Title = eventItems.Date;
                series.DataLabels = false;
                series.Values = new ChartValues<ObservableValue> { new ObservableValue(wheelItem.TimeSpan.TotalSeconds) };
                seriesCollection.Add(series);
            }
        }

        private void InitializeHouseChart()
        {
            SeriesCollection seriesCollection = chartHouse.Series;
            foreach (EventItemsXExamine eventItems in _eventItemsList)
            {
                ColumnSeries series = new();
                TimeSpan ts = TimeSpan.Zero;
                EventItem houseItem = eventItems.Items.Where(x => (x.AreaName ?? "").StartsWith("Dom") && x.IsActive == false).Select(x => x).First();
                series.Title = eventItems.Date;
                series.DataLabels = false;
                series.Values = new ChartValues<ObservableValue> { new ObservableValue(houseItem.TimeSpan.TotalSeconds) };
                seriesCollection.Add(series);
            }
        }

        private void InitializeWaterTankChart()
        {
            SeriesCollection seriesCollection = chartWaterTank.Series;
            foreach (EventItemsXExamine eventItems in _eventItemsList)
            {
                ColumnSeries series = new();
                TimeSpan ts = TimeSpan.Zero;
                EventItem waterItem = eventItems.Items.Where(x => (x.AreaName ?? "").StartsWith("Poid") && x.IsActive == true).Select(x => x).First();
                series.Title = eventItems.Date;
                series.DataLabels = false;
                series.Values = new ChartValues<ObservableValue> { new ObservableValue(waterItem.TimeSpan.TotalSeconds) };
                seriesCollection.Add(series);
            }
        }
    }
}
