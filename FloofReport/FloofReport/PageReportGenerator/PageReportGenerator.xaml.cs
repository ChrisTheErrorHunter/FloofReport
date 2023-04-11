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
            model = new PageReportGeneratorModel(context);
            DataContext = model;
            InitializeComponent();
        }

        private void cmbCage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.GetAllDatesForCage((Cage)(sender as ComboBox).SelectedItem);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            model.GenerateRaport();
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            model.AddSelectedDateToXExamine();
        }

        private void btnXEgsamineGenerate_Click(object sender, RoutedEventArgs e)
        {
            model.GenerateXExamineReport();
        }

        private void btnDeleteDate_Click(object sender, RoutedEventArgs e)
        {
            model.DeleteSelectedDate(dtgDates.SelectedIndex);
        }
    }
}
