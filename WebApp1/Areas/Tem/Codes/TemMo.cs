using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Areas.Tem.Codes
{
    #region Modeller

    public class MoOturumInfo
    {
        public int KullaniciId { get; set; }
        public string KullaniciAd { get; set; }
        public DateTime? GirisZaman { get; set; }
        public DateTime? CikisZaman { get; set; }
        public int Zaman { get; set; }

    }

    #endregion
}
