using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemSehir
    {
        public TemSehir()
        {
            TemIlce = new HashSet<TemIlce>();
        }

        public int Id { get; set; }
        public int UlkeId { get; set; }
        public int Sira { get; set; }
        public string AlanKod { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }

        public virtual TemUlke Ulke { get; set; }
        public virtual ICollection<TemIlce> TemIlce { get; set; }
    }
}
