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
 public class DmoTemIlce : BaseDmo
 {
     public DmoTemIlce(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemIlce> Get()
     {
         return this.dataContext.TemIlce.AsNoTracking()
             .Select(s => new DtoTemIlce(this.dataContext)
             {
                 Id = s.Id,
                 SehirId = s.SehirId,
                 Sira = s.Sira,
                 Kod = s.Kod,
                 Ad = s.Ad,
                 CcSehirIdAd = s.Sehir.Ad.MyToTrim()
             });
     }

     public DtoTemIlce GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemIlce row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.SehirId = 0;
         row.Sira = 0;

         return row;
     }

     public IEnumerable<DtoTemIlce> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemIlce _model)
     {
         TemIlce row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemIlce() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemIlce");
         }
         else
         {
             row = this.dataContext.TemIlce.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.SehirId = _model.SehirId;

         if (_model.Sira != 0 )
         {
             row.Sira = _model.Sira;
         } 
         else {
            row.Sira = this.dataContext.TemIlce
                .Where(c => c.SehirId == _model.SehirId)
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
             this.dataContext.TemIlce.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemIlce.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemIlce.Remove(row);
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


