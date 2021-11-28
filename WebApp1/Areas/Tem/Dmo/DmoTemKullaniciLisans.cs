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
 public class DmoTemKullaniciLisans : BaseDmo
 {
     public DmoTemKullaniciLisans(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemKullaniciLisans> Get()
     {
         return this.dataContext.TemKullaniciLisans.AsNoTracking()
             .Select(s => new DtoTemKullaniciLisans(this.dataContext)
             {
                 Id = s.Id,
                 KullaniciId = s.KullaniciId,
                 Durum = s.Durum,
                 BaslamaTarihi = s.BaslamaTarihi,
                 BitisTarihi = s.BitisTarihi,
                 InsertUserId = s.InsertUserId,
                 UpdateUserId = s.UpdateUserId,
                 InsertDateTime = s.InsertDateTime,
                 UpdateDateTime = s.UpdateDateTime,
                 CcKullaniciIdAd = s.Kullanici.Ad.MyToTrim()
             });
     }

     public DtoTemKullaniciLisans GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemKullaniciLisans row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.KullaniciId = 0;
         row.Durum = true;
         row.InsertUserId = this.dataContext.UserId;
         row.UpdateUserId = 0;
         row.InsertDateTime = DateTime.Now;

         return row;
     }

     public IEnumerable<DtoTemKullaniciLisans> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemKullaniciLisans _model)
     {
         TemKullaniciLisans row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemKullaniciLisans() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemKullaniciLisans");
         }
         else
         {
             row = this.dataContext.TemKullaniciLisans.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.KullaniciId = _model.KullaniciId;
         row.Durum = _model.Durum;
         row.BaslamaTarihi = _model.BaslamaTarihi;
         row.BitisTarihi = _model.BitisTarihi;
         row.InsertUserId = _model.InsertUserId;
         row.UpdateUserId = this.dataContext.UserId;
         row.InsertDateTime = _model.InsertDateTime;
         row.UpdateDateTime = DateTime.Now;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemKullaniciLisans.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemKullaniciLisans.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemKullaniciLisans.Remove(row);
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


