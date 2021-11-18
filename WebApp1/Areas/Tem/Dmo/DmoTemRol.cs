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
 public class DmoTemRol : BaseDmo
 {
     public DmoTemRol(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemRol> Get()
     {
         return this.dataContext.TemRol.AsNoTracking()
             .Select(s => new DtoTemRol(this.dataContext)
             {
                 Id = s.Id,
                 Sira = s.Sira,
                 Ad = s.Ad,
                 Yetki = s.Yetki
             });
     }

     public DtoTemRol GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemRol row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Sira = this.dataContext.TemRol.DefaultIfEmpty().Max(m => m == null ? 0 : m.Sira) + 1;

         return row;
     }

     public IEnumerable<DtoTemRol> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemRol _model)
     {
         TemRol row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemRol() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemRol");
         }
         else
         {
             row = this.dataContext.TemRol.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Sira = _model.Sira;
         row.Ad = _model.Ad;
         row.Yetki = _model.Yetki;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemRol.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemRol.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemRol.Remove(row);
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


