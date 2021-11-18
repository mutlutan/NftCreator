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
 public class DmoTemOturumLog : BaseDmo
 {
     public DmoTemOturumLog(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemOturumLog> Get()
     {
         return this.dataContext.TemOturumLog.AsNoTracking()
             .Select(s => new DtoTemOturumLog(this.dataContext)
             {
                 Id = s.Id,
                 KullaniciId = s.KullaniciId,
                 Tarayici = s.Tarayici,
                 InternetProtokolAdres = s.InternetProtokolAdres,
                 OturumGuid = s.OturumGuid,
                 GirisZaman = s.GirisZaman,
                 CikisZaman = s.CikisZaman
             });
     }

     public DtoTemOturumLog GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemOturumLog row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.KullaniciId = 0;

         return row;
     }

     public IEnumerable<DtoTemOturumLog> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemOturumLog _model)
     {
         TemOturumLog row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemOturumLog() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemOturumLog");
         }
         else
         {
             row = this.dataContext.TemOturumLog.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.KullaniciId = _model.KullaniciId;
         row.Tarayici = _model.Tarayici;
         row.InternetProtokolAdres = _model.InternetProtokolAdres;
         row.OturumGuid = _model.OturumGuid;
         row.GirisZaman = _model.GirisZaman;
         row.CikisZaman = _model.CikisZaman;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemOturumLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemOturumLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemOturumLog.Remove(row);
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


