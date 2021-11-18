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
 public class DmoTemMailSablon : BaseDmo
 {
     public DmoTemMailSablon(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemMailSablon> Get()
     {
         return this.dataContext.TemMailSablon.AsNoTracking()
             .Select(s => new DtoTemMailSablon(this.dataContext)
             {
                 Id = s.Id,
                 AntetId = s.AntetId,
                 Kopya = s.Kopya,
                 Gizli = s.Gizli,
                 Alan = s.Alan,
                 Konu = s.Konu,
                 Icerik = s.Icerik,
                 CcAntetIdAd = s.Antet.Ad.MyToTrim()
             });
     }

     public DtoTemMailSablon GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemMailSablon row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.AntetId = 0;

         return row;
     }

     public IEnumerable<DtoTemMailSablon> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemMailSablon _model)
     {
         TemMailSablon row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemMailSablon() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemMailSablon");
         }
         else
         {
             row = this.dataContext.TemMailSablon.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.AntetId = _model.AntetId;
         row.Kopya = _model.Kopya;
         row.Gizli = _model.Gizli;
         row.Alan = _model.Alan;
         row.Konu = _model.Konu;
         row.Icerik = _model.Icerik;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemMailSablon.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemMailSablon.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemMailSablon.Remove(row);
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


