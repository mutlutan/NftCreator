using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKullaniciLisans
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public bool Durum { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        public virtual TemKullanici Kullanici { get; set; }
    }
}
