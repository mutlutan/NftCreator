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
 public class DmoTemRequestLog : BaseDmo
 {
     public DmoTemRequestLog(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemRequestLog> Get()
     {
         return this.dataContext.TemRequestLog.AsNoTracking()
             .Select(s => new DtoTemRequestLog(this.dataContext)
             {
                 Id = s.Id,
                 OperationDate = s.OperationDate,
                 OperationKod = s.OperationKod,
                 IpAddress = s.IpAddress,
                 SessionGuid = s.SessionGuid,
                 RequestJson = s.RequestJson,
                 ResponseJson = s.ResponseJson
             });
     }

     public DtoTemRequestLog GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemRequestLog row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;

         return row;
     }

     public IEnumerable<DtoTemRequestLog> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemRequestLog _model)
     {
         TemRequestLog row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemRequestLog() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemRequestLog");
         }
         else
         {
             row = this.dataContext.TemRequestLog.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.OperationDate = _model.OperationDate;
         row.OperationKod = _model.OperationKod;
         row.IpAddress = _model.IpAddress;
         row.SessionGuid = _model.SessionGuid;
         row.RequestJson = _model.RequestJson;
         row.ResponseJson = _model.ResponseJson;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemRequestLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemRequestLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemRequestLog.Remove(row);
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


