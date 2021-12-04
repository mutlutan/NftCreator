using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebApp1.Codes
{
    #region models enums

    #region settings model
    public class MyConfigs
    {
        /*Diger ayarlar*/
        public string SupportedCultures { get; set; }
        public string ActiveAreas { get; set; }
        /*db settings*/
        public string MasterConnectionString { get; set; } // db sunucusu master db ye connect olması için
        public string DefaultConnectionString { get; set; }

        /*internal tokenlar*/
        public string UpdateToken { get; set; }
        public string TemToken { get; set; }
        public string RobToken { get; set; }
    }
    #endregion

    #region Enums
    public enum EnmLogTur
    {
        Hata = 11,
        Genel = 12,
        Istek = 13,
        Middleware = 14,
        Uyari = 15
    }
    public enum EnmMailSablon
    {
        SifreBildirim = 101,
        UyeOnayBildirim = 102,
        IletisimBildirim = 201,
        EtkinlikKayit = 301,
    }

    public enum EnmYetkiGrup
    {
        Admin = 11,
        Personel = 21,
        Musteri = 31
    }
    public enum EnmRol
    {
        Admin = 1001,
        Personel = 1101
    }

    public enum EnmZamanTur
    {
        Gunluk = 0,
        Haftalik = 1,
        Aylik = 2
    }

    #endregion

    #region token models
    public class MoCaptchaToken
    {
        public String Code { get; set; }
    }

    public class MoUserToken
    {
        public string OturumGuid { get; set; } = Guid.NewGuid().ToString();
        public string Culture { get; set; } = "en-US";
        public string Host { get; set; }
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int LisansGun { get; set; }
        public string NameSurname { get; set; } = "";
        public Boolean UserIsLogon { get; set; } = false;
        public Boolean UserIsGuest { get; set; } = false;
        public string UserRols { get; set; } = "";

        public EnmYetkiGrup YetkiGrup { get; set; }
    }
    #endregion

    #region api models

    public class MoRequestDovizKuruGetir
    {
        public string ParaBirimKod { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public int Gun { get; set; }
    }

    public class MoTokenRequest
    {
        public string Culture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CaptchaCode { get; set; }
        public string CaptchaToken { get; set; }
    }

    public class MoProjeHavuzuRequest
    {
        public string Where { get; set; }
        public string OrderBy { get; set; }
    }

    #endregion

    #region Temel modleller

    public class MoResponse<T> where T : class, new()
    {
        public Boolean Success { get; set; } = false;
        public List<string> Message { get; set; } = new List<string>();
        public T Data { get; set; }
    }

    public class MoCreateCaptchaResponse
    {
        public string CaptchaImage { get; set; }
        public string CaptchaToken { get; set; }
    }

    public class MoLogin
    {
        public string Culture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IPAdres { get; set; }
        public string Tarayici { get; set; }
    }

    public class MoUpdateQuery
    {
        public int Id { get; set; }
        public string Aciklama { get; set; }
        public string Komut { get; set; }
    }
    #endregion

    #region DashModel
    public enum EnmDashItem
    {
        KullaniciSayisi = 111,
        ProjeSayisi = 121,
    }

    public class MyDashItem
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string IconClass { get; set; }
        public string IconStyle { get; set; }
        public string Title { get; set; }
        public string RefreshTables { get; set; } // hangi tablolar değiştiğinde resfsh olacağını belirler
        public string YetkiGrups { get; set; } = "10";
        public string Url { get; set; }
    }
    public class MyDashData
    {
        public string Text { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public string TextCssClass { get; set; }
        public string ValueCssClass { get; set; }
    }
    #endregion

    #region Lookup Object

    public class LookupFilters
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    public class LookupSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }

    public class LookupRequest
    {
        public string TableName { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string OtherFields { get; set; }
        public IList<LookupFilters> Filters { get; set; }
        public IList<LookupSort> Sorts { get; set; }
    }

    #endregion

    #region api request modeller
    public class ApiFilters
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
    }

    public class ApiSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
    public class ApiRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IList<ApiFilters> Filters { get; set; }
        public IList<ApiSort> Sorts { get; set; }
    }
    #endregion

    #region Gorseller model

    public class MyFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FileUrl { get; set; }
        public string FileViewUrl { get; set; }
        public string FileVersion { get; set; }
        public long Size { get; set; }
        public string SizeText { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    #endregion

    #endregion

    #region MyApp

    public class MyApp
    {
        public static DateTime ApplicationStartedDateTime { get; set; } = DateTime.Now;

        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment Env { get; set; }

        public static string EnvContentRootPath { get; set; }
        public static string EnvWebRootPath { get; set; }


        #region app Version
        public static string AppName1 { get; set; } = "Nft";
        public static string AppName2 { get; set; } = "Creator";
        public static string AppName
        {
            get
            {
                string rS = MyApp.AppName1;
                if (!string.IsNullOrEmpty(MyApp.AppName2))
                {
                    rS += " " + MyApp.AppName2;
                }
                return rS;
            }
        }

        public static string Version
        {
            get
            {
                string Version = "1.0.0.3";
                if (Env.EnvironmentName == "Development")
                {
                    Version += "_";
                    Version += DateTime.Now.ToString("MMddHHmmss");
                }
                return Version;
            }
        }
        #endregion

        #region file versiyon
        public static string FilePathToVersion(string filePath)
        {
            string rV = "";

            try
            {
                //1.yöntem, yavaşlatacaktır biraz
                string filePathFull = MyApp.EnvWebRootPath + "\\" + filePath;
                if (System.IO.File.Exists(filePathFull))
                {
                    var fileInfo = new System.IO.FileInfo(filePathFull);
                    rV = "?v." + fileInfo.LastWriteTime.Ticks.ToString();
                }

                //2.yöntem, hızlı yöntem
                //rV = "?v." + MyApp.Version;
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public static string GetZorlukToStarHtml(int zorlukId)
        {
            var sb = new StringBuilder();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    string clss = "text-muted";
                    if ((zorlukId / 10) >= i)
                    {
                        clss = "text-warning";
                    }
                    sb.Append("<span class='fa fa-star " + clss + "'></span>");
                }
            }
            catch
            { }

            return sb.ToString();
        }
        #endregion

        #region comps
        public static List<string> GetComps()
        {
            var fileList = new List<string>();
            List<string> areaList = MyApp.Areas;
            areaList.Add("_");


            foreach (string areaName in areaList)
            {
                var dir = new System.IO.DirectoryInfo(MyApp.Env.WebRootPath + "\\" + MyApp.AppAreasDirectory + "\\" + areaName + "\\" + MyApp.AppCompsDirectory);
                if (dir.Exists)
                {
                    foreach (var file in dir.EnumerateFiles("*.js"))
                    {
                        //*.js url
                        var s = file.FullName.Replace(MyApp.Env.WebRootPath, "");
                        fileList.Add(s);
                    }
                }
            }

            return fileList;
        }
        #endregion

        #region token işlemleri
        public static string JWTKey
        {
            get
            {
                string key = "NFT-JWT-KEY-00000000000000001" + "-" + "v.01"; //JWT key;
                return key;
            }
        }

        #region JWT Token(Captcha Token; captcha verisi taşımada kullanılır, şifreli olmalıdır  )
        public static string GenerateCaptchaToken(MoCaptchaToken moCaptchaToken)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(MyApp.JWTKey));

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(moCaptchaToken);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: new System.Security.Claims.Claim[]{
                    new System.Security.Claims.Claim("Data", jsonText.MyToEncrypt(MyApp.JWTKey))
                },
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512),
                expires: DateTime.Now.AddMinutes(2)
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        public static Boolean ValidateCaptchaToken(string captchaCode, string captchaToken)
        {
            Boolean rV = false;
            try
            {
                var moCaptchaToken = new MoCaptchaToken();
                string authToken = captchaToken.MyToTrim().Trim().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(authToken))
                {
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false, // oluşturulan jetonda izleyici yok
                        ValidateIssuer = false,   // oluşturulan jetonda bir yayıncı yok
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MyApp.JWTKey)) // The same key as the one that generate the token
                    };

                    System.Security.Principal.IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);

                    var data = ((System.Security.Claims.ClaimsIdentity)principal.Identity).Claims.Select(s => new { s.Type, s.Value });
                    string jsonText = data.Where(c => c.Type == "Data").FirstOrDefault().Value;
                    moCaptchaToken = Newtonsoft.Json.JsonConvert.DeserializeObject<MoCaptchaToken>(jsonText.MyToDecrypt(MyApp.JWTKey));

                    rV = (moCaptchaToken.Code == captchaCode);
                }
            }
            catch { }

            return rV;
        }
        #endregion

        #region JWT Token(User Token; api için login olunduğunda oluşur )
        public static string GenerateUserToken(MoUserToken moUserToken)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(MyApp.JWTKey));

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(moUserToken);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: new System.Security.Claims.Claim[]{
                    new System.Security.Claims.Claim("User", jsonText.MyToBase64Str())
                },
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512),
                expires: DateTime.Now.AddDays(1)
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        public static MoUserToken ValidateUserToken(Models.DataContext dataContext, string token)
        {
            var moUserToken = new MoUserToken();
            try
            {
                string authToken = token.MyToTrim().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(authToken))
                {
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false, // oluşturulan jetonda izleyici yok
                        ValidateIssuer = false,   // oluşturulan jetonda bir yayıncı yok
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MyApp.JWTKey)) // The same key as the one that generate the token
                    };

                    System.Security.Principal.IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);

                    var data = ((System.Security.Claims.ClaimsIdentity)principal.Identity).Claims.Select(s => new { s.Type, s.Value });
                    string jsonText = data.Where(c => c.Type == "User").FirstOrDefault().Value.MyFromBase64Str();
                    moUserToken = Newtonsoft.Json.JsonConvert.DeserializeObject<MoUserToken>(jsonText);

                    //tekil giriş kontrolü sağlanıyor, biraz yavaşlatabilir
                    if (!moUserToken.UserIsGuest)
                    {
                        var user = dataContext.TemOturumLog
                            .Where(c => c.KullaniciId == moUserToken.UserId)
                            .OrderByDescending(o => o.Id).FirstOrDefault();

                        if (user == null)
                        {
                            moUserToken.UserIsLogon = false;
                        }
                        else
                        {
                            moUserToken.UserIsLogon = moUserToken.OturumGuid == user.OturumGuid;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                moUserToken = new MoUserToken();
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return moUserToken;
        }
        #endregion


        #endregion

        #region config

        public static IConfigurationRoot ConfigurationRoot { get; set; }
        public static MyConfigs Configs
        {
            get
            {
                return ConfigurationRoot.GetSection("MyConfigs").Get<MyConfigs>();
            }
        }

        #endregion

        #region logs
        static readonly object LogKilit = new();

        private static void LogSaveForDb(/*EnmLogTur logTur, string icerik*/)
        {
            string logDirectory = "logs";
            string strGun = DateTime.Today.ToString("yyyy.MM.dd");

            try
            {
                //using Models.DataContext dataContext = new Models.DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Models.DataContext>());
                //dataContext.SetConnectionString(MyApp.Configs, null);

                //var row = new Models.TemSistemLog() { };
                //row.Id = (int)dataContext.GetNextSequenceValue("sqTemSistemLog");
                //row.TarihSaat = DateTime.Now;
                //row.LogTurId = (int)logTur;
                //row.Icerik = icerik;
                //dataContext.TemSistemLog.Add(row);
                //dataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                try
                {
                    lock (LogKilit)
                    {
                        System.IO.StreamWriter dosya = System.IO.File.AppendText(logDirectory + "/log_err_" + strGun + ".txt");
                        dosya.WriteLine("hata(" + DateTime.Now.ToString() + "):" + ex.Message);
                        dosya.Close();
                    }
                }
                catch { }
            }
        }

        public static void LogSaveForFile(EnmLogTur logTur, string icerik)
        {
            string logDirectory = "logs";
            string strAy = DateTime.Today.ToString("yyyy.MM");
            string strGun = DateTime.Today.ToString("yyyy.MM.dd");

            try
            {
                if (!System.IO.Directory.Exists(logDirectory))
                {
                    System.IO.Directory.CreateDirectory(logDirectory);
                }

                lock (LogKilit)
                {
                    System.IO.StreamWriter dosya = System.IO.File.AppendText(logDirectory + "/log_" + strAy + ".txt");
                    dosya.WriteLine(logTur.ToString() + " : " + icerik);
                    dosya.Close();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    lock (LogKilit)
                    {
                        System.IO.StreamWriter dosya = System.IO.File.AppendText(logDirectory + "/log_err_" + strGun + ".txt");
                        dosya.WriteLine("hata(" + DateTime.Now.ToString() + "):" + ex.Message);
                        dosya.Close();
                    }
                }
                catch { }
            }
        }

        private static void SaveLog(EnmLogTur logTur, string icerik)
        {
            //MyApp.LogSaveForDb(logTur,icerik); //dbye yazacağın zaman açarsın
            MyApp.LogSaveForFile(logTur, icerik);
        }

        public static void WriteLog(EnmLogTur logTur, string icerik)
        {
            MyApp.SaveLog(logTur, icerik);
        }

        public static void WriteLogForMethodExceptionMessage(MethodBase method, Exception ex)
        {
            var sbParams = new System.Text.StringBuilder();
            foreach (var pi in method.GetParameters())
            {
                sbParams.Append(pi.Name + "=" + "??");
            }
            string icerik = $"{method.DeclaringType.FullName}.{method.Name}({sbParams}) => hata : " + ex.ToString() + " StackTrace: " + ex.StackTrace;
            MyApp.SaveLog(EnmLogTur.Hata, icerik);
        }

        public static void WriteLogForMethod(MethodBase method, EnmLogTur logTur, string text)
        {
            var sbParams = new System.Text.StringBuilder();
            foreach (var pi in method.GetParameters())
            {
                sbParams.Append(pi.Name + "=" + "??");
            }
            string icerik = $"{method.DeclaringType.FullName}.{method.Name}({sbParams}) => log : " + text;
            MyApp.SaveLog(logTur, icerik);
        }

        #endregion

        #region AppDirectory

        public static string AppAreasDirectory { get; set; } = "Areas";
        public static string AppDictionaryDirectory { get; set; } = "Dictionary";
        public static string AppCompsDirectory { get; set; } = "Comps";

        public static string AppDataDirectory { get; set; } = "Data";

        public static string AppFilesDirectory { get; set; } = MyApp.AppDataDirectory + "\\" + "Files";
        public static string UserFilesDirectory(string userCode)
        {
            return MyApp.Env.WebRootPath + "\\" + MyApp.AppFilesDirectory + "\\" + userCode;
        }

        public static string AppThumbsDirectory { get; set; } = MyApp.AppDataDirectory + "\\" + "Thumbs";
        public static string UserThumbsDirectory(string userCode)
        {
            return MyApp.Env.WebRootPath + "\\" + MyApp.AppThumbsDirectory + "\\" + userCode;
        }

        #endregion

        #region nft dir

        public static string UserProjectDirectory(string userCode, string projectName)
        {
            return MyApp.UserFilesDirectory(userCode) + "\\" + projectName;
        }

        public static string UserImportDirectory(string userCode, string projectName)
        {
            return MyApp.UserProjectDirectory(userCode, projectName) + "\\" + "import";
        }

        public static string UserExportDirectory(string userCode, string projectName)
        {
            return MyApp.UserProjectDirectory(userCode, projectName) + "\\" + "export";
        }
        #endregion

        #region Sabitler
        public static string Ayrac { get; set; } = " | ";

        public static string InsertDateColumnName { get; set; } = "InsertDateTime";
        public static string UpdateDateColumnName { get; set; } = "UpdateDateTime";
        public static string InsertUserColumnName { get; set; } = "InsertUserId";
        public static string UpdateUserColumnName { get; set; } = "UpdateUserId";

        #endregion

        #region Areas
        public static void CheckAreas()
        {
            foreach (string areaName in MyApp.Areas)
            {
                #region ContentRootPath
                string dirContentArea = MyApp.EnvContentRootPath + "\\" + MyApp.AppAreasDirectory + "/" + areaName;
                if (!System.IO.Directory.Exists(dirContentArea))
                {
                    System.IO.Directory.CreateDirectory(dirContentArea);
                }

                //Codes
                if (true)
                {
                    string dirCodes = dirContentArea + "/" + "Codes";
                    if (!System.IO.Directory.Exists(dirCodes))
                    {
                        System.IO.Directory.CreateDirectory(dirCodes);
                    }
                }

                //Controllers
                if (true)
                {
                    string dirControllers = dirContentArea + "/" + "Controllers";
                    if (!System.IO.Directory.Exists(dirControllers))
                    {
                        System.IO.Directory.CreateDirectory(dirControllers);
                    }
                }

                //Dmo
                if (true)
                {
                    string dirDmo = dirContentArea + "/" + "Dmo";
                    if (!System.IO.Directory.Exists(dirDmo))
                    {
                        System.IO.Directory.CreateDirectory(dirDmo);
                    }
                }

                //Dto
                if (true)
                {
                    string dirDto = dirContentArea + "/" + "Dto";
                    if (!System.IO.Directory.Exists(dirDto))
                    {
                        System.IO.Directory.CreateDirectory(dirDto);
                    }
                }

                //Models
                if (true)
                {
                    string dirModels = dirContentArea + "/" + "Models";
                    if (!System.IO.Directory.Exists(dirModels))
                    {
                        System.IO.Directory.CreateDirectory(dirModels);
                    }

                    string fileName = dirModels + "/_Rep.cs";
                    if (!System.IO.File.Exists(fileName))
                    {
                        var sbFileContents = new System.Text.StringBuilder();

                        sbFileContents.AppendLine("using System;");
                        sbFileContents.AppendLine("");
                        sbFileContents.AppendLine("//<!-- Auto Generated -->");
                        sbFileContents.AppendLine("namespace WebApp1.Models");
                        sbFileContents.AppendLine("{");
                        sbFileContents.AppendLine("    public partial class _Rep");
                        sbFileContents.AppendLine("    {");
                        sbFileContents.AppendLine("        /*code_generator_property_start*/");
                        sbFileContents.AppendLine("        ");
                        sbFileContents.AppendLine("        /*code_generator_property_end*/");
                        sbFileContents.AppendLine("        ");
                        sbFileContents.AppendLine($"        public void Init_{areaName}()");
                        sbFileContents.AppendLine("        {");
                        sbFileContents.AppendLine("            /*code_generator_constructor_start*/");
                        sbFileContents.AppendLine("            ");
                        sbFileContents.AppendLine("            /*code_generator_constructor_end*/");
                        sbFileContents.AppendLine("        }");
                        sbFileContents.AppendLine("    };");
                        sbFileContents.AppendLine("};");

                        System.IO.File.WriteAllText(fileName, sbFileContents.ToString());
                    }
                }

                #endregion

                #region WebRootPath
                string dirWebArea = MyApp.EnvWebRootPath + "\\" + MyApp.AppAreasDirectory + "\\" + areaName;
                if (!System.IO.Directory.Exists(dirWebArea))
                {
                    System.IO.Directory.CreateDirectory(dirWebArea);
                }

                // Authority
                if (true)
                {
                    string dirAuthority = dirWebArea + "/" + "Authority";
                    if (!System.IO.Directory.Exists(dirAuthority))
                    {
                        System.IO.Directory.CreateDirectory(dirAuthority);
                    }

                    string fileName = dirAuthority + "/" + areaName + ".js";
                    if (!System.IO.File.Exists(fileName))
                    {
                        var sbFileContents = new System.Text.StringBuilder();

                        sbFileContents.AppendLine($@"window.{areaName}Authority =");
                        sbFileContents.AppendLine(@"    {");
                        sbFileContents.AppendLine($@"       ");
                        sbFileContents.AppendLine(@"    };");

                        System.IO.File.WriteAllText(fileName, sbFileContents.ToString());
                    }
                }

                //Dictionary
                if (true)
                {
                    string dirDictionary = dirWebArea + "/" + "Dictionary";
                    if (!System.IO.Directory.Exists(dirDictionary))
                    {
                        System.IO.Directory.CreateDirectory(dirDictionary);
                    }

                    string fileName = dirDictionary + "/" + "_.json";
                    if (!System.IO.File.Exists(fileName))
                    {
                        var sbFileContents = new System.Text.StringBuilder();

                        sbFileContents.AppendLine($@"[");
                        sbFileContents.AppendLine($@"   ");
                        sbFileContents.AppendLine($@"]");

                        System.IO.File.WriteAllText(fileName, sbFileContents.ToString());
                    }
                }

                //Views
                if (true)
                {
                    string dirViews = dirWebArea + "/" + "Views";
                    if (!System.IO.Directory.Exists(dirViews))
                    {
                        System.IO.Directory.CreateDirectory(dirViews);
                    }
                }
                #endregion
            }
        }

        public static List<string> Areas
        {
            get
            {
                var rV = new List<string>();
                try
                {
                    // geçerli yöntem, sıaralama ihtiyacına göre ekleyebilir, açıp kapayabilirsin appsettings.json dan
                    foreach (var area in MyApp.Configs.ActiveAreas.Split(","))
                    {
                        rV.Add(area);
                    }
                }
                catch { }

                return rV;
            }
        }
        #endregion

        #region Sözlük
        public static string[] VeriDiliTablolari
        {
            get
            {
                return new string[] { "RobKelime", "RobKategori", "RobDers", "RobZorluk", "RobSeviye", "RobBasarim", "RobKazanim", "RobBeceri", "RobYas" };
            }
        }
        public static Dictionary<string, Dictionary<string, string>> Sozluk { get; set; }

        public static void LoadDictionary()
        {
            MyApp.Sozluk = new Dictionary<string, Dictionary<string, string>>();
            var fileList = new List<string>();
            List<string> dirList = MyApp.Areas;
            dirList.Add("_");

            foreach (string dirItem in dirList)
            {
                var dirListInfo = new System.IO.DirectoryInfo(MyApp.EnvWebRootPath + "\\" + MyApp.AppAreasDirectory + "\\" + dirItem);
                foreach (var dirSubItem in dirListInfo.GetDirectories(MyApp.AppDictionaryDirectory))
                {
                    IEnumerable<System.IO.FileInfo> files = dirSubItem.EnumerateFiles("*" + ".json", System.IO.SearchOption.TopDirectoryOnly);
                    fileList.AddRange(files.Select(s => s.FullName));
                }
            }

            foreach (var filePath in fileList)
            {
                if (System.IO.File.Exists(filePath))
                {
                    string jsonString = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        var dynamicObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);
                        if (dynamicObject != null && dynamicObject.Count > 0)
                        {
                            foreach (var itemObject in dynamicObject)
                            {
                                var itemDict = new Dictionary<string, string>();
                                foreach (var item in itemObject.value)
                                {
                                    itemDict.Add(item.Name, item.Value.Value);
                                }
                                MyApp.Sozluk[itemObject.key.Value] = itemDict;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Translate
        public static string TranslateToForPrms(string key, string lang, string[] prms)
        {
            string rValue = key;

            if (MyApp.Sozluk.ContainsKey(key))
            {
                Dictionary<string, string> row = MyApp.Sozluk[key];
                rValue = row["tr"]; //default tr value
                if (row.ContainsKey(lang))
                {
                    rValue = row[lang];
                }
            }

            try
            {
                if (prms != null && prms.Length > 0)
                {
                    rValue = string.Format(rValue, prms);
                }
            }
            catch { }

            return rValue;
        }

        public static string TranslateTo(string key, string lang)
        {
            return MyApp.TranslateToForPrms(key, lang, null);
        }

        public static string TranslateWithBingApiV3(string _text, string _target, string _source, Boolean _titleCase)
        {
            string rV = "";
            string url = "https://www.bing.com/ttranslatev3?isVertical=1&&IG=13CE46ACD94A492AB143E9E789D6F4E1&IID=translator.5023.24";

            using (var wb = new System.Net.WebClient())
            {
                var data = new System.Collections.Specialized.NameValueCollection() { };
                data["fromLang"] = _source;
                data["text"] = _text;
                data["to"] = _target;
                data["token"] = "dk8cZuOUIFqbWRkIFwOUgABKKpHpLXfB";
                data["key"] = "1637248824077";

                var response = wb.UploadValues(url, "POST", data);
                string jsonText = System.Text.Encoding.UTF8.GetString(response);
                dynamic stuff = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText);
                rV = stuff[0].translations[0].text.ToString();
            }


            if (_titleCase)
            {
                rV = rV.MyToTitleCase();
            }

            return rV.Trim();
        }

        public static string TranslateWithGoogleApi(string _text, string _target, string _source, Boolean _titleCase)
        {
            string rV = "";
            string url = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyCJ7_NoaZ4cFjupJuTikKc0-Oq1wH1sAdg";
            url += "&source=" + _source;
            url += "&target=" + _target;
            url += "&q=" + _text;

            using (var client = new System.Net.WebClient())
            {
                //System.Net.WebClient client = new System.Net.WebClient();
                client.Headers.Add("referer", "https://s.codepen.io");
                string jsonText = client.DownloadString(url);
                dynamic stuff = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText);
                rV = stuff.data.translations[0].translatedText.Value;
            }

            if (_titleCase)
            {
                rV = rV.MyToTitleCase();
            }

            return rV.Trim();
        }

        #endregion 

        #region GetSqlScript
        public static string GetSqlScript(string fileName)
        {
            string rV = "";

            var resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(MyApp.Env.ApplicationName + ".SqlScript." + fileName);
            if (resourceStream != null)
            {
                using var reader = new System.IO.StreamReader(resourceStream, System.Text.Encoding.UTF8);
                rV = reader.ReadToEnd();
            }

            return rV;
        }
        #endregion

        #region DB Compare
        public static System.Text.StringBuilder CompareDatabase(string sourceConStr, string targetConStr)
        {
            var logs = new System.Text.StringBuilder();

            var kaynakSchema = new DatabaseSchemaReader.DatabaseReader(new Microsoft.Data.SqlClient.SqlConnection(sourceConStr)).ReadAll();
            var hedefSchema = new DatabaseSchemaReader.DatabaseReader(new Microsoft.Data.SqlClient.SqlConnection(targetConStr)).ReadAll();

            var comparison = new DatabaseSchemaReader.Compare.CompareSchemas(hedefSchema, kaynakSchema);

            logs.Append(comparison.Execute());

            return logs;
        }
        #endregion

        #region kendo filter
        public static Kendo.Mvc.UI.DataSourceRequest ApiRequestToDataSourceRequest(ApiRequest request)
        {
            Kendo.Mvc.UI.DataSourceRequest req = new();

            req.Page = request.Page;
            req.PageSize = request.PageSize;

            if (request.Filters != null)
            {
                var compositeFilterDescriptor = new Kendo.Mvc.CompositeFilterDescriptor()
                {
                    LogicalOperator = Kendo.Mvc.FilterCompositionLogicalOperator.And
                };

                foreach (var filter in request.Filters)
                {
                    var filterOperator = (Kendo.Mvc.FilterOperator)Enum.Parse(typeof(Kendo.Mvc.FilterOperator), filter.Operator);
                    var filterDescriptor = new Kendo.Mvc.FilterDescriptor(filter.Field, filterOperator, filter.Value);
                    compositeFilterDescriptor.FilterDescriptors.Add(filterDescriptor);
                }

                req.Filters = new List<Kendo.Mvc.IFilterDescriptor>() { };
                req.Filters.Add(compositeFilterDescriptor);
            }

            if (request.Sorts != null)
            {
                req.Sorts = new List<Kendo.Mvc.SortDescriptor>();
                if (request.Sorts != null)
                {
                    foreach (var sort in request.Sorts)
                    {
                        Kendo.Mvc.ListSortDirection sortDirection = Kendo.Mvc.ListSortDirection.Ascending;
                        if (sort.Dir == "desc")
                        {
                            sortDirection = Kendo.Mvc.ListSortDirection.Descending;
                        }

                        req.Sorts.Add(new Kendo.Mvc.SortDescriptor()
                        {
                            Member = sort.Field,
                            SortDirection = sortDirection
                        });
                    }
                }
            }
            return req;
        }
        #endregion
    }

    #endregion
}
