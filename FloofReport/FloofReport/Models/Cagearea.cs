﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FloofReport.Models
{
    public partial class Cagearea
    {
        public Cagearea()
        {
            Visualevents = new HashSet<Visualevent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Cageid { get; set; }

        public virtual Cage Cage { get; set; }
        public virtual ICollection<Visualevent> Visualevents { get; set; }
    }
}