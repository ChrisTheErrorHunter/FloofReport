﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FloofReport.Models
{
    public partial class Visualevent
    {
        public int Id { get; set; }
        public DateTime Registrationtime { get; set; }
        public int? Cageid { get; set; }
        public int Hamsterid { get; set; }
        public int Areaid { get; set; }

        public virtual Cagearea Area { get; set; }
        public virtual Cage Cage { get; set; }
        public virtual Animal Hamster { get; set; }
    }
}