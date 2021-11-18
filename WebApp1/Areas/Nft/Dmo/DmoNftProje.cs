using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;
using WebApp1.Areas.Nft.Dto;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Nft.Dmo
{
 public class DmoNftProje : BaseDmo
 {
     public DmoNftProje(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoNftProje> Get()
     {
         return this.dataContext.NftProje.AsNoTracking()
             .Select(s => new DtoNftProje(this.dataContext)
             {
                 Id = s.Id,
                 GizliId = s.GizliId,
                 Durum = s.Durum,
                 TarihSaat = s.TarihSaat,
                 Ad = s.Ad,
                 KullaniciId = s.KullaniciId,
                 CcKullaniciIdAd = s.Kullanici.Ad.MyToTrim()
             });
     }

     public DtoNftProje GetByNew()
     {
         //Default değerler ile bir row döner
         DtoNftProje row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.Durum = true;
         row.KullaniciId = 0;

         return row;
     }

     public IEnumerable<DtoNftProje> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoNftProje _model)
     {
         NftProje row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new NftProje() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqNftProje");
         }
         else
         {
             row = this.dataContext.NftProje.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.GizliId = _model.GizliId;
         row.Durum = _model.Durum;
         row.TarihSaat = _model.TarihSaat;
         row.Ad = _model.Ad;
         row.KullaniciId = _model.KullaniciId;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.NftProje.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.NftProje.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.NftProje.Remove(row);
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


