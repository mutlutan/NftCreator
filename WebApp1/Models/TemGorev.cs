using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemGorev
    {
        public TemGorev()
        {
            TemKisiDepartmanGorev = new HashSet<TemKisiDepartmanGorev>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Ad { get; set; }

        public virtual ICollection<TemKisiDepartmanGorev> TemKisiDepartmanGorev { get; set; }
    }
}
