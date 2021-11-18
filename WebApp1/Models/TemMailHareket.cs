using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemMailHareket
    {
        public int Id { get; set; }
        public int SablonId { get; set; }
        public DateTime? KayitZaman { get; set; }
        public int DurumId { get; set; }
        public int DenemeSayisi { get; set; }
        public DateTime? SonDenemeZaman { get; set; }
        public string Aciklama { get; set; }
        public string Adres { get; set; }
        public string Kopya { get; set; }
        public string Gizli { get; set; }
        public string Konu { get; set; }
        public string Icerik { get; set; }

        public virtual TemMailHareketDurum Durum { get; set; }
        public virtual TemMailSablon Sablon { get; set; }
    }
}
