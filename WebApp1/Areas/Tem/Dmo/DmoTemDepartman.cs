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
 public class DmoTemDepartman : BaseDmo
 {
     public DmoTemDepartman(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemDepartman> Get()
     {
         return this.dataContext.TemDepartman.AsNoTracking()
             .Select(s => new DtoTemDepartman(this.dataContext)
             {
                 Id = s.Id,
                 Durum = s.Durum,
                 UstId = s.UstId,
                 Sira = s.Sira,
                 Ad = s.Ad
             });
     }

     public DtoTemDepartman GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemDepartman row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = false;
         row.UstId = 0;
         row.Sira = 0;

         return row;
     }

     public IEnumerable<DtoTemDepartman> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemDepartman _model)
     {
         TemDepartman row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemDepartman() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemDepartman");
         }
         else
         {
             row = this.dataContext.TemDepartman.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Durum = _model.Durum;
         row.UstId = _model.UstId;
         row.Sira = _model.Sira;
         row.Ad = _model.Ad;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemDepartman.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemDepartman.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemDepartman.Remove(row);
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


