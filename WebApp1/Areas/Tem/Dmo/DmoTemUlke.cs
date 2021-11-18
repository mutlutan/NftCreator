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
 public class DmoTemUlke : BaseDmo
 {
     public DmoTemUlke(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemUlke> Get()
     {
         return this.dataContext.TemUlke.AsNoTracking()
             .Select(s => new DtoTemUlke(this.dataContext)
             {
                 Id = s.Id,
                 Sira = s.Sira,
                 AlanKod = s.AlanKod,
                 Kod = s.Kod,
                 Ad = s.Ad
             });
     }

     public DtoTemUlke GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemUlke row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Sira = 0;

         return row;
     }

     public IEnumerable<DtoTemUlke> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemUlke _model)
     {
         TemUlke row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemUlke() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemUlke");
         }
         else
         {
             row = this.dataContext.TemUlke.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         

         if (_model.Sira != 0 )
         {
             row.Sira = _model.Sira;
         } 
         else {
            row.Sira = this.dataContext.TemUlke
                .DefaultIfEmpty().Max(m => m == null ? 0 : m.Sira) + 1;
         }

         row.AlanKod = _model.AlanKod;
         row.Kod = _model.Kod;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemUlke.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemUlke.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemUlke.Remove(row);
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


