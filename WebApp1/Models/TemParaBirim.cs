using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemParaBirim
    {
        public TemParaBirim()
        {
            TemDovizKurArsiv = new HashSet<TemDovizKurArsiv>();
        }

        public int Id { get; set; }
        public bool Updateable { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateCode { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Simge { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }
        public string AltBirim { get; set; }

        public virtual ICollection<TemDovizKurArsiv> TemDovizKurArsiv { get; set; }
    }
}
