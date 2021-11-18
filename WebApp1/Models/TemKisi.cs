using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKisi
    {
        public TemKisi()
        {
            TemKisiDepartmanGorev = new HashSet<TemKisiDepartmanGorev>();
        }

        public int Id { get; set; }
        public bool Durum { get; set; }
        public DateTime KayitZaman { get; set; }
        public string KimlikNumarasi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Adres { get; set; }
        public int IlceId { get; set; }
        public string MobilTelefon { get; set; }
        public string SabitTelefon { get; set; }
        public string FaxNumarasi { get; set; }
        public string MailAdres { get; set; }
        public int EgitimDurumId { get; set; }
        public int CinsiyetId { get; set; }
        public string KanGrup { get; set; }
        public string Aciklama { get; set; }

        public virtual TemCinsiyet Cinsiyet { get; set; }
        public virtual TemEgitimDurum EgitimDurum { get; set; }
        public virtual TemIlce Ilce { get; set; }
        public virtual ICollection<TemKisiDepartmanGorev> TemKisiDepartmanGorev { get; set; }
    }
}
