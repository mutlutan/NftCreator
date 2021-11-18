using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemMesaj
    {
        public TemMesaj()
        {
            InverseUst = new HashSet<TemMesaj>();
        }

        public int Id { get; set; }
        public int UstId { get; set; }
        public int GondericiId { get; set; }
        public int AliciId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }

        public virtual TemKullanici Alici { get; set; }
        public virtual TemKullanici Gonderici { get; set; }
        public virtual TemMesaj Ust { get; set; }
        public virtual ICollection<TemMesaj> InverseUst { get; set; }
    }
}
