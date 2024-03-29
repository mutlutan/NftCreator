using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemKullanici : TemKullanici
    {
        protected readonly DataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? MyApp.TranslateTo("xLng.Aktif", this.dataContext.Language) : MyApp.TranslateTo("xLng.Pasif", this.dataContext.Language)); }
        }
        public string CcRolsAd{
            get {
                string rV = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(this.Rols) && this.Rols.Trim().Length > 0)
                    {
                        foreach (string s in this.Rols.Split(','))
                        {
                            int id = Convert.ToInt32(s.MyToInt());
                            if (rV != string.Empty) { rV += MyApp.Ayrac; }
                            rV += this.dataContext.TemRol.Where(c => c.Id == id).FirstOrDefault().Ad.MyToTrim();
                        }
                    }
                }
                catch { }
                return rV;
            }
        }
        public string CcInsertUserId
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   Areas.Tem.Codes.TemBusiness temBusiness = new(this.dataContext);
                   var queryResult = this.dataContext.TemKullanici.Where(c => c.Id == this.InsertUserId)
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
        public string CcUpdateUserId
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   Areas.Tem.Codes.TemBusiness temBusiness = new(this.dataContext);
                   var queryResult = this.dataContext.TemKullanici.Where(c => c.Id == this.UpdateUserId)
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
        public DtoTemKullanici(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


