using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemParametre : TemParametre
    {
        protected readonly DataContext dataContext;

        public string CcAuditLogTables
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.AuditLogTables.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcDataBackupTip
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   switch (this.DataBackupTip)
                   {
                       case 0:
                           rV = MyApp.TranslateTo("xLng.Gunluk", this.dataContext.Language);
                           break;
                       case 1:
                           rV = MyApp.TranslateTo("xLng.Haftalik", this.dataContext.Language);
                           break;
                       case 2:
                           rV = MyApp.TranslateTo("xLng.Aylik", this.dataContext.Language);
                           break;
                   }
               }
               catch { }
               return rV;
           }
        }
        public string CcDataBackupGun
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.DataBackupGun.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcDataBackupAyGunNo
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.DataBackupAyGunNo.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcFileBackupTip
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   switch (this.FileBackupTip)
                   {
                       case 0:
                           rV = MyApp.TranslateTo("xLng.Gunluk", this.dataContext.Language);
                           break;
                       case 1:
                           rV = MyApp.TranslateTo("xLng.Haftalik", this.dataContext.Language);
                           break;
                       case 2:
                           rV = MyApp.TranslateTo("xLng.Aylik", this.dataContext.Language);
                           break;
                   }
               }
               catch { }
               return rV;
           }
        }
        public string CcFileBackupGun
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.FileBackupGun.MyToTrim();
               }
               catch { }
               return rV;
           }
        }
        public string CcFileBackupAyGunNo
        {
           get
           {
               string rV = string.Empty;
               try
               {
                   rV = this.FileBackupAyGunNo.MyToTrim();
               }
               catch { }
               return rV;
           }
        }

        //Constructor
        public DtoTemParametre(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


