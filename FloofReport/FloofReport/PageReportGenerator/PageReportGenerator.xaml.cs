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
    /// Interaction logic for PageReportGenerator.xaml
    /// </summary>
    public partial class PageReportGenerator : Page
    {
        PageReportGeneratorModel model;
        public PageReportGenerator(HamsterBookContext context)
        {
            InitializeComponent();
            model = new PageReportGeneratorModel(context);
            DataContext = model;

        }
    }
}
