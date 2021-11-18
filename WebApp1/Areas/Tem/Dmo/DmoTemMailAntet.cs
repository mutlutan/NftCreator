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
 public class DmoTemMailAntet : BaseDmo
 {
     public DmoTemMailAntet(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemMailAntet> Get()
     {
         return this.dataContext.TemMailAntet.AsNoTracking()
             .Select(s => new DtoTemMailAntet(this.dataContext)
             {
                 Id = s.Id,
                 Ad = s.Ad,
                 Icerik = s.Icerik
             });
     }

     public DtoTemMailAntet GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemMailAntet row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;

         return row;
     }

     public IEnumerable<DtoTemMailAntet> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemMailAntet _model)
     {
         TemMailAntet row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemMailAntet() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemMailAntet");
         }
         else
         {
             row = this.dataContext.TemMailAntet.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.Ad = _model.Ad;
         row.Icerik = _model.Icerik;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemMailAntet.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemMailAntet.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemMailAntet.Remove(row);
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


