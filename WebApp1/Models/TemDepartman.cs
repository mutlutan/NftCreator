using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemDepartman
    {
        public TemDepartman()
        {
            InverseUst = new HashSet<TemDepartman>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int UstId { get; set; }
        public decimal Sira { get; set; }
        public string Ad { get; set; }

        public virtual TemDepartman Ust { get; set; }
        public virtual ICollection<TemDepartman> InverseUst { get; set; }
    }
}
