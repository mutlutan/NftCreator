using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemVersiyon
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string Komut { get; set; }
    }
}
