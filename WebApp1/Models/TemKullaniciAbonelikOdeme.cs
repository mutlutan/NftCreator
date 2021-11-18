using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKullaniciAbonelikOdeme
    {
        public int Id { get; set; }
        public int KullaniciAbonelikId { get; set; }
        public bool Durum { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string YanitIcerigi { get; set; }
        public int? InsertUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual TemKullaniciAbonelik KullaniciAbonelik { get; set; }
    }
}
