using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKullaniciSifre
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public DateTime? KayitZaman { get; set; }
        public string Sifre { get; set; }

        public virtual TemKullanici Kullanici { get; set; }
    }
}
