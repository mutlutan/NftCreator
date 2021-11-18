using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class NftProje
    {
        public int Id { get; set; }
        public Guid Gid { get; set; }
        public bool Durum { get; set; }
        public DateTime? TarihSaat { get; set; }
        public string Ad { get; set; }
        public int KullaniciId { get; set; }

        public virtual TemKullanici Kullanici { get; set; }
    }
}
