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
        public int? InsertUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual TemKullanici Kullanici { get; set; }
    }
}
