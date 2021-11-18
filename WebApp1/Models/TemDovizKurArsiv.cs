using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemDovizKurArsiv
    {
        public int Id { get; set; }
        public int ParaBirimId { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime TarihSaat { get; set; }
        public decimal DovizAlis { get; set; }
        public decimal DovizSatis { get; set; }
        public decimal EfektifAlis { get; set; }
        public decimal EfektifSatis { get; set; }

        public virtual TemParaBirim ParaBirim { get; set; }
    }
}
