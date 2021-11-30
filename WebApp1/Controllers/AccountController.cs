using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApp1.Areas.Tem.Codes;
using WebApp1.Codes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp1.Controllers
{
    public class AccountController : _Controller
    {
        public AccountController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        public void FnLogout()
        {
            if (this.userToken.UserIsLogon)
            {
                try
                {
                    var modelTemOturumLog = this.dataContext.TemOturumLog
                        .Where(c => c.OturumGuid == this.userToken.OturumGuid)
                        .OrderBy(o => o.Id)
                        .LastOrDefault();
                    if (modelTemOturumLog != null)
                    {
                        modelTemOturumLog.OturumGuid = "";
                        modelTemOturumLog.CikisZaman = DateTime.Now;
                        this.dataContext.SaveChanges();
                    }
                }
                catch { }
            }
        }

        // GET: /<controller>/
        [ResponseCache(Duration = 0)]
        public IActionResult Index(string culture)
        {
            TemBusiness temBusiness = new(this.dataContext);
            var resCaptcha = temBusiness.CreateCaptcha();
           
            if (!string.IsNullOrEmpty(culture))
            {
                //temBusiness.SetCulture(culture);
                ViewBag.Culture = culture;
            }

            ViewBag.CaptchaImage = resCaptcha.Data.CaptchaImage;
            ViewBag.CaptchaToken = resCaptcha.Data.CaptchaToken;

            ViewBag.LogoImageUrl = "/img/logo/logoYatay.png?v" + WebApp1.Codes.MyApp.Version;
            ViewBag.GirisImageUrl = "/img/giris/login.png?v" + WebApp1.Codes.MyApp.Version;

            //login buton renkleri
            ViewBag.GirisButonFonRengi = "#164574";
            ViewBag.GirisButonYaziRengi = "white";

            if (MyApp.Env.EnvironmentName == "Development")
            {
                //ViewBag.UserName = "admin"; 
                //ViewBag.Password = "1";

                ViewBag.UserName = "customer";
                ViewBag.Password = "1";

                //ViewBag.UserName = "mutlutan@outlook.com"; 
                //ViewBag.Password = "123";

                ViewBag.SecurityCode = "1111";
            }

            return View();
        }

        [ResponseCache(Duration = 0)]
        public IActionResult SignUp(string culture)
        {
            TemBusiness temBusiness = new(this.dataContext);
            var resCaptcha = temBusiness.CreateCaptcha();

            if (!string.IsNullOrEmpty(culture))
            {
                ViewBag.Culture = culture;
            }

            ViewBag.CaptchaImage = resCaptcha.Data.CaptchaImage;
            ViewBag.CaptchaToken = resCaptcha.Data.CaptchaToken;

            ViewBag.LogoImageUrl = "/img/logo/logoYatay.png?v" + WebApp1.Codes.MyApp.Version;
            ViewBag.RegisterImageUrl = "/img/uyeol/uyeol.png?v" + WebApp1.Codes.MyApp.Version;

            //login buton renkleri
            ViewBag.UyeOlButonFonRengi = "#155ea8";
            ViewBag.UyeOlButonYaziRengi = "white";

            return View();
        }


        [HttpPost]
        public IActionResult UyeKayitTalep(string _mail, string _adSoyad, int _dogumYili, string _sifre, string _captchaCode, string _captchaToken)
        {
            Boolean rSuccess = false;
            string rMessage;
            try
            {
                var temBus = new Areas.Tem.Codes.TemBusiness(this.dataContext);

                var captchaValid = MyApp.ValidateCaptchaToken(_captchaCode, _captchaToken);

                if (captchaValid)
                {
                    if (!temBus.KullaniciMailAdresVarmi(_mail))
                    {
                        rSuccess = temBus.KullaniciKayitTalepMailGonder(_mail, _adSoyad, _dogumYili, _sifre);

                        if (rSuccess)
                        {
                            rMessage = MyApp.TranslateTo("xLng.UyelikTalebiOnayiMailAdresineGonderildi", this.dataContext.Language);
                        }
                        else
                        {
                            rMessage = MyApp.TranslateTo("xLng.UyelikTalebinizAlinamadi", this.dataContext.Language);
                        }
                    }
                    else
                    {
                        rMessage = MyApp.TranslateTo("xLng.GridiginizMailAdresiDahaOnceSistemeKaydedilmis", this.dataContext.Language);
                    }
                }
                else
                {
                    rMessage = MyApp.TranslateTo("xLng.GuvenlikKoduGecersiz", this.dataContext.Language);
                }
            }
            catch (Exception ex)
            {
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { Message = rMessage, Success = rSuccess });
        }

        [HttpGet]
        public IActionResult UyeKayitOnay(string prms)
        {
            //kayıt yapılacak
            var robTem = new TemBusiness(this.dataContext);
            var sonuc = robTem.KullaniciKaydet(prms);

            ViewBag.Message = sonuc.Message[0];

            return View();
        }

        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public void RefreshTimeOut()
        {
            //session time out olmasın diye

            if (this.userToken.UserIsLogon)
            {
                try
                {
                    //user çıkış bilgisi güncelleniyor (son işlem zamanı)(bu asenkron olsun bekleme yapmasın)
                    // son yazılabilen çıkış zamanı gerçeğe en yakın çıkış zamanıdır, session end event gelince bunu düzeltirsin
                    var dtoTemOturumLog = this.rep.Areas_Tem_RepTemOturumLog.Get()
                        .Where(c => c.OturumGuid == this.userToken.OturumGuid)
                        .OrderBy(o => o.Id)
                        .LastOrDefault();
                    if (dtoTemOturumLog != null)
                    {
                        dtoTemOturumLog.CikisZaman = DateTime.Now;
                        this.rep.Areas_Tem_RepTemOturumLog.CreateOrUpdate(dtoTemOturumLog);

                        this.rep.SaveChanges();
                    }
                }
                catch { }
            }
        }

        [ResponseCache(Duration = 0)]
        public ActionResult GetDictionary()
        {
            return Json(new { dictionary = MyApp.Sozluk, cultures = MyApp.Configs.SupportedCultures });
        }


        [ResponseCache(Duration = 0)]
        public ActionResult GetAppInfo()
        {
            return Json(new
            {
                appName = MyApp.AppName,
                version = MyApp.Version,
                areas = MyApp.Areas
            });
        }

        [ResponseCache(Duration = 0)]
        public ActionResult GetUserInfo()
        {
            var temVersiyon = new Models.TemVersiyon();
            try
            {
                temVersiyon = this.dataContext.TemVersiyon.FirstOrDefault();
            }
            catch { }

            var business = new TemBusiness(this.dataContext);
            var kullanici = business.GetKullanici(this.userToken.UserId);

            return Json(new
            {
                bLogin = this.userToken.UserIsLogon,
                bGuest = this.userToken.UserIsGuest,
                sCulture = this.userToken.Culture,
                nUserId = this.userToken.UserId,
                sUserName = this.userToken.UserName,
                bAdmin = this.userToken.YetkiGrup == EnmYetkiGrup.Admin,
                sAdSoyad = this.userToken.NameSurname,
                sKullaniciResim = kullanici.Resim.MyToStr(), // business.GetKullaniciResim(this.session.UserId),
                sKullaniciUyrukAd = "Türkiye",
                nYetkiGrup = (int)this.userToken.YetkiGrup,
                sUserYetki = business.GetKullaniciYetkiler(this.userToken.UserId),
                nSemaVersiyon = temVersiyon.Id,
                nLisansGun = this.userToken.LisansGun
            });
        }

        [HttpPost]
        [AuthenticateRequired]
        public ActionResult SetUserImage(string _imageData)
        {
            string rErrors = "";
            try
            {
                var temBusiness = new TemBusiness(this.dataContext);
                var response = temBusiness.SetKullaniciResim(this.userToken.UserId, _imageData, this.userToken.Culture);
                if (!response.Success)
                {
                    rErrors = string.Join(",", response.Message);
                }
            }
            catch (Exception ex)
            {
                rErrors = ex.MyLastInner().Message;
            }
            return Json(new { errors = rErrors });
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public IActionResult ChangePassword(string _sOldPassword, string _sPassword, string _sCaptchaCode, string _sCaptchaToken)
        {
            string rMessage = "";
            Boolean rOk = false;

            try
            {
                var captchaValid = MyApp.ValidateCaptchaToken(_sCaptchaCode, _sCaptchaToken);

                if (captchaValid)
                {
                    var user = this.dataContext.TemKullanici
                    .Where(c => c.Id > 0 && c.Id == this.userToken.UserId)
                    .Where(c => c.Sifre == _sOldPassword.MyToEncryptPassword())
                    .FirstOrDefault();

                    if (user != null)
                    {
                        if (_sPassword.Length >= 6)
                        {
                            user.Sifre = _sPassword.MyToEncryptPassword();
                            this.dataContext.SaveChanges();
                            rOk = true;
                            rMessage = MyApp.TranslateTo("xLng.KayitDuzeltildi", this.dataContext.Language);
                        }
                        else
                        {
                            rMessage = MyApp.TranslateTo("xLng.EnAzAltiKarekterBirHarfBirSayi", this.dataContext.Language);
                        }
                    }
                    else
                    {
                        rMessage = MyApp.TranslateTo("xLng.EskiSifreGecersiz", this.dataContext.Language);
                    }
                }
                else
                {
                    rMessage = MyApp.TranslateTo("xLng.GuvenlikKoduGecersiz", this.dataContext.Language);
                }
            }
            catch (Exception ex)
            {
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { sMessage = rMessage, bOk = rOk });
        }

        public ActionResult Logout()
        {
            string rValue = "";
            try
            {
                this.FnLogout();
            }
            catch (Exception ex)
            {
                rValue = ex.MyLastInner().Message;
            }
            return Json(new { sErrors = rValue });
        }

    }
}
