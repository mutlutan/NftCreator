using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApp1.Codes
{
    public class MyJobs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
        public static System.Threading.Timer MyTimer = null;

        private static readonly string baseBackupFolder = "C:\\ProgramData\\" + MyApp.AppName1 + MyApp.AppName2;
        private static readonly string dbBackupFolder = MyJobs.baseBackupFolder + "\\" + "BackupDatabase";
        private static readonly string fileBackupFolder = MyJobs.baseBackupFolder + "\\" + "BackupFile";

        #region Yardımcı fonksiyonlar
        public static Boolean FnTimeCheck(DateTime now, Boolean backupDurum, int backupTip, TimeSpan? backupSaat, string backupGun, int backupAyGunNo)
        {
            Boolean rV = false;
            try
            {
                if (backupDurum)
                {
                    Boolean tipOk = false;
                    Boolean saatOk = false;

                    if (backupTip == (int)EnmZamanTur.Aylik)
                    {
                        tipOk = backupAyGunNo == now.Day;
                    }

                    if (backupTip == (int)EnmZamanTur.Haftalik)
                    {
                        tipOk = backupGun.MyToLower().Split(',').Contains(now.DayOfWeek.ToString().MyToLower());
                    }

                    if (backupTip == (int)EnmZamanTur.Gunluk)
                    {
                        tipOk = true;
                    }

                    saatOk = now.Hour == backupSaat.Value.Hours && now.Minute == backupSaat.Value.Minutes;

                    if (tipOk && saatOk)
                    {
                        rV = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public static void FnDataBackup(Models.DataContext context, string dbName, string backupFileName)
        {
            try
            {
                // bu dizine "NT Service\MSSQLSERVER" veya "MSSQLSERVER" yetkisi vermen gerek, yada C:\ direk ver
                if (!string.IsNullOrEmpty(MyJobs.dbBackupFolder))
                {
                    if (!System.IO.Directory.Exists(MyJobs.dbBackupFolder))
                    {
                        MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $"({dbName}) (" + DateTime.Now.ToString() + "): " + "yedek dizini oluşturuluyor, " + MyJobs.dbBackupFolder);
                        System.IO.Directory.CreateDirectory(MyJobs.dbBackupFolder);
                    }
                }
                string backupFolder_backupFileName = MyJobs.dbBackupFolder + "\\" + backupFileName;
                string sqltext = $@"BACKUP DATABASE {dbName} TO DISK = '{backupFolder_backupFileName}' WITH FORMAT";

                MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $"({dbName}) (" + DateTime.Now.ToString() + "): " + "Yedek başlıyor, " + backupFolder_backupFileName);

                //context.GetDbConnection().Query(sqltext);
                int timeOutSn = 60 * 10; //10Dk.
                context.GetDbConnection().Execute(sqltext, null, null, timeOutSn);

                MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $"{dbName} (" + DateTime.Now.ToString() + "): " + "Yedek alındı, " + backupFolder_backupFileName);

            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        public static void FnFileBackup()
        {
            try
            {

                if (!string.IsNullOrEmpty(MyJobs.fileBackupFolder))
                {
                    if (!System.IO.Directory.Exists(MyJobs.fileBackupFolder))
                    {
                        System.IO.Directory.CreateDirectory(MyJobs.fileBackupFolder);
                    }
                }

                // file backup başlar
                string fileFolder = Codes.MyApp.EnvWebRootPath + "\\" + Codes.MyApp.AppDataDirectory;
                System.IO.DirectoryInfo directoryInfo = new(fileFolder);
                if (directoryInfo.Exists)
                {
                    string searchPatern = "*.*";

                    System.IO.FileInfo[] files = directoryInfo
                        .EnumerateFiles(searchPatern, System.IO.SearchOption.AllDirectories)
                        .OrderBy(p => p.CreationTime).ToArray();

                    int sayac = 0;
                    foreach (System.IO.FileInfo file in files)
                    {
                        string targetDirectory = file.DirectoryName.Replace(fileFolder, MyJobs.fileBackupFolder);
                        if (!System.IO.Directory.Exists(targetDirectory))
                        {
                            System.IO.Directory.CreateDirectory(targetDirectory);
                        }

                        string targetFileName = file.FullName.Replace(fileFolder, MyJobs.fileBackupFolder);
                        if (!System.IO.File.Exists(targetFileName))
                        {
                            file.CopyTo(targetFileName, true);
                            sayac++;
                        }
                        else
                        {
                            System.IO.FileInfo targetFile = new(targetFileName);
                            if (file.LastWriteTime != targetFile.LastWriteTime || file.Attributes != targetFile.Attributes)
                            {
                                file.CopyTo(targetFileName, true);
                                sayac++;
                            }
                        }
                    }

                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, " (" + DateTime.Now.ToString() + "): " + "Yedek alındı, dosya sayısı : " + sayac.ToString() + " adet");
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        #endregion

        #region görevler

        public static async Task Gorev_LocalWebRequest(string kurulusKod, Models.DataContext context, DateTime now)
        {
            //Bu işlem ile application stop olmasını engellemek için doğal bir yöntem olarak kullanılabilir.
            try
            {
                if ((now.Minute == 0) || (now.Minute == 15) || (now.Minute == 30) || (now.Minute == 45))
                {
                    var parametre = context.TemParametre.FirstOrDefault();
                    using var client = new System.Net.Http.HttpClient();

                    string sHost = parametre.HostAddress;
                    client.BaseAddress = new Uri(sHost);
                    var response = await client.GetAsync("");
                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $"({kurulusKod}) ({DateTime.Now}): {sHost}");

                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        public static void Gorev_DatabaseBackup(Models.DataContext context, DateTime now)
        {
            try
            {
                // veritabanı yedeklemesi buradan
                MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, $" TimeCheck...");
                var parametre = context.TemParametre.FirstOrDefault();
                var sonucTimeCheck = MyJobs.FnTimeCheck(now, parametre.DataBackupDurum, parametre.DataBackupTip, parametre.DataBackupSaat, parametre.DataBackupGun, parametre.DataBackupAyGunNo);

                if (sonucTimeCheck)
                {
                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, $" TimeCheck Ok.");
                    string dbName = context.GetDbConnection().Database;
                    string backupFileName = dbName + "_" + now.DayOfWeek.ToString() + ".bak";
                    MyJobs.FnDataBackup(context, dbName, backupFileName);
                }
                else
                {
                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, $" TimeCheck..." + parametre.DataBackupSaat + "" + now);
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        public static void Gorev_FileBackup(Models.DataContext context, DateTime now)
        {
            try
            {
                // wwwroot/data altındakilerin yedeklemesi buradan
                MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, $" TimeCheck...");
                var parametre = context.TemParametre.FirstOrDefault();
                var sonucTimeCheck = MyJobs.FnTimeCheck(now, parametre.FileBackupDurum, parametre.FileBackupTip, parametre.FileBackupSaat, parametre.FileBackupGun, parametre.FileBackupAyGunNo);

                if (sonucTimeCheck)
                {
                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, $" TimeCheck Ok.");
                    MyJobs.FnFileBackup();
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        public static void Gorev_DovizGuncelle(Models.DataContext context)
        {
            try
            {
                MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $" Updateable Durum Check...");
                if (context.TemParaBirim.Where(c => c.Updateable && c.Durum).Any())
                {
                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $" Updateable Durum Check Ok.");
                    System.Xml.Linq.XElement kurlar = System.Xml.Linq.XElement.Load("https://www.tcmb.gov.tr/kurlar/today.xml");
                    foreach (var kur in kurlar.Elements())
                    {
                        try
                        {
                            string kod = kur.Attribute("CurrencyCode").Value;
                            var paraBirim = context.TemParaBirim.Where(c => c.Updateable && c.Durum && c.Kod == kod).FirstOrDefault();
                            if (paraBirim != null)
                            {
                                string currencyDecimalSeparator = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;

                                string forexBuying = kur.Element("ForexBuying").Value;
                                string forexSelling = kur.Element("ForexSelling").Value;
                                string banknoteBuying = kur.Element("BanknoteBuying").Value;
                                string banknoteSelling = kur.Element("BanknoteSelling").Value;

                                if (banknoteBuying.Length == 0)
                                {
                                    banknoteBuying = "0";
                                }

                                if (banknoteSelling.Length == 0)
                                {
                                    banknoteSelling = "0";
                                }

                                decimal _DovizAlis = Convert.ToDecimal(forexBuying.Replace(".", currencyDecimalSeparator));
                                decimal _DovizSatis = Convert.ToDecimal(forexSelling.Replace(".", currencyDecimalSeparator));
                                decimal _EfektifAlis = Convert.ToDecimal(banknoteBuying.Replace(".", currencyDecimalSeparator));
                                decimal _EfektifSatis = Convert.ToDecimal(banknoteSelling.Replace(".", currencyDecimalSeparator));

                                // Arşiv kadını set ediyoruz
                                var dovizKurArsiv = context.TemDovizKurArsiv
                                    .Where(c => c.ParaBirimId == paraBirim.Id && c.Tarih == DateTime.Today)
                                    .FirstOrDefault();

                                if (dovizKurArsiv == null)
                                {
                                    Models.TemDovizKurArsiv modelDovizKurArsiv = new() { };
                                    modelDovizKurArsiv.Id = (int)context.GetNextSequenceValue("sqTemDovizKurArsiv");
                                    modelDovizKurArsiv.ParaBirimId = paraBirim.Id;
                                    modelDovizKurArsiv.Tarih = DateTime.Today;
                                    modelDovizKurArsiv.TarihSaat = DateTime.Now;
                                    modelDovizKurArsiv.DovizAlis = _DovizAlis;
                                    modelDovizKurArsiv.DovizSatis = _DovizSatis;
                                    modelDovizKurArsiv.EfektifAlis = _EfektifAlis;
                                    modelDovizKurArsiv.EfektifSatis = _EfektifSatis;
                                    context.Add(modelDovizKurArsiv);

                                    paraBirim.UpdateDate = DateTime.Now;

                                    context.SaveChanges();
                                    MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $" day insert Ok.");
                                }
                                else
                                {
                                    if (dovizKurArsiv.DovizAlis != _DovizAlis || dovizKurArsiv.DovizSatis != _DovizSatis || dovizKurArsiv.EfektifAlis != _EfektifAlis || dovizKurArsiv.EfektifSatis != _EfektifSatis)
                                    {
                                        dovizKurArsiv.TarihSaat = DateTime.Now;
                                        dovizKurArsiv.DovizAlis = _DovizAlis;
                                        dovizKurArsiv.DovizSatis = _DovizSatis;
                                        dovizKurArsiv.EfektifAlis = _EfektifAlis;
                                        dovizKurArsiv.EfektifSatis = _EfektifSatis;

                                        paraBirim.UpdateDate = DateTime.Now;

                                        context.SaveChanges();
                                        MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $" day update Ok.");
                                    }
                                    else
                                    {
                                        MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Uyari, $" ({kod})kur değişmedi.");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }
        }

        #endregion

        #region görevler start
        public static void Gorevler(object sender)
        {
            MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, Environment.NewLine + "Gorevler check => " + DateTime.Now.ToString());

            DateTime now = DateTime.Now;
            MyConfigs cfg = (MyConfigs)sender;

            using Models.DataContext context = new(new DbContextOptions<Models.DataContext>());
            context.SetConnectionString(cfg, new System.Globalization.CultureInfo("en-US"));
            //görev fonksiyonları çağrılıyor
            //Gorev_LocalWebRequest(kurulusKod, context, now).Wait();
            Gorev_DatabaseBackup(context, now);
            Gorev_FileBackup(context, now);

            //saatte bir içeriyi uygular
            if (now.Minute == 0)
            {
                Gorev_DovizGuncelle(context);

                try
                {
                    SqlConnection.ClearAllPools();
                }
                catch { }

                try
                {
                    //GC.Collect(); sesion dan kurtulmadan açma sesiona bir tokat atıyor
                }
                catch { }
            }
        }
        #endregion
    }
}
