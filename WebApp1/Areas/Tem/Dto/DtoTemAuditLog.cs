using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemAuditLog : TemAuditLog
    {
        protected readonly DataContext dataContext;

        public string CcUserId
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   Areas.Tem.Codes.TemBusiness temBusiness = new(this.dataContext);
                   var queryResult = this.dataContext.TemKullanici.Where(c => c.Id == this.UserId)
                   .Select(s => new { value = s.Id, text = s.Ad })
                   .FirstOrDefault();

                   if (queryResult != null)
                   {
                       rV = queryResult.text;
                   }
               }
               catch { }
               return rV;
           }
        }
        public string CcOperationType
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   switch (this.OperationType)
                   {
                       case "C":
                           rV = MyApp.TranslateTo("xLng.Create", this.dataContext.Language);
                           break;
                       case "R":
                           rV = MyApp.TranslateTo("xLng.Read", this.dataContext.Language);
                           break;
                       case "U":
                           rV = MyApp.TranslateTo("xLng.Update", this.dataContext.Language);
                           break;
                       case "D":
                           rV = MyApp.TranslateTo("xLng.Delete", this.dataContext.Language);
                           break;
                   }
               }
               catch { }
               return rV;
           }
        }

        //Constructor
        public DtoTemAuditLog(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


