﻿using FloofReport.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FloofReport
{
    public class PageReportGeneratorModel
    {
        private HamsterBookContext _context;
        public ObservableCollection<Cage> Cages { get; set; }

        ReportCalculator calculator = new();

        public ObservableCollection<string> AvailabeTimes { get; set; } = new();

        public PageReportGeneratorModel(HamsterBookContext context) 
        {
            _context = context;
            string reportShort = "";
            InitCages();
            DateTime dt = new DateTime();
            dt = DateTime.Parse("10-01-2023");
            List<EventItem> ev = calculator.ManufactureReportTimeFrames(dt);
            List<EventItem> itemsToDisplay = calculator.GetWrappedEvents(ev);
            TimeSpan sum = new();
            foreach (EventItem e in ev)
            {
                //debugTxbt.Text += (e.AreaCode.ToString() + " at time: " + e.TimeSpan.ToString());
                sum += e.TimeSpan;
            }
            foreach(EventItem e in itemsToDisplay)
            {
                reportShort += "\nAreaCode: " + e.AreaCode.ToString() + " Time: " + e.TimeSpan.ToString() + " Activity: " + e.IsActive.ToString();
            }
            MessageBox.Show(reportShort);
            //GetAllDatesForCage();
        }
        private void InitCages()
        {
            var cages = from c in _context.Cages select c;
            Cages = new ObservableCollection<Cage>(cages.ToList());
        }

        public void GetAllDatesForCage(Cage cage)
        {
            AvailabeTimes.Clear();
            var distinctDays = _context.Visualevents
                .Where(x => x.Cageid == cage.Id)
                .Select(x => System.Data.Entity.DbFunctions.TruncateTime(x.Registrationtime))
                .Distinct()
                .ToList();
            foreach (var d in distinctDays)
            {
                AvailabeTimes.Add(d.ToString() ?? "");
            }
        }
    }
}
