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
 public class DmoTemMailHareket : BaseDmo
 {
     public DmoTemMailHareket(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemMailHareket> Get()
     {
         return this.dataContext.TemMailHareket.AsNoTracking()
             .Select(s => new DtoTemMailHareket(this.dataContext)
             {
                 Id = s.Id,
                 SablonId = s.SablonId,
                 KayitZaman = s.KayitZaman,
                 DurumId = s.DurumId,
                 DenemeSayisi = s.DenemeSayisi,
                 SonDenemeZaman = s.SonDenemeZaman,
                 Aciklama = s.Aciklama,
                 Adres = s.Adres,
                 Kopya = s.Kopya,
                 Gizli = s.Gizli,
                 Konu = s.Konu,
                 Icerik = s.Icerik,
                 CcSablonIdKonu = s.Sablon.Konu.MyToTrim(),
                 CcDurumIdAd = s.Durum.Ad.MyToTrim()
             });
     }

     public DtoTemMailHareket GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemMailHareket row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.SablonId = 0;
         row.DurumId = 0;
         row.DenemeSayisi = 0;

         return row;
     }

     public IEnumerable<DtoTemMailHareket> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemMailHareket _model)
     {
         TemMailHareket row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemMailHareket() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemMailHareket");
         }
         else
         {
             row = this.dataContext.TemMailHareket.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.SablonId = _model.SablonId;
         row.KayitZaman = _model.KayitZaman;
         row.DurumId = _model.DurumId;
         row.DenemeSayisi = _model.DenemeSayisi;
         row.SonDenemeZaman = _model.SonDenemeZaman;
         row.Aciklama = _model.Aciklama;
         row.Adres = _model.Adres;
         row.Kopya = _model.Kopya;
         row.Gizli = _model.Gizli;
         row.Konu = _model.Konu;
         row.Icerik = _model.Icerik;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemMailHareket.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemMailHareket.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemMailHareket.Remove(row);
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


