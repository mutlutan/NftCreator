﻿using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemEgitimDurum
    {
        public int Id { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Ad { get; set; }
    }
}
