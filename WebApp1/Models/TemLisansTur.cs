using System;
using System.Collections.Generic;

namespace WebApp1.Models
{
    public partial class TemLisansTur
    {
        public TemLisansTur()
        {
            TemKullaniciLisans = new HashSet<TemKullaniciLisans>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public string Ad { get; set; }

        public virtual ICollection<TemKullaniciLisans> TemKullaniciLisans { get; set; }
    }
}
