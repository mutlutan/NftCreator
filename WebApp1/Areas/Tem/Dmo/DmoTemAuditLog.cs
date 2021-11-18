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
 public class DmoTemAuditLog : BaseDmo
 {
     public DmoTemAuditLog(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemAuditLog> Get()
     {
         return this.dataContext.TemAuditLog.AsNoTracking()
             .Select(s => new DtoTemAuditLog(this.dataContext)
             {
                 Id = s.Id,
                 OperationDate = s.OperationDate,
                 UserId = s.UserId,
                 IpAddress = s.IpAddress,
                 OperationType = s.OperationType,
                 TableName = s.TableName,
                 PrimaryKeyField = s.PrimaryKeyField,
                 PrimaryKeyValue = s.PrimaryKeyValue,
                 CurrentValues = s.CurrentValues,
                 OriginalValues = s.OriginalValues
             });
     }

     public DtoTemAuditLog GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemAuditLog row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.UserId = 0;

         return row;
     }

     public IEnumerable<DtoTemAuditLog> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemAuditLog _model)
     {
         TemAuditLog row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemAuditLog() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemAuditLog");
         }
         else
         {
             row = this.dataContext.TemAuditLog.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.OperationDate = _model.OperationDate;
         row.UserId = _model.UserId;
         row.IpAddress = _model.IpAddress;
         row.OperationType = _model.OperationType;
         row.TableName = _model.TableName;
         row.PrimaryKeyField = _model.PrimaryKeyField;
         row.PrimaryKeyValue = _model.PrimaryKeyValue;
         row.CurrentValues = _model.CurrentValues;
         row.OriginalValues = _model.OriginalValues;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemAuditLog.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemAuditLog.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemAuditLog.Remove(row);
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


