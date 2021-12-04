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
 public class DmoTemParametre : BaseDmo
 {
     public DmoTemParametre(DataContext dataContext) : base(dataContext) { }

     public IQueryable<DtoTemParametre> Get()
     {
         return this.dataContext.TemParametre.AsNoTracking()
             .Select(s => new DtoTemParametre(this.dataContext)
             {
                 Id = s.Id,
                 HostAddress = s.HostAddress,
                 LisansData = s.LisansData,
                 AuditLog = s.AuditLog,
                 AuditLogTables = s.AuditLogTables,
                 KurumEnlem = s.KurumEnlem,
                 KurumBoylam = s.KurumBoylam,
                 KurumAd = s.KurumAd,
                 KurumUnvan = s.KurumUnvan,
                 KurumAdes = s.KurumAdes,
                 KurumSemtSehir = s.KurumSemtSehir,
                 KurumMail = s.KurumMail,
                 KurumTelefon1 = s.KurumTelefon1,
                 KurumTelefon2 = s.KurumTelefon2,
                 KurumCep1 = s.KurumCep1,
                 KurumCep2 = s.KurumCep2,
                 KurumFax1 = s.KurumFax1,
                 KurumFax2 = s.KurumFax2,
                 MailHost = s.MailHost,
                 MailPort = s.MailPort,
                 MailEnableSsl = s.MailEnableSsl,
                 MailUserName = s.MailUserName,
                 MailPassword = s.MailPassword.MyFromBase64Str(),
                 DataBackupDurum = s.DataBackupDurum,
                 DataBackupSaat = s.DataBackupSaat,
                 DataBackupTip = s.DataBackupTip,
                 DataBackupGun = s.DataBackupGun,
                 DataBackupAyGunNo = s.DataBackupAyGunNo,
                 FileBackupDurum = s.FileBackupDurum,
                 FileBackupSaat = s.FileBackupSaat,
                 FileBackupTip = s.FileBackupTip,
                 FileBackupGun = s.FileBackupGun,
                 FileBackupAyGunNo = s.FileBackupAyGunNo
             });
     }

     public DtoTemParametre GetByNew()
     {
         //Default değerler ile bir row döner
         DtoTemParametre row = new(this.dataContext) { };
         // Burada field default değerleri veriliyor...
         row.Id = 0;
         row.AuditLog = false;
         row.MailPort = 0;
         row.MailEnableSsl = false;
         row.DataBackupDurum = false;
         row.DataBackupTip = 0;
         row.DataBackupAyGunNo = 0;
         row.FileBackupDurum = false;
         row.FileBackupTip = 0;
         row.FileBackupAyGunNo = 0;

         return row;
     }

     public IEnumerable<DtoTemParametre> GetById(int _id)
     {
         return this.Get().Where(c => c.Id == _id);
     }

     public int CreateOrUpdate(DtoTemParametre _model)
     {
         TemParametre row;
         Boolean isNew = _model.Id == 0;
         
         if (isNew)
         {
             //sadece insertte eklenip, update de değişmeyecek alanlar buraya
             row = new TemParametre() { };
             row.Id = (int)this.dataContext.GetNextSequenceValue("sqTemParametre");
         }
         else
         {
             row = this.dataContext.TemParametre.Where(c => c.Id == _model.Id).FirstOrDefault();
             if (row == null)
             {
                 throw new Exception(MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language));
             }
         }
         
         row.HostAddress = _model.HostAddress;
         row.LisansData = _model.LisansData;
         row.AuditLog = _model.AuditLog;
         row.AuditLogTables = _model.AuditLogTables;
         row.KurumEnlem = _model.KurumEnlem;
         row.KurumBoylam = _model.KurumBoylam;
         row.KurumAd = _model.KurumAd;
         row.KurumUnvan = _model.KurumUnvan;
         row.KurumAdes = _model.KurumAdes;
         row.KurumSemtSehir = _model.KurumSemtSehir;
         row.KurumMail = _model.KurumMail;
         row.KurumTelefon1 = _model.KurumTelefon1;
         row.KurumTelefon2 = _model.KurumTelefon2;
         row.KurumCep1 = _model.KurumCep1;
         row.KurumCep2 = _model.KurumCep2;
         row.KurumFax1 = _model.KurumFax1;
         row.KurumFax2 = _model.KurumFax2;
         row.MailHost = _model.MailHost;
         row.MailPort = _model.MailPort;
         row.MailEnableSsl = _model.MailEnableSsl;
         row.MailUserName = _model.MailUserName;
         row.MailPassword = _model.MailPassword.MyToBase64Str();
         row.DataBackupDurum = _model.DataBackupDurum;
         row.DataBackupSaat = _model.DataBackupSaat;
         row.DataBackupTip = _model.DataBackupTip;
         row.DataBackupGun = _model.DataBackupGun;
         row.DataBackupAyGunNo = _model.DataBackupAyGunNo;
         row.FileBackupDurum = _model.FileBackupDurum;
         row.FileBackupSaat = _model.FileBackupSaat;
         row.FileBackupTip = _model.FileBackupTip;
         row.FileBackupGun = _model.FileBackupGun;
         row.FileBackupAyGunNo = _model.FileBackupAyGunNo;

         if (!isNew)
         {
             //sadece update eklenip, insertte de değişmeyecek alanlar buraya
         }
         
         if (isNew)
         {
             this.dataContext.TemParametre.Add(row);
         }
         
         return row.Id;
     }

     public bool Delete(int _id)
     {
         Boolean rV = false;

         var row = this.dataContext.TemParametre.Where(c => c.Id == _id).FirstOrDefault();
         if (row != null)
         {
             this.dataContext.TemParametre.Remove(row);
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


