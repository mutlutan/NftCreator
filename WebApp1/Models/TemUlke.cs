using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemUlke
    {
        public TemUlke()
        {
            TemSehir = new HashSet<TemSehir>();
        }

        public int Id { get; set; }
        public int Sira { get; set; }
        public string AlanKod { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }

        public virtual ICollection<TemSehir> TemSehir { get; set; }
    }
}
