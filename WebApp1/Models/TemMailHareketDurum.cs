using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemMailHareketDurum
    {
        public TemMailHareketDurum()
        {
            TemMailHareket = new HashSet<TemMailHareket>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }

        public virtual ICollection<TemMailHareket> TemMailHareket { get; set; }
    }
}
