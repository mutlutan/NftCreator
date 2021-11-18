using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemAbonelikUrun
    {
        public TemAbonelikUrun()
        {
            TemAbonelikUrunPlan = new HashSet<TemAbonelikUrunPlan>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public int Sira { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }

        public virtual ICollection<TemAbonelikUrunPlan> TemAbonelikUrunPlan { get; set; }
    }
}
