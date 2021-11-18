using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemMailSablon
    {
        public TemMailSablon()
        {
            TemMailHareket = new HashSet<TemMailHareket>();
        }

        public int Id { get; set; }
        public int AntetId { get; set; }
        public string Kopya { get; set; }
        public string Gizli { get; set; }
        public string Alan { get; set; }
        public string Konu { get; set; }
        public string Icerik { get; set; }

        public virtual TemMailAntet Antet { get; set; }
        public virtual ICollection<TemMailHareket> TemMailHareket { get; set; }
    }
}
