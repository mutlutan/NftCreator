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
 public class DmoTemAbonelikUrunPlan : BaseDmo
 {
     public DmoTemAbonelikUrunPlan(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemAbonelikUrunPlan> Get()
     {
         return this.dataContext.TemAbonelikUrunPlan.AsNoTracking()
             .Select(s => new DtoTemAbonelikUrunPlan(this.dataContext)
             {
                 Id = s.Id,
                 AbonelikUrunId = s.AbonelikUrunId,
                 Durum = s.Durum,
                 Sira = s.Sira,
                 Kod = s.Kod,
                 Ad = s.Ad,
                 AbonelikDonemId = s.AbonelikDonemId,
                 Ucret = s.Ucret,
                 ParaBirimId = s.ParaBirimId,
                 Aciklama = s.Aciklama,
                 CcAbonelikUrunIdAd = s.AbonelikUrun.Ad.MyToTrim(),
                 CcAbonelikDonemIdAd = s.AbonelikDonem.Ad.MyToTrim(),
                 CcParaBirimIdAd = s.ParaBirim.Ad.MyToTrim()
             });
     }

     public DtoTemAbonelikUrunPlan GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemAbonelikUrunPlan row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.AbonelikUrunId = 0;
         row.Durum = true;
         row.Sira = 0;
         row.AbonelikDonemId = 0;
         row.Ucret = 0;
         row.ParaBirimId = 0;

         return row;
     }

     public IEnumerable<DtoTemAbonelikUrunPlan> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemAbonelikUrunPlan _model)
     {
         TemAbonelikUrunPlan row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemAbonelikUrunPlan() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemAbonelikUrunPlan");
         }
         else
         {
             row = this.dataContext.TemAbonelikUrunPlan.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.AbonelikUrunId = _model.AbonelikUrunId;
         row.Durum = _model.Durum;

         if (_model.Sira != 0 )
         {
             row.Sira = _model.Sira;
         } 
         else {
            row.Sira = this.dataContext.TemAbonelikUrunPlan
                .Where(c => c.AbonelikUrunId == _model.AbonelikUrunId)
                .DefaultIfEmpty().Max(m => m == null ? 0 : m.Sira) + 1;
         }

         row.Kod = _model.Kod;
         row.Ad = _model.Ad;
         row.AbonelikDonemId = _model.AbonelikDonemId;
         row.Ucret = _model.Ucret;
         row.ParaBirimId = _model.ParaBirimId;
         row.Aciklama = _model.Aciklama;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemAbonelikUrunPlan.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemAbonelikUrunPlan.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemAbonelikUrunPlan.Remove(row);
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


