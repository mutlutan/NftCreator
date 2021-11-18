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
 public class DmoTemAdres : BaseDmo
 {
     public DmoTemAdres(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemAdres> Get()
     {
         return this.dataContext.TemAdres.AsNoTracking()
             .Select(s => new DtoTemAdres(this.dataContext)
             {
                 Id = s.Id,
                 UstId = s.UstId,
                 Kod = s.Kod,
                 Ad = s.Ad
             });
     }

     public DtoTemAdres GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemAdres row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.UstId = 0;

         return row;
     }

     public IEnumerable<DtoTemAdres> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemAdres _model)
     {
         TemAdres row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemAdres() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemAdres");
         }
         else
         {
             row = this.dataContext.TemAdres.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.UstId = _model.UstId;
         row.Kod = _model.Kod;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemAdres.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemAdres.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemAdres.Remove(row);
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


