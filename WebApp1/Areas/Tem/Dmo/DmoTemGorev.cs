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
 public class DmoTemGorev : BaseDmo
 {
     public DmoTemGorev(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemGorev> Get()
     {
         return this.dataContext.TemGorev.AsNoTracking()
             .Select(s => new DtoTemGorev(this.dataContext)
             {
                 Id = s.Id,
                 Durum = s.Durum,
                 Sira = s.Sira,
                 Ad = s.Ad
             });
     }

     public DtoTemGorev GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemGorev row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = false;
         row.Sira = this.dataContext.TemGorev.DefaultIfEmpty().Max(m => m == null ? 0 : m.Sira) + 1;

         return row;
     }

     public IEnumerable<DtoTemGorev> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemGorev _model)
     {
         TemGorev row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemGorev() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemGorev");
         }
         else
         {
             row = this.dataContext.TemGorev.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Durum = _model.Durum;
         row.Sira = _model.Sira;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemGorev.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemGorev.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemGorev.Remove(row);
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


