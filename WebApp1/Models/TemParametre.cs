using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemParametre
    {
        public int Id { get; set; }
        public bool UniqueVisit { get; set; }
        public string HostAddress { get; set; }
        public string LisansData { get; set; }
        public bool AuditLog { get; set; }
        public string AuditLogTables { get; set; }
        public string KurumEnlem { get; set; }
        public string KurumBoylam { get; set; }
        public string KurumAd { get; set; }
        public string KurumUnvan { get; set; }
        public string KurumAdes { get; set; }
        public string KurumSemtSehir { get; set; }
        public string KurumMail { get; set; }
        public string KurumTelefon1 { get; set; }
        public string KurumTelefon2 { get; set; }
        public string KurumCep1 { get; set; }
        public string KurumCep2 { get; set; }
        public string KurumFax1 { get; set; }
        public string KurumFax2 { get; set; }
        public string MailHost { get; set; }
        public int MailPort { get; set; }
        public bool MailEnableSsl { get; set; }
        public string MailUserName { get; set; }
        public string MailPassword { get; set; }
        public bool DataBackupDurum { get; set; }
        public TimeSpan? DataBackupSaat { get; set; }
        public int DataBackupTip { get; set; }
        public string DataBackupGun { get; set; }
        public int DataBackupAyGunNo { get; set; }
        public bool FileBackupDurum { get; set; }
        public TimeSpan? FileBackupSaat { get; set; }
        public int FileBackupTip { get; set; }
        public string FileBackupGun { get; set; }
        public int FileBackupAyGunNo { get; set; }
        public string IyzicoBaseUrl { get; set; }
        public string IyzicoApiKey { get; set; }
        public string IyzicoSecretKey { get; set; }
    }
}
