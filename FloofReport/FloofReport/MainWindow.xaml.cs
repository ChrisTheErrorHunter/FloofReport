using FloofReport.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FloofReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReportCalculator calculator = new();
        public MainWindow()
        {
            InitializeComponent();
            DateTime dt = new DateTime();
            dt = DateTime.Parse("10-01-2023");
            List<EventItem> ev =  calculator.ManufactureReportTimeFrames(dt);
            TimeSpan sum = new();
            foreach(EventItem e in ev)
            {
                debugTxbt.Text += (e.AreaCode.ToString() + " at time: " + e.TimeSpan.ToString());
                sum += e.TimeSpan;
            }
            MessageBox.Show(sum.ToString());
        }
    }
}
