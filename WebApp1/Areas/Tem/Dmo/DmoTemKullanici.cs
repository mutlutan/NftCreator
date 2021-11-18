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
 public class DmoTemKullanici : BaseDmo
 {
     public DmoTemKullanici(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemKullanici> Get()
     {
         return this.dataContext.TemKullanici.AsNoTracking()
             .Select(s => new DtoTemKullanici(this.dataContext)
             {
                 Id = s.Id,
                 Durum = s.Durum,
                 KayitZaman = s.KayitZaman,
                 SahipTur = s.SahipTur,
                 SahipId = s.SahipId,
                 Ad = s.Ad,
                 Sifre = string.Empty,
                 Rols = s.Rols,
                 Resim = s.Resim
             });
     }

     public DtoTemKullanici GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemKullanici row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = true;
         row.KayitZaman = DateTime.Now;
         row.SahipId = 0;

         return row;
     }

     public IEnumerable<DtoTemKullanici> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemKullanici _model)
     {
         TemKullanici row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemKullanici() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemKullanici");
         }
         else
         {
             row = this.dataContext.TemKullanici.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Durum = _model.Durum;
         row.KayitZaman = _model.KayitZaman;
         row.SahipTur = _model.SahipTur;
         row.SahipId = _model.SahipId;
         row.Ad = _model.Ad;

         if (!string.IsNullOrEmpty(_model.Sifre))
         {
             row.Sifre = _model.Sifre.MyToEncryptPassword();
         }

         row.Rols = _model.Rols;
         row.Resim = _model.Resim;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemKullanici.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemKullanici.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemKullanici.Remove(row);
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


