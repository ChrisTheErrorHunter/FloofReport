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
        
        public MainWindowModel()
        {
            InitCages();
        }

        
    }
}
