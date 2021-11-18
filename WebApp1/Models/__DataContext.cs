using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApp1.Codes;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Text;
using Dapper;

namespace WebApp1.Models
{
    //PM> run
    //Scaffold-DbContext "Server=.;Database=nft_creator;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -outputDir Models -context DataContext -Force -NoPluralize -NoOnConfiguring -Verbose

    public partial class DataContext : DbContext
    {
        //public _DataContext(DbContextOptions<_DataContext> options) : base(options) { }

        public MyConfigs ConStrings { get; set; }
        public CultureInfo Culture { get; set; } = new CultureInfo("tr-TR");
        public string KurulusKod { get; set; } = "";
        public int UserId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string IPAddress { get; set; } = "";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=robotik_db_serce;Trusted_Connection=True;");
            }
        }

        //----------------------------------------------------------------------------------
        public string Language
        {
            get
            {
                return this.Culture.TwoLetterISOLanguageName; //"tr","en" ... vb.
            }
        }

        public void SetConnectionString(MyConfigs _myConfigs, CultureInfo _cultureInfo = null)
        {
            this.ConStrings = _myConfigs;
            this.Culture = _cultureInfo;

            var csb = new SqlConnectionStringBuilder(this.ConStrings.DefaultConnectionString) { };
            csb.ApplicationName = MyApp.AppName;
            if (_cultureInfo != null)
            {
                csb.CurrentLanguage = _cultureInfo.Parent.EnglishName;
            }

            csb.MultipleActiveResultSets = true;

            csb.ApplicationName = MyApp.AppName;


            this.Database.GetDbConnection().ConnectionString = csb.ToString();
        }

        public int GetNextSequenceValue(string _SequenceName)
        {
            int rV = 0;
            var connection = this.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT NEXT VALUE FOR " + _SequenceName;
            //command.CommandText = "SELECT " + _SequenceName + ".NEXTVAL FROM DUAL"; //Oracle

            try
            {
                connection.Open();
                rV = Convert.ToInt32(command.ExecuteScalar());
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return rV;
        }

        public DbConnection GetDbConnection()
        {
            return this.Database.GetDbConnection();
        }

        #region Shema bilgileri
        public DatabaseSchemaReader.DataSchema.DatabaseSchema GetDBSchema()
        {
            using var dbReader = new DatabaseSchemaReader.DatabaseReader(this.Database.GetDbConnection());
            return dbReader.ReadAll();
        }

        public DatabaseSchemaReader.DataSchema.DatabaseTable GetTableInfo(string _sTableName)
        {
            return this.GetDBSchema().Tables.Where(c => c.Name == _sTableName).FirstOrDefault();
        }

        #endregion

        #region Database Create -update - RunScript
        public StringBuilder CreateDatabase()
        {
            var LogList = new StringBuilder();
            DateTime startTime = DateTime.Now;

            string _sInitialCatalog = this.Database.GetDbConnection().Database;
            string _sCollate = "TURKISH_CI_AS"; //burası seçilebilir birşey olmalı
            string _SQLText = "CREATE DATABASE " + _sInitialCatalog + " COLLATE " + _sCollate + ";";

            using (var Con1 = new SqlConnection(this.ConStrings.MasterConnectionString))
            {
                Con1.Open();
                using (var cmd = new SqlCommand(_SQLText, Con1))
                {
                    cmd.ExecuteNonQuery();
                    LogList.AppendLine("Veritabanı dosyası " + _sCollate + " olarak oluşturuldu. " + (DateTime.Now - startTime).TotalSeconds.ToString("N2") + " sn");
                }
                Con1.Close();
            }

            return LogList;
        }

        public StringBuilder RunScript(string _Separators, string _Script)
        {
            var LogList = new StringBuilder();

            using (var Con1 = new SqlConnection(this.ConStrings.DefaultConnectionString))
            {
                Con1.Open();
                foreach (string s in _Script.Split(new string[] { _Separators }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string sqlText = s;

                    if (!string.IsNullOrEmpty(sqlText))
                    {
                        if (sqlText.Trim().Length > 5)
                        {
                            using var cmd = new SqlCommand(sqlText, Con1);
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Script:(" + sqlText + ")\n" + ex.MyLastInner().Message);
                            }
                        }
                    }
                }
                Con1.Close();
            }

            return LogList;
        }

        public StringBuilder CreateTables()
        {
            var LogList = new System.Text.StringBuilder();

            // temel modül script
            if (MyApp.Configs.ActiveAreas.Split(",").Where(c => c == "Tem").Any())
            {
                LogList.Append(this.RunScript(";", MyApp.GetSqlScript("111_Tem_Create.sql")).ToString() + "Temel table create iş sonu. \n");
                LogList.Append(this.RunScript(";", MyApp.GetSqlScript("112_Tem_Insert.sql")).ToString() + "Temel table insert iş sonu. \n");
            }

            // Robotik modül script
            if (MyApp.Configs.ActiveAreas.Split(",").Where(c => c == "Nft").Any())
            {
                LogList.Append(this.RunScript(";", MyApp.GetSqlScript("211_Nft_Create.sql")).ToString() + "Nft table create iş sonu. \n");
                LogList.Append(this.RunScript(";", MyApp.GetSqlScript("212_Nft_Insert.sql")).ToString() + "Nft table insert iş sonu. \n");
            }

            return LogList;
        }

        public string RunDatabaseUpdate()
        {
            string sonucMesaj = "";
            string lastCommand = "";

            try
            {
                int versiyonId = 0;
                var versiyon = this.TemVersiyon.AsNoTracking().OrderBy(o => o.Id).LastOrDefault();
                if (versiyon != null)
                {
                    versiyonId = versiyon.Id;
                }

                // db update
                var dbConnection = this.GetDbConnection();
                foreach (var item in MyDatabaseUpdate.GetDatabaseUpdateList(versiyonId))
                {
                    // sql ler execute edilecek
                    sonucMesaj += "Id : " + item.Id + ", Description : " + item.Aciklama + Environment.NewLine;
                    lastCommand = item.Komut;
                    string commandText = $@"
                            BEGIN TRY
                                begin tran
                                      {item.Komut}     
                                      Insert TemVersiyon(Id, Tarih, Aciklama) Values({item.Id}, getdate(), '{item.Aciklama}');
                                commit tran
                            END TRY
                            BEGIN CATCH
                                IF @@TRANCOUNT > 0
                                    ROLLBACK TRAN

                                    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
                                    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
                                    DECLARE @ErrorState INT = ERROR_STATE()

                                -- Use RAISERROR inside the CATCH block to return error  
                                -- information about the original error that caused  
                                -- execution to jump to the CATCH block.  
                                RAISERROR (@ErrorMessage, -- Message text.  
                                           @ErrorSeverity, -- Severity.  
                                           @ErrorState -- State.  
                                          );
                            END CATCH 
                        ";

                    dbConnection.Query(commandText);

                    var qVersiyon = this.TemVersiyon.Where(c => c.Id == item.Id).FirstOrDefault();
                    qVersiyon.Komut = item.Komut;
                    this.SaveChanges();
                }

                sonucMesaj += Environment.NewLine + "İş sonu versiyon : " + this.TemVersiyon.AsNoTracking().OrderBy(o => o.Id).LastOrDefault().Id;
            }
            catch (Exception ex)
            {
                sonucMesaj += Environment.NewLine + lastCommand;
                sonucMesaj += Environment.NewLine + ex.Message;
            }

            return sonucMesaj;
        }

        #endregion


        #region Audit Logs
        private void BeforeSaveChanges(string[] _AuditTableList)
        {
            //Hangi tabloların, hangi hareketlerini tutacağına karar verildiği yer
            var changeTrack = this.ChangeTracker.Entries()
                .Where(c => c.State == EntityState.Added | c.State == EntityState.Modified | c.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changeTrack)
            {
                string tableName = entry.Entity.GetType().Name;
                Boolean logKaydet = _AuditTableList.Where(c => c.Contains(tableName)).Any();

                if (logKaydet)
                {
                    var newTemAuditLog = new TemAuditLog() { };
                    newTemAuditLog.Id = this.GetNextSequenceValue("sqTemAuditLog");
                    newTemAuditLog.OperationDate = DateTime.Now;
                    newTemAuditLog.UserId = this.UserId;
                    newTemAuditLog.IpAddress = this.IPAddress;
                    newTemAuditLog.TableName = tableName;
                    newTemAuditLog.PrimaryKeyField = entry.Properties.Where(c => c.Metadata.IsPrimaryKey() == true).FirstOrDefault().Metadata.Name;


                    if (entry.State == EntityState.Added)
                    {
                        newTemAuditLog.OperationType = "C";
                        newTemAuditLog.PrimaryKeyValue = Convert.ToString(entry.CurrentValues[newTemAuditLog.PrimaryKeyField]);
                        newTemAuditLog.CurrentValues = Newtonsoft.Json.JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        newTemAuditLog.OperationType = "U";
                        newTemAuditLog.PrimaryKeyValue = Convert.ToString(entry.OriginalValues[newTemAuditLog.PrimaryKeyField]);
                        newTemAuditLog.CurrentValues = Newtonsoft.Json.JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                        newTemAuditLog.OriginalValues = Newtonsoft.Json.JsonConvert.SerializeObject(entry.OriginalValues.ToObject());
                    }
                    if (entry.State == EntityState.Deleted)
                    {
                        newTemAuditLog.OperationType = "D";
                        newTemAuditLog.PrimaryKeyValue = Convert.ToString(entry.OriginalValues[newTemAuditLog.PrimaryKeyField]);
                        newTemAuditLog.OriginalValues = Newtonsoft.Json.JsonConvert.SerializeObject(entry.OriginalValues.ToObject());
                    }

                    this.TemAuditLog.Add(newTemAuditLog);
                }
            }
        }

        public override int SaveChanges()
        {
            var parametre = this.TemParametre.FirstOrDefault();
            if (parametre != null)
            {
                if (parametre.AuditLog == true && parametre.AuditLogTables != null && parametre.AuditLogTables.Length > 0)
                {
                    this.BeforeSaveChanges(parametre.AuditLogTables.Split(','));
                }
            }

            return base.SaveChanges(true);
        }
        #endregion
    }

}
