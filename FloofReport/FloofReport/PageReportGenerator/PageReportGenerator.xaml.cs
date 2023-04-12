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
        PageReportGeneratorModel _model;

        public PageReportGenerator(HamsterBookContext context)
        {
            _model = new PageReportGeneratorModel(context);
            DataContext = _model;
            InitializeComponent();
            
        }

        private void cmbCage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _model.GetAllDatesForCage((Cage)(sender as ComboBox).SelectedItem);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            _model.GenerateRaport();
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            _model.AddSelectedDateToXExamine();
        }

        private void btnXEgsamineGenerate_Click(object sender, RoutedEventArgs e)
        {
            _model.GenerateXExamineReport();
        }

        private void btnDeleteDate_Click(object sender, RoutedEventArgs e)
        {
            _model.DeleteSelectedDate(dtgDates.SelectedIndex);
            FrameworkElement element = page;
            string outputFile = @"C:\Users\Krzysztof\Documents\Floof\page.pdf";
            PdfExporter.ExportToPdf(element, outputFile);
        }
    }
}
