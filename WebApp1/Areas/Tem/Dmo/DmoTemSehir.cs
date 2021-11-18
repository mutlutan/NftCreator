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
 public class DmoTemSehir : BaseDmo
 {
     public DmoTemSehir(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemSehir> Get()
     {
         return this.dataContext.TemSehir.AsNoTracking()
             .Select(s => new DtoTemSehir(this.dataContext)
             {
                 Id = s.Id,
                 UlkeId = s.UlkeId,
                 Sira = s.Sira,
                 AlanKod = s.AlanKod,
                 Kod = s.Kod,
                 Ad = s.Ad,
                 CcUlkeIdAd = s.Ulke.Ad.MyToTrim()
             });
     }

     public DtoTemSehir GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemSehir row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.UlkeId = 0;
         row.Sira = 0;

         return row;
     }

     public IEnumerable<DtoTemSehir> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemSehir _model)
     {
         TemSehir row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemSehir() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemSehir");
         }
         else
         {
             row = this.dataContext.TemSehir.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.UlkeId = _model.UlkeId;

         if (_model.Sira != 0 )
         {
             row.Sira = _model.Sira;
         } 
         else {
            row.Sira = this.dataContext.TemSehir
                .Where(c => c.UlkeId == _model.UlkeId)
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
             this.dataContext.TemSehir.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemSehir.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemSehir.Remove(row);
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


