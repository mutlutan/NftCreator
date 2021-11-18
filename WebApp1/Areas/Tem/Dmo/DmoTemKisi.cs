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
 public class DmoTemKisi : BaseDmo
 {
     public DmoTemKisi(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemKisi> Get()
     {
         return this.dataContext.TemKisi.AsNoTracking()
             .Select(s => new DtoTemKisi(this.dataContext)
             {
                 Id = s.Id,
                 Durum = s.Durum,
                 KayitZaman = s.KayitZaman,
                 KimlikNumarasi = s.KimlikNumarasi,
                 Ad = s.Ad,
                 Soyad = s.Soyad,
                 Adres = s.Adres,
                 IlceId = s.IlceId,
                 MobilTelefon = s.MobilTelefon,
                 SabitTelefon = s.SabitTelefon,
                 FaxNumarasi = s.FaxNumarasi,
                 MailAdres = s.MailAdres,
                 EgitimDurumId = s.EgitimDurumId,
                 CinsiyetId = s.CinsiyetId,
                 KanGrup = s.KanGrup,
                 Aciklama = s.Aciklama,
                 CcEgitimDurumIdAd = s.EgitimDurum.Ad.MyToTrim(),
                 CcCinsiyetIdAd = s.Cinsiyet.Ad.MyToTrim()
             });
     }

     public DtoTemKisi GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemKisi row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = true;
         row.KayitZaman = DateTime.Now;
         row.IlceId = 0;
         row.EgitimDurumId = 0;
         row.CinsiyetId = 0;

         return row;
     }

     public IEnumerable<DtoTemKisi> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemKisi _model)
     {
         TemKisi row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemKisi() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemKisi");
         }
         else
         {
             row = this.dataContext.TemKisi.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Durum = _model.Durum;
         row.KayitZaman = _model.KayitZaman;
         row.KimlikNumarasi = _model.KimlikNumarasi;
         row.Ad = _model.Ad;
         row.Soyad = _model.Soyad;
         row.Adres = _model.Adres;
         row.IlceId = _model.IlceId;
         row.MobilTelefon = _model.MobilTelefon;
         row.SabitTelefon = _model.SabitTelefon;
         row.FaxNumarasi = _model.FaxNumarasi;
         row.MailAdres = _model.MailAdres;
         row.EgitimDurumId = _model.EgitimDurumId;
         row.CinsiyetId = _model.CinsiyetId;
         row.KanGrup = _model.KanGrup;
         row.Aciklama = _model.Aciklama;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemKisi.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemKisi.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemKisi.Remove(row);
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


