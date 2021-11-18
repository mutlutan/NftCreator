using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemKullaniciAbonelik
    {
        public TemKullaniciAbonelik()
        {
            TemKullaniciAbonelikOdeme = new HashSet<TemKullaniciAbonelikOdeme>();
        }

        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public int AbonelikDurumId { get; set; }
        public int AbonelikUrunPlanId { get; set; }
        public string Jeton { get; set; }
        public string OdemeFormuBilgileri { get; set; }
        public int? InsertUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual TemAbonelikDurum AbonelikDurum { get; set; }
        public virtual TemAbonelikUrunPlan AbonelikUrunPlan { get; set; }
        public virtual TemKullanici Kullanici { get; set; }
        public virtual ICollection<TemKullaniciAbonelikOdeme> TemKullaniciAbonelikOdeme { get; set; }
    }
}
