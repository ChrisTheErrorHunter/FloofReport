using FloofReport.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloofReport
{
    public class MainWindowModel
    {
        private HamsterBookContext _context = new();

        public PageReportGenerator PageReportGenerator { get; set; }
        
        public MainWindowModel()
        {
            PageReportGenerator = new(_context);
        }

        
    }
}
