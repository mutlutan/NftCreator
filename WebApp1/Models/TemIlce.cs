using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemIlce
    {
        public int Id { get; set; }
        public int SehirId { get; set; }
        public int Sira { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }

        public virtual TemSehir Sehir { get; set; }
    }
}
