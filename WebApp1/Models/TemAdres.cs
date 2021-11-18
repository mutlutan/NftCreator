using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemAdres
    {
        public TemAdres()
        {
            InverseUst = new HashSet<TemAdres>();
        }

        public int Id { get; set; }
        public int UstId { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }

        public virtual TemAdres Ust { get; set; }
        public virtual ICollection<TemAdres> InverseUst { get; set; }
    }
}
