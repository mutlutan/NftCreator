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
                 Kod = s.Kod,
                 Ad = s.Ad,
                 Sifre = string.Empty,
                 Rols = s.Rols,
                 Resim = s.Resim,
                 AdSoyad = s.AdSoyad,
                 DogumTarihi = s.DogumTarihi,
                 InsertUserId = s.InsertUserId,
                 UpdateUserId = s.UpdateUserId,
                 InsertDateTime = s.InsertDateTime,
                 UpdateDateTime = s.UpdateDateTime
             });
     }

     public DtoTemKullanici GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemKullanici row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = true;
         row.Kod = "*";
         row.InsertUserId = this.dataContext.UserId;
         row.UpdateUserId = 0;
         row.InsertDateTime = DateTime.Now;

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

         if (_model.Kod.MyToTrim() == "*" )
         {
            string yilAy = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM").MyMoonToStr();
            int inc = 0;

            var lastDataRow = this.dataContext.TemKullanici.AsNoTracking().Where(c => c.Kod.StartsWith(yilAy)).OrderBy(o => o.Kod).LastOrDefault();
            if (lastDataRow != null)
            {
                inc = lastDataRow.Kod.Replace(yilAy, "").MyToInt();
            }

            inc++;
            row.Kod = yilAy + inc.MyToStr().PadLeft(3, '0');
         }
         else
         {
             row.Kod = _model.Kod;
         }

         row.Ad = _model.Ad;

         if (!string.IsNullOrEmpty(_model.Sifre))
         {
             row.Sifre = _model.Sifre.MyToEncryptPassword();
         }

         row.Rols = _model.Rols;
         row.Resim = _model.Resim;
         row.AdSoyad = _model.AdSoyad;
         row.DogumTarihi = _model.DogumTarihi;
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


