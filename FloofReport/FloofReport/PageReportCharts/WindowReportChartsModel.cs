using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf.Converters;
using LiveCharts.Defaults;

namespace FloofReport
{
    public class WindowReportChartsModel : VMHelper
    {
        public Func<ChartPoint, string> PointLabel { get; set; }
        private PieSeries _pieChartSeries = new();
        public PieSeries PieChartSeries 
        {
            get
            {
                return _pieChartSeries;
            }
            set
            {
                _pieChartSeries = value;
                NotifyPropertyChanged(this, nameof(PieChartSeries));
            }
        }
        private List<EventItem> _events;
        public WindowReportChartsModel(List<EventItem> events)
        {
            _events = events;
            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            
        }
       
    }
}
