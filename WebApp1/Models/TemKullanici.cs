using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKullanici
    {
        public TemKullanici()
        {
            TemKullaniciLisans = new HashSet<TemKullaniciLisans>();
            TemKullaniciSifre = new HashSet<TemKullaniciSifre>();
            TemMesajAlici = new HashSet<TemMesaj>();
            TemMesajGonderici = new HashSet<TemMesaj>();
            TemOturumLog = new HashSet<TemOturumLog>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }
        public string Sifre { get; set; }
        public string Rols { get; set; }
        public string Resim { get; set; }
        public string AdSoyad { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public int? InsertUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<TemKullaniciLisans> TemKullaniciLisans { get; set; }
        public virtual ICollection<TemKullaniciSifre> TemKullaniciSifre { get; set; }
        public virtual ICollection<TemMesaj> TemMesajAlici { get; set; }
        public virtual ICollection<TemMesaj> TemMesajGonderici { get; set; }
        public virtual ICollection<TemOturumLog> TemOturumLog { get; set; }
    }
}
