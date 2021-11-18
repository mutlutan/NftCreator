using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemOturumLog : TemOturumLog
    {
        protected readonly DataContext dataContext;

        public string CcKullaniciId
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   Areas.Tem.Codes.TemBusiness temBusiness = new(this.dataContext);
                   var queryResult = this.dataContext.TemKullanici.Where(c => c.Id == this.KullaniciId)
                   .Select(s => new { value = s.Id, text = s.Ad + " <" + temBusiness.GetKullaniciSahipAdSahipTur(s.Id) + ">" })
                   .FirstOrDefault();

                   if (queryResult != null)
                   {
                       rV = queryResult.text;
                   }
               }
               catch { }
               return rV;
           }
        }

        //Constructor
        public DtoTemOturumLog(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


