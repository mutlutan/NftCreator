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
 public class DmoTemMesaj : BaseDmo
 {
     public DmoTemMesaj(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemMesaj> Get()
     {
         return this.dataContext.TemMesaj.AsNoTracking()
             .Select(s => new DtoTemMesaj(this.dataContext)
             {
                 Id = s.Id,
                 UstId = s.UstId,
                 GondericiId = s.GondericiId,
                 AliciId = s.AliciId,
                 Baslik = s.Baslik,
                 Icerik = s.Icerik,
                 CcGondericiIdAd = s.Gonderici.Ad.MyToTrim(),
                 CcAliciIdAd = s.Alici.Ad.MyToTrim()
             });
     }

     public DtoTemMesaj GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemMesaj row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.UstId = 0;
         row.GondericiId = 0;
         row.AliciId = 0;

         return row;
     }

     public IEnumerable<DtoTemMesaj> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemMesaj _model)
     {
         TemMesaj row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemMesaj() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemMesaj");
         }
         else
         {
             row = this.dataContext.TemMesaj.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.UstId = _model.UstId;
         row.GondericiId = _model.GondericiId;
         row.AliciId = _model.AliciId;
         row.Baslik = _model.Baslik;
         row.Icerik = _model.Icerik;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemMesaj.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemMesaj.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemMesaj.Remove(row);
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


