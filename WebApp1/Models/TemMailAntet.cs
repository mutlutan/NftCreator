using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemMailAntet
    {
        public TemMailAntet()
        {
            TemMailSablon = new HashSet<TemMailSablon>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public string Icerik { get; set; }

        public virtual ICollection<TemMailSablon> TemMailSablon { get; set; }
    }
}
