using System;
using System.Collections.Generic;
using System.Linq;
using WebApp1.Codes;
using WebApp1.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Reflection;

namespace WebApp1.Areas.Tem.Codes
{
    #region Temel Business

    public class TemBusiness
    {
        private readonly DataContext dataContext;
        private readonly _Rep rep = null;

        public TemBusiness(DataContext _dataContext)
        {
            if (_dataContext != null)
            {
                this.dataContext = new DataContext(new DbContextOptions<DataContext>());
                this.dataContext.Database.GetDbConnection().ConnectionString = _dataContext.Database.GetDbConnection().ConnectionString;
                this.dataContext.IPAddress = _dataContext.IPAddress;
                this.dataContext.KurulusKod = _dataContext.KurulusKod;
                this.dataContext.ConStrings = _dataContext.ConStrings;
                this.dataContext.UserId = _dataContext.UserId;
                this.dataContext.UserName = _dataContext.UserName;

                this.rep = new Models._Rep(this.dataContext);
            }
        }

        #region kullanıcı metodları
        //public void SetCulture(string culture)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo(culture);
        //        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(culture);
        //    }
        //}

        public MoResponse<MoCreateCaptchaResponse> CreateCaptcha()
        {
            MoResponse<MoCreateCaptchaResponse> response = new() { Data = new MoCreateCaptchaResponse() };

            try
            {
                var moCaptchaToken = new MoCaptchaToken()
                {
                    Code = MyCaptcha.NewText()
                };

                if (MyApp.Env.EnvironmentName == "Development")
                {
                    moCaptchaToken.Code = "1111";
                }

                var captcha = MyCaptcha.Make(moCaptchaToken.Code, 250, 100);
                var x = MyApp.GenerateCaptchaToken(moCaptchaToken);

                response.Data = new MoCreateCaptchaResponse
                {
                    CaptchaImage = captcha,
                    CaptchaToken = MyApp.GenerateCaptchaToken(moCaptchaToken)
                };

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        public MoResponse<MoUserToken> UserLoginForApi(MoLogin input)
        {
            MoResponse<MoUserToken> response = new() { Data = new MoUserToken() };

            try
            {
                var userModel = this.dataContext.TemKullanici
                .Where(c => c.Id > 0)
                .Where(c => c.Ad == input.UserName && c.Sifre == input.Password.MyToEncryptPassword())
                .Select(s => new
                {
                    s.Id,
                    s.Durum,
                    s.Ad,
                    s.Resim,
                    s.Rols
                })
                .ToList()
                .FirstOrDefault();

                if (userModel != null)
                {
                    if (userModel.Durum == true)
                    {
                        response.Data.Culture = input.Culture;
                        response.Data.UserId = userModel.Id;
                        response.Data.UserName = userModel.Ad;
                        response.Data.NameSurname = userModel.Ad;
                        response.Data.LisansGun = this.GetKullaniciLisansGun(userModel.Id);

                        response.Data.UserIsLogon = true;
                        if (userModel.Rols.MyToStr().Contains("1001"))
                        {
                            response.Data.YetkiGrup = EnmYetkiGrup.Admin;
                            response.Data.LisansGun = 365;
                        }
                        else if (userModel.Rols.MyToStr().Length > 0)
                        {
                            response.Data.YetkiGrup = EnmYetkiGrup.Personel;
                            response.Data.LisansGun = 365;
                        }
                        else
                        {
                            response.Data.YetkiGrup = EnmYetkiGrup.Musteri;
                        }

                        Areas.Tem.Dto.DtoTemOturumLog dtoTemOturumLog = this.rep.Areas_Tem_RepTemOturumLog.GetByNew();
                        dtoTemOturumLog.KullaniciId = response.Data.UserId;
                        dtoTemOturumLog.GirisZaman = DateTime.Now;
                        dtoTemOturumLog.CikisZaman = DateTime.Now;
                        dtoTemOturumLog.InternetProtokolAdres = input.IPAdres;
                        dtoTemOturumLog.OturumGuid = response.Data.OturumGuid;
                        dtoTemOturumLog.Tarayici = input.Tarayici.MyToMaxLength(250);
                        this.rep.Areas_Tem_RepTemOturumLog.CreateOrUpdate(dtoTemOturumLog);

                        this.rep.SaveChanges();

                        response.Success = true;
                    }
                    else
                    {
                        response.Message.Add(MyApp.TranslateTo("xLng.HesabinizAskiyaAlinmistir", this.dataContext.Language));
                    }
                }
                else
                {
                    response.Message.Add(MyApp.TranslateTo("xLng.KullaniciveyaSifreGecersiz", this.dataContext.Language));
                }

            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        public MoResponse<List<MoOturumInfo>> GetConnectedUserList()
        {

            MoResponse<List<MoOturumInfo>> response = new();

            var data = this.dataContext.TemOturumLog
                .Include(i => i.Kullanici)
                .Where(c => c.CikisZaman >= DateTime.Now.AddMinutes(-2))
                .Where(c => c.OturumGuid.Length > 0)
                .OrderBy(o => o.GirisZaman)
                .ToList()
                .Select(s => new Codes.MoOturumInfo
                {
                    KullaniciId = s.KullaniciId,
                    KullaniciAd = s.Kullanici.Ad,
                    GirisZaman = s.GirisZaman,
                    CikisZaman = s.CikisZaman,
                    Zaman = (s.CikisZaman - s.GirisZaman).Value.TotalMinutes.MyToInt()
                }).ToList();

            response.Data = data;
            response.Success = true;

            return response;
        }

        #endregion

        #region Genel

        public int GetKullaniciLisansGun(int _kullaniciId)
        {
            int rV = 0;
            try
            {
                var kullanici = this.dataContext.TemKullanici
                    .Where(c => c.Id > 0 && c.Id == _kullaniciId)
                    .FirstOrDefault();

                if (kullanici != null)
                {
                    #region TemKullaniciLisans
                    var kullaniciLisans = this.dataContext.TemKullaniciLisans
                        .Where(c => c.Durum == true && c.KullaniciId == _kullaniciId);

                    if (kullaniciLisans.Any())
                    {
                        // başlama ve bitiş tarihi arasındaki gün toplamı
                        kullaniciLisans.Where(c => c.BitisTarihi >= DateTime.Now.Date);
                        int gunler = 0;
                        foreach (var item in kullaniciLisans)
                        {
                            var gun = item.BitisTarihi.Value - DateTime.Now.Date;
                            gunler += gun.TotalDays.MyToInt();
                        }
                        rV += gunler;

                        // Bitiş tarihi null olan bir kayıt var ise lisans bitişi intmax gündür
                        if (kullaniciLisans.Where(c => c.BitisTarihi == null).Any())
                        {
                            rV += 365;
                        }
                    }

                    #endregion

                    #region TemKullaniciAbonelikOdeme
                    var kullaniciAbonelikOdeme = this.dataContext.TemKullaniciAbonelikOdeme
                       .Where(c => c.Durum == true && c.KullaniciAbonelik.KullaniciId == _kullaniciId);

                    if (kullaniciAbonelikOdeme.Any())
                    {
                        // başlama ve bitiş tarihi arasındaki gün toplamı
                        kullaniciAbonelikOdeme.Where(c => c.BitisTarihi >= DateTime.Now.Date);
                        int gunler = 0;
                        foreach (var item in kullaniciAbonelikOdeme)
                        {
                            var gun = item.BitisTarihi.Value - DateTime.Now.Date;
                            gunler += gun.TotalDays.MyToInt();
                        }
                        rV += gunler;

                        // Bitiş tarihi null olan bir kayıt var ise lisans bitişi intmax gündür
                        if (kullaniciAbonelikOdeme.Where(c => c.BitisTarihi == null).Any())
                        {
                            rV += 365;
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public Boolean KullaniciMailAdresVarmi(string _mail)
        {
            Boolean rV = false;

            try
            {
                rV = this.dataContext.TemKullanici
                .Where(c => c.Ad == _mail)
                .Any();
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public Boolean KullaniciKayitTalepMailGonder(string _mail, string _adSoyad, int _dogumYili, string _sifre)
        {
            Boolean rV = false;
            try
            {
                //uyelik bilgileri şifrelenip link haline getiriliyor, mail gönderilecek 
                var jsonObj = new { Mail = _mail, AdSoyad = _adSoyad, DogumYili = _dogumYili, Sifre = _sifre };
                var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj);
                string onayLinkValue = jsonText.MyToEncrypt("Aa123");

                using var mailHelper = new MyMailHelper(this.dataContext);
                rV = mailHelper.SendMail_UyeMailOnay(_mail, _adSoyad, onayLinkValue);
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public MoResponse KullaniciKaydet(string prms)
        {
            var response = new MoResponse();
            try
            {
                //uyelik bilgileri şifrelenip link haline getirilmiş ve buraya linkden düşmüştür,şifre açılıp kullanınıcı kaydı yapılacak
                string jsonText = prms.MyToDecrypt("Aa123");
                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonText);
                string dogumYili = jsonObj.DogumYili.ToString();

                // kullanıcı kaydı
                var dtoKullanici = new Tem.Dto.DtoTemKullanici(this.dataContext)
                {
                    Durum = true,
                    KayitZaman = DateTime.Now,
                    Ad = jsonObj.Mail.ToString(),
                    Sifre = jsonObj.Sifre.ToString()
                };
                int kullaniciId = rep.Areas_Tem_RepTemKullanici.CreateOrUpdate(dtoKullanici);
                var sonuc = this.rep.SaveChanges();

                response.Message.Add($"{MyApp.TranslateTo("xLng.Sayin", this.dataContext.Language)} {jsonObj.AdSoyad.ToString()} {MyApp.TranslateTo("xLng.KayitIslemiBasariylaYapilmistir", this.dataContext.Language)}");

            }
            catch (Exception ex)
            {
                response.Error = true;
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }


        //sahip ve tür beraber
        public string GetKullaniciSahipAdSahipTur(int _id)
        {
            string rV = "";
            //Sahip bağlantısı olan tablodan adı alınır, (sahip tablolarının Ad alanı olmalıdır)
            try
            {
                var kullanici = this.dataContext.TemKullanici
                    .Where(c => c.Id > 0 && c.Id == _id)
                    .FirstOrDefault();

                if (kullanici != null)
                {
                    if (kullanici.Rols.MyToStr().Contains("1001"))
                    {
                        rV = EnmYetkiGrup.Admin.ToString();
                    }
                    else if (kullanici.Rols.MyToStr().Length > 0)
                    {
                        rV = EnmYetkiGrup.Personel.ToString();
                    }
                    else
                    {
                        rV = EnmYetkiGrup.Musteri.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public string GetKullaniciResim(int _id)
        {
            string rV = "";

            try
            {
                var kullanici = this.dataContext.TemKullanici
                    .Where(c => c.Id > 0)
                    .Where(c => c.Id == _id)
                    .FirstOrDefault();

                if (kullanici != null)
                {
                    rV = kullanici.Resim.MyToStr();
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public MoResponse<object> SetKullaniciResim(int userId, string imageData, string culture)
        {
            MoResponse<object> response = new();

            try
            {
                if (imageData.Length < 99000)
                {
                    var user = this.dataContext.TemKullanici
                        .Where(c => c.Id > 0 && c.Id == userId)
                        .FirstOrDefault();
                    if (user != null)
                    {
                        user.Resim = imageData;
                        this.dataContext.Update(user);
                        this.dataContext.SaveChanges();
                        response.Success = true;
                    }
                }
                else
                {
                    response.Message.Add(MyApp.TranslateTo("xLng.GonderdiginizResimBuyuk", culture));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.Message);
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return response;
        }

        public IEnumerable<string> GetKullaniciYetkiler(int userId)
        {
            IEnumerable<string> rV = new List<string>();
            try
            {
                var kullanici = MyCache.TemKullaniciList.Where(c => c.Id == userId).FirstOrDefault();
                if (kullanici != null)
                {
                    foreach (string rolId in kullanici.Rols.MyToStr().Split(','))
                    {
                        if (rolId.MyToTrim().Length > 0)
                        {
                            var rolRow = MyCache.TemRolList.Where(c => c.Id == Convert.ToInt32(rolId)).FirstOrDefault();
                            if (rolRow != null)
                            {
                                if (rolRow.Yetki != null)
                                {
                                    rV = rV.Union(rolRow.Yetki.Split(','));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public Boolean UserIsAuthorized(int userId, string _Key)
        {
            Boolean rV = false;
            try
            {
                var kullanici = MyCache.TemKullaniciList
                    .Where(c => c.Id == userId).FirstOrDefault();

                if (kullanici != null)
                {
                    var userYetki = this.GetKullaniciYetkiler(userId);

                    //admin vey a yönetici değil ise normal user ...
                    if (kullanici.Rols.MyToStr().Contains("1001"))
                    {
                        rV = true;
                    }
                    else
                    {
                        if (userYetki != null && userYetki.Any())
                        {
                            var sonuc = userYetki.Where(c => c.Contains(_Key)).FirstOrDefault();
                            if (!string.IsNullOrEmpty(sonuc))
                            {
                                rV = sonuc.Length > 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }

        public TemKullanici GetKullanici(int _id)
        {
            var rV = new TemKullanici();

            try
            {
                var kullanici = this.dataContext.TemKullanici
                    .Where(c => c.Id > 0)
                    .Where(c => c.Id == _id)
                    .FirstOrDefault();

                if (kullanici != null)
                {
                    rV = kullanici;
                }
            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return rV;
        }


        #endregion

        #region lookup
        public Kendo.Mvc.UI.DataSourceResult GetLookupRead(string culture, LookupRequest lookupRequest)
        {
            Kendo.Mvc.UI.DataSourceResult dsr = new();

            //hangi tablo dan ise ondan çekilecek, (tablo içinde VeriDili alanı var ise içindeki ni dönecek hale getir)
            lookupRequest.TableName = lookupRequest.TableName.MyToStrAntiInjection();
            lookupRequest.ValueField = lookupRequest.ValueField.MyToStrAntiInjection();

            string textFieldLine = string.Empty;
            if (!string.IsNullOrEmpty(lookupRequest.TextField))
            {
                foreach (string textField in lookupRequest.TextField.Split(","))
                {
                    if (!string.IsNullOrEmpty(textFieldLine))
                    {
                        textFieldLine += " + ' ' + ";
                    }
                    textFieldLine += textField.MyToStrAntiInjection();
                }
            }
            string sqlSelect = "Select " + lookupRequest.ValueField + " as value, " + textFieldLine + " as text";

            if (!string.IsNullOrEmpty(lookupRequest.OtherFields.MyToTrim()))
            {
                sqlSelect += ", " + lookupRequest.OtherFields;
            }

            string sqlFrom = " From " + lookupRequest.TableName;
            string sqlWhere = "";
            string sqlWhereLine = "";
            string sqlOrder = "";
            string sqlOrderLine = "";

            //filters
            if (lookupRequest.Filters != null)
            {
                foreach (var filterLine in lookupRequest.Filters)
                {
                    if (sqlWhereLine.Length > 0)
                    {
                        sqlWhereLine += " and ";
                    }

                    if (filterLine.ValueType == "Int" | filterLine.ValueType == "Decimal" | filterLine.ValueType == "Float" | filterLine.ValueType == "Boolean")
                    {
                        sqlWhereLine += filterLine.Field + filterLine.Operator + filterLine.Value;
                    }

                    if (filterLine.ValueType == "String" | filterLine.ValueType == "DateTime")
                    {
                        sqlWhereLine += filterLine.Field + filterLine.Operator + "'" + filterLine.Value + "'";
                    }
                }

                if (sqlWhereLine.Length > 0)
                {
                    sqlWhere += " Where " + sqlWhereLine;
                }
            }

            //sort
            if (lookupRequest.Sorts != null)
            {
                foreach (var sortLine in lookupRequest.Sorts)
                {
                    if (sqlOrderLine.Length > 0)
                    {
                        sqlOrderLine += ", ";
                    }

                    if (sortLine.Field.MyToTrim().Length > 0)
                    {
                        sqlOrderLine += sortLine.Field.MyToTrim() + " " + sortLine.Dir.MyToTrim();
                    }
                }

                if (sqlOrderLine.MyToTrim().Length > 0)
                {
                    sqlOrder = " Order By " + sqlOrderLine;
                }
            }

            #region veri dili işlemleri
            string[] tablesVeriDili = MyApp.VeriDiliTablolari;
            try
            {
                //veri dili verisi
                if (tablesVeriDili.Contains(lookupRequest.TableName))
                {
                    sqlSelect += ", VeriDili";
                }
            }
            catch { }

            #endregion

            var sqlCommand = sqlSelect + " " + sqlFrom + " " + sqlWhere + " " + sqlOrder;

            var queryResult = this.dataContext.GetDbConnection().Query<dynamic>(sqlCommand);


            #region veri dili işlemleri

            try
            {
                //veri dili döngüsü
                if (tablesVeriDili.Contains(lookupRequest.TableName))
                {
                    foreach (var item in queryResult)
                    {
                        if (item.VeriDili != null)
                        {
                            string text = "";
                            foreach (string textField in lookupRequest.TextField.Split(","))
                            {
                                if (!string.IsNullOrEmpty(text))
                                {
                                    text += " + ' ' + ";
                                }
                                string itemVeriDili = item.VeriDili.ToString();
                                string itemText = item.text.ToString();
                                text += itemVeriDili.MyVeriDiliToStr(culture.Substring(0, 2), textField, itemText);

                            }
                            item.text = text;
                        }
                        item.VeriDili = "";
                    }
                }
            }
            catch { }

            #endregion

            dsr.Data = queryResult;
            dsr.Total = queryResult.Count();

            return dsr;

        }
        #endregion
    }

    #endregion
}
