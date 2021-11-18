using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemKisi : TemKisi
    {
        protected readonly DataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? MyApp.TranslateTo("xLng.Aktif", this.dataContext.Language) : MyApp.TranslateTo("xLng.Pasif", this.dataContext.Language)); }
        }
        public string CcIlceId
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   var queryResult = this.dataContext.TemIlce.Where(c => c.Id == this.IlceId)
                       .Select(s => new { value = s.Id, text = s.Ad.MyToTrim() + " / " + s.Sehir.Ad.MyToTrim() + " / " + s.Sehir.Ulke.Ad.MyToTrim() })
                       .FirstOrDefault();

                   if (queryResult != null)
                   {
                       rV += queryResult.text;
                   }
               }
               catch { }
               return rV;
           }
        }
        public string CcEgitimDurumIdAd{ get; set; }
        public string CcCinsiyetIdAd{ get; set; }
        public string CcKanGrup
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.KanGrup.MyToTrim();
               }
               catch { }
               return rV;
           }
        }

        //Constructor
        public DtoTemKisi(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


