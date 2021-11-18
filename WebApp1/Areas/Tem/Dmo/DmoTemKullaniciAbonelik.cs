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
 public class DmoTemKullaniciAbonelik : BaseDmo
 {
     public DmoTemKullaniciAbonelik(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemKullaniciAbonelik> Get()
     {
         return this.dataContext.TemKullaniciAbonelik.AsNoTracking()
             .Select(s => new DtoTemKullaniciAbonelik(this.dataContext)
             {
                 Id = s.Id,
                 KullaniciId = s.KullaniciId,
                 AbonelikDurumId = s.AbonelikDurumId,
                 AbonelikUrunPlanId = s.AbonelikUrunPlanId,
                 Jeton = s.Jeton,
                 OdemeFormuBilgileri = s.OdemeFormuBilgileri,
                 InsertUserId = s.InsertUserId,
                 UpdateUserId = s.UpdateUserId,
                 InsertDateTime = s.InsertDateTime,
                 UpdateDateTime = s.UpdateDateTime,
                 CcKullaniciIdAd = s.Kullanici.Ad.MyToTrim(),
                 CcAbonelikDurumIdAd = s.AbonelikDurum.Ad.MyToTrim(),
                 CcAbonelikUrunPlanIdAd = s.AbonelikUrunPlan.Ad.MyToTrim()
             });
     }

     public DtoTemKullaniciAbonelik GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemKullaniciAbonelik row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.KullaniciId = 0;
         row.AbonelikDurumId = 0;
         row.AbonelikUrunPlanId = 0;
         row.InsertUserId = this.dataContext.UserId;
         row.UpdateUserId = 0;
         row.InsertDateTime = DateTime.Now;

         return row;
     }

     public IEnumerable<DtoTemKullaniciAbonelik> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemKullaniciAbonelik _model)
     {
         TemKullaniciAbonelik row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemKullaniciAbonelik() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemKullaniciAbonelik");
         }
         else
         {
             row = this.dataContext.TemKullaniciAbonelik.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.KullaniciId = _model.KullaniciId;
         row.AbonelikDurumId = _model.AbonelikDurumId;
         row.AbonelikUrunPlanId = _model.AbonelikUrunPlanId;
         row.Jeton = _model.Jeton;
         row.OdemeFormuBilgileri = _model.OdemeFormuBilgileri;
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
             this.dataContext.TemKullaniciAbonelik.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemKullaniciAbonelik.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemKullaniciAbonelik.Remove(row);
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


