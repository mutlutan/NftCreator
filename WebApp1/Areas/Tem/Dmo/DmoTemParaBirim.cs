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
 public class DmoTemParaBirim : BaseDmo
 {
     public DmoTemParaBirim(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemParaBirim> Get()
     {
         return this.dataContext.TemParaBirim.AsNoTracking()
             .Select(s => new DtoTemParaBirim(this.dataContext)
             {
                 Id = s.Id,
                 Updateable = s.Updateable,
                 UpdateDate = s.UpdateDate,
                 UpdateCode = s.UpdateCode,
                 Durum = s.Durum,
                 Sira = s.Sira,
                 Simge = s.Simge,
                 Kod = s.Kod,
                 Ad = s.Ad,
                 AltBirim = s.AltBirim
             });
     }

     public DtoTemParaBirim GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemParaBirim row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Updateable = false;
         row.Durum = false;
         row.Sira = 0;

         return row;
     }

     public IEnumerable<DtoTemParaBirim> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemParaBirim _model)
     {
         TemParaBirim row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemParaBirim() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemParaBirim");
         }
         else
         {
             row = this.dataContext.TemParaBirim.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Updateable = _model.Updateable;
         row.UpdateDate = _model.UpdateDate;
         row.UpdateCode = _model.UpdateCode;
         row.Durum = _model.Durum;
         row.Sira = _model.Sira;
         row.Simge = _model.Simge;
         row.Kod = _model.Kod;
         row.Ad = _model.Ad;
         row.AltBirim = _model.AltBirim;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemParaBirim.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemParaBirim.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemParaBirim.Remove(row);
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


