using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemOturumLog
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public string Tarayici { get; set; }
        public string InternetProtokolAdres { get; set; }
        public string OturumGuid { get; set; }
        public DateTime? GirisZaman { get; set; }
        public DateTime? CikisZaman { get; set; }

        public virtual TemKullanici Kullanici { get; set; }
    }
}
