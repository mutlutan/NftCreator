using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemAbonelikUrunPlan
    {
        public TemAbonelikUrunPlan()
        {
            TemKullaniciAbonelik = new HashSet<TemKullaniciAbonelik>();
        }

        public int Id { get; set; }
        public int AbonelikUrunId { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }
        public int AbonelikDonemId { get; set; }
        public decimal Ucret { get; set; }
        public int ParaBirimId { get; set; }
        public string Aciklama { get; set; }

        public virtual TemAbonelikDonem AbonelikDonem { get; set; }
        public virtual TemAbonelikUrun AbonelikUrun { get; set; }
        public virtual TemParaBirim ParaBirim { get; set; }
        public virtual ICollection<TemKullaniciAbonelik> TemKullaniciAbonelik { get; set; }
    }
}
