using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp1.Codes;
using WebApp1.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApp1.Areas.Tem.Codes;
using System.Reflection;

namespace WebApp1.Areas.Tem.Controllers
{
    //[Produces("application/json", "application/xml")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TemController : _Controller
    {
        public TemController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        // GET api/Tem/Test 
        /// <summary>
        /// Sunucu tarih saatini öğrenmek için kullanılır.
        /// </summary>
        /// <returns>Sunucu Tarih Saat</returns>
        [HttpPost("Test")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequiredAttribute]
        public IActionResult Test([FromBody] object request)
        {
            var resultObject = new { DateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), RequestValue = request };
            return Json(resultObject);
        }

        //Rate limiting kullanılmalı
        /// <summary>
        /// Captcha üretim metodu, dönüşte resim ve token döner, token validate için kullanılır
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateCaptcha")]
        [ResponseCache(Duration = 0)]
        public IActionResult CreateCaptcha()
        {
            MoResponse<MoCreateCaptchaResponse> response = new();

            try
            {
                TemBusiness temBusiness = new(this.dataContext);
                response = temBusiness.CreateCaptcha();
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.Message);
                MyApp.WriteLogForMethodExceptionMessage(System.Reflection.MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }


        //login
        [HttpPost("CreateToken")]
        [ResponseCache(Duration = 0)]
        public IActionResult CreateToken([FromBody] MoTokenRequest request)
        {
            MoResponse<object> response = new();
            try
            {
                var captchaValid = MyApp.ValidateCaptchaToken(request.CaptchaCode, request.CaptchaToken);
                
                if (captchaValid)
                {
                    TemBusiness temBusiness = new(this.dataContext);

                    var userLogin = temBusiness.UserLoginForApi(new MoLogin()
                    {
                        Culture = request.Culture,
                        UserName = request.UserName,
                        Password = request.Password,
                        IPAdres = this.dataContext.IPAddress,
                        Tarayici = this.browserDetector.Browser?.Name + " " + this.browserDetector.Browser?.Version + " " + this.browserDetector.Browser?.OS + " " + this.browserDetector.Browser?.DeviceType
                    });

                    if (userLogin.Success)
                    {
                        response.Data = new { UserToken = MyApp.GenerateUserToken((MoUserToken)userLogin.Data) };
                        response.Success = true;
                    }
                    else
                    {
                        response.Message.AddRange(userLogin.Message);
                    }
                }
                else
                {
                    response.Message.Add(MyApp.TranslateTo("xLng.GuvenlikKoduGecersiz", request.Culture));
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Hata: " + ex.MyLastInner().Message);
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        /// <summary>
        /// Döviz kuru almak için kullanılır (Merkez bankası)
        /// </summary>
        /// <param name="paraBirimKod">Para birimi Kodu (Merkez bankasındaki kod)</param>
        /// <param name="token">Metodu kullanmak için gerekli jeton</param>
        /// /// <param name="yil">Döviz kuru tarihinin yılı</param>
        /// <param name="ay">Döviz kuru tarihinin ayı</param>
        /// <param name="gun">Döviz kuru tarihinin günü</param>
        /// <returns>Döviz kuru bilgisi json olarak döner</returns>
        [HttpPost("DovizKuru")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequiredAttribute]
        public IActionResult DovizKuru([FromBody] MoRequestDovizKuruGetir request)
        {
            DateTime tarihSaat = DateTime.Now;
            String paraBirimSimge = "₺";
            decimal dovizAlis = 1;
            decimal dovizSatis = 1;
            decimal efektifAlis = 1;
            decimal efektifSatis = 1;
            string mesaj = "";
            Boolean durum = false;
            try
            {
                var paraBirim = dataContext.TemParaBirim.Where(c => c.Durum && c.Kod == request.ParaBirimKod).FirstOrDefault();
                if (paraBirim != null)
                {
                    var dovizKurArsiv = dataContext.TemDovizKurArsiv
                        .Include(i => i.ParaBirim)
                        .Where(c => c.ParaBirimId == paraBirim.Id && c.Tarih <= new DateTime(request.Yil, request.Ay, request.Gun))
                        .OrderBy(o => o.TarihSaat)
                        .LastOrDefault();
                    if (dovizKurArsiv != null)
                    {
                        paraBirimSimge = dovizKurArsiv.ParaBirim.Simge;
                        tarihSaat = dovizKurArsiv.TarihSaat;
                        dovizAlis = dovizKurArsiv.DovizAlis;
                        dovizSatis = dovizKurArsiv.DovizSatis;
                        efektifAlis = dovizKurArsiv.EfektifAlis;
                        efektifSatis = dovizKurArsiv.EfektifSatis;
                        durum = true;
                    }
                    else
                    {
                        mesaj = "İstenilen tarihte para birim için döviz kuru bilgisi bulunamadı.";
                    }
                }
                else
                {
                    mesaj = "Gönderilen para birim kodu geçersiz.";
                }
            }
            catch (Exception ex)
            {
                mesaj = "Hata: " + ex.MyLastInner().Message;
            }

            return Json(new
            {
                TarihSaat = tarihSaat,
                ParaBirimSimge = paraBirimSimge,
                DovizAlis = dovizAlis,
                DovizSatis = dovizSatis,
                EfektifAlis = efektifAlis,
                EfektifSatis = efektifSatis,
                Durum = durum,
                Mesaj = mesaj
            });
        }

        [HttpPost("AktifKullanici")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequiredAttribute]
        public IActionResult AktifKullanici()
        {
            MoResponse<object> response = new();

            try
            {
                var temBusiness = new TemBusiness(this.dataContext);
                var resim = temBusiness.GetKullaniciResim(this.userToken.UserId);
                var kullaniciYetkiler = temBusiness.GetKullaniciYetkiler(this.userToken.UserId);

                response.Data = new
                {
                    TokenBilgisi = this.userToken,
                    KullaniciResim = resim,
                    KullaniciYetkiler = kullaniciYetkiler
                };

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add(ex.MyLastInner().Message);
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        [HttpPost("KullaniciResimKaydet")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequiredAttribute]
        public IActionResult KullaniciResimKaydet([FromBody] object obj)
        {
            MoResponse<object> response = new();
            try
            {
                dynamic jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(obj.ToString());
                string imageData = jsonResponse.ImageData;

                var temBusiness = new TemBusiness(this.dataContext);

                response = temBusiness.SetKullaniciResim(this.userToken.UserId, imageData, this.userToken.Culture);

            }
            catch (Exception ex)
            {
                MyApp.WriteLogForMethodExceptionMessage(System.Reflection.MethodBase.GetCurrentMethod(), ex);
            }

            return Json(response);
        }

        [HttpPost("GetLookup")]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequiredAttribute]
        public IActionResult GetLookup([FromBody] LookupRequest request)
        {
            Kendo.Mvc.UI.DataSourceResult dsr = new();
            try
            {
                TemBusiness temBusiness = new(this.dataContext);

                dsr = temBusiness.GetLookupRead(this.userToken.Culture, request);
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
                MyApp.WriteLogForMethodExceptionMessage(MethodBase.GetCurrentMethod(), ex);
            }

            return Json(dsr);
        }

    }
}