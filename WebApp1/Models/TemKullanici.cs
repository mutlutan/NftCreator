using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKullanici
    {
        public TemKullanici()
        {
            NftProje = new HashSet<NftProje>();
            TemKullaniciAbonelik = new HashSet<TemKullaniciAbonelik>();
            TemKullaniciLisans = new HashSet<TemKullaniciLisans>();
            TemKullaniciSifre = new HashSet<TemKullaniciSifre>();
            TemMesajAlici = new HashSet<TemMesaj>();
            TemMesajGonderici = new HashSet<TemMesaj>();
            TemOturumLog = new HashSet<TemOturumLog>();
        }

        public int Id { get; set; }
        public Guid GizliId { get; set; }
        public bool Durum { get; set; }
        public DateTime? KayitZaman { get; set; }
        public string Ad { get; set; }
        public string Sifre { get; set; }
        public string Rols { get; set; }
        public string Resim { get; set; }

        public virtual ICollection<NftProje> NftProje { get; set; }
        public virtual ICollection<TemKullaniciAbonelik> TemKullaniciAbonelik { get; set; }
        public virtual ICollection<TemKullaniciLisans> TemKullaniciLisans { get; set; }
        public virtual ICollection<TemKullaniciSifre> TemKullaniciSifre { get; set; }
        public virtual ICollection<TemMesaj> TemMesajAlici { get; set; }
        public virtual ICollection<TemMesaj> TemMesajGonderici { get; set; }
        public virtual ICollection<TemOturumLog> TemOturumLog { get; set; }
    }
}
