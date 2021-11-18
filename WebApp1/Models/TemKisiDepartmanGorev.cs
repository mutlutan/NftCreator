using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKisiDepartmanGorev
    {
        public int Id { get; set; }
        public int KisiId { get; set; }
        public DateTime? BaslangicTarih { get; set; }
        public DateTime? BitisTarih { get; set; }
        public int DepartmanId { get; set; }
        public int GorevId { get; set; }
        public string Aciklama { get; set; }

        public virtual TemDepartman Departman { get; set; }
        public virtual TemGorev Gorev { get; set; }
        public virtual TemKisi Kisi { get; set; }
    }
}
