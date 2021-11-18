using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;
using WebApp1.Areas.Tem.Dto;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dmo
{
 public class DmoTemAbonelikUrun : BaseDmo
 {
     public DmoTemAbonelikUrun(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemAbonelikUrun> Get()
     {
         return this.dataContext.TemAbonelikUrun.AsNoTracking()
             .Select(s => new DtoTemAbonelikUrun(this.dataContext)
             {
                 Id = s.Id,
                 Durum = s.Durum,
                 Sira = s.Sira,
                 Kod = s.Kod,
                 Ad = s.Ad
             });
     }

     public DtoTemAbonelikUrun GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemAbonelikUrun row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = true;
         row.Sira = 0;

         return row;
     }

     public IEnumerable<DtoTemAbonelikUrun> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemAbonelikUrun _model)
     {
         TemAbonelikUrun row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemAbonelikUrun() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemAbonelikUrun");
         }
         else
         {
             row = this.dataContext.TemAbonelikUrun.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Durum = _model.Durum;

         if (_model.Sira != 0 )
         {
             row.Sira = _model.Sira;
         } 
         else {
            row.Sira = this.dataContext.TemAbonelikUrun
                .DefaultIfEmpty().Max(m => m == null ? 0 : m.Sira) + 1;
         }

         row.Kod = _model.Kod;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemAbonelikUrun.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemAbonelikUrun.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemAbonelikUrun.Remove(row);
             rV = true;
         }
         else
         {
             throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
         }

         return rV;
     }

     #region Ek fonksiyonlar

     #endregion

 }

}


