using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemAbonelikDurum
    {
        public TemAbonelikDurum()
        {
            TemKullaniciAbonelik = new HashSet<TemKullaniciAbonelik>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }

        public virtual ICollection<TemKullaniciAbonelik> TemKullaniciAbonelik { get; set; }
    }
}
