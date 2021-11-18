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
 public class DmoTemKullaniciAbonelikOdeme : BaseDmo
 {
     public DmoTemKullaniciAbonelikOdeme(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemKullaniciAbonelikOdeme> Get()
     {
         return this.dataContext.TemKullaniciAbonelikOdeme.AsNoTracking()
             .Select(s => new DtoTemKullaniciAbonelikOdeme(this.dataContext)
             {
                 Id = s.Id,
                 KullaniciAbonelikId = s.KullaniciAbonelikId,
                 Durum = s.Durum,
                 BaslamaTarihi = s.BaslamaTarihi,
                 BitisTarihi = s.BitisTarihi,
                 YanitIcerigi = s.YanitIcerigi,
                 InsertUserId = s.InsertUserId,
                 UpdateUserId = s.UpdateUserId,
                 InsertDateTime = s.InsertDateTime,
                 UpdateDateTime = s.UpdateDateTime
             });
     }

     public DtoTemKullaniciAbonelikOdeme GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemKullaniciAbonelikOdeme row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.KullaniciAbonelikId = 0;
         row.Durum = true;
         row.InsertUserId = this.dataContext.UserId;
         row.UpdateUserId = 0;
         row.InsertDateTime = DateTime.Now;

         return row;
     }

     public IEnumerable<DtoTemKullaniciAbonelikOdeme> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemKullaniciAbonelikOdeme _model)
     {
         TemKullaniciAbonelikOdeme row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemKullaniciAbonelikOdeme() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemKullaniciAbonelikOdeme");
         }
         else
         {
             row = this.dataContext.TemKullaniciAbonelikOdeme.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.KullaniciAbonelikId = _model.KullaniciAbonelikId;
         row.Durum = _model.Durum;
         row.BaslamaTarihi = _model.BaslamaTarihi;
         row.BitisTarihi = _model.BitisTarihi;
         row.YanitIcerigi = _model.YanitIcerigi;
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
             this.dataContext.TemKullaniciAbonelikOdeme.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemKullaniciAbonelikOdeme.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemKullaniciAbonelikOdeme.Remove(row);
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


