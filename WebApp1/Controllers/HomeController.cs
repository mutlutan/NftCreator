using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp1.Codes;
using System.Text;
using Iyzipay.Request;
using Iyzipay.Model;


namespace WebApp1.Controllers
{
    public class HomeController : _Controller
    {
        public HomeController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        

        public IActionResult TestDevExtreme()
        {
            return View();
        }


        // Home/Test?abonelikId=25&jeton=d633621d-1566-4fe1-a651-eab04536e70e
        public IActionResult Test()
        {
            #region mail kit deneme
            //string password = "*"; //mail şifresi
            //string user = "mutlutan@outlook.com";
            //var mailMessage = new MimeKit.MimeMessage();
            //mailMessage.From.Add(new MimeKit.MailboxAddress("gönderen hacı", user));
            //mailMessage.To.Add(new MimeKit.MailboxAddress("sana", "mutlutan@outlook.com"));
            //mailMessage.Subject = "subject";
            //mailMessage.Body = new MimeKit.TextPart("plain")
            //{
            //    Text = "Hello 1"
            //};

            //using (var client = new MailKit.Net.Smtp.SmtpClient())
            //{
            //    client.Connect("smtp.live.com", 587, false);
            //    client.Authenticate(user, password);
            //    client.Send(mailMessage);
            //    client.Disconnect(true);
            //}
            #endregion
            //var x = "020D03".MyToDecryptPassword();
            #region mail test
            //new MyMailHelper(this.dataContext).SendMail_Sifre_Bildirim("mutlutan@outlook.com", "123deneme");
            #endregion

            #region translate test
            //var xx = MyApp.TranslateWithBingApiV3("Project", "tr", "en", true);
            #endregion

            return null; // Json(sonuc);
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            if (!this.userToken.UserIsLogon)
            {
                return RedirectToAction("Index", "Account");
            }
            else if(this.userToken.KullaniciSahipTur == EnmSahipTur.Musteri)
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        [HttpPost]
        public IActionResult IletisimGonder(string _kurumAd, string _adSoyad, string _email, string _telefon, string _mesaj, string _sCaptchaCode, string _sCaptchaToken)
        {
            var response = new MoResponse();

            var captchaValid = MyApp.ValidateCaptchaToken(_sCaptchaCode, _sCaptchaToken);

            if (captchaValid)
            {
                try
                {
                    var sonuc = new MyMailHelper(this.dataContext).SendMail_Iletisim_Bildirim(_kurumAd, _adSoyad, _email, _telefon, _mesaj);
                    //rMessage = MyApp.TranslateTo("xLng.MesajinizUlastiEnKisaZamandaIletisimeGecilecektir");
                }
                catch
                {
                    //rMessage = MyApp.TranslateTo("xLng.IsleminizYapilamadiDahaSonraTekrarDeneyebilirsiniz");
                }
                response.Message.Add(MyApp.TranslateTo("xLng.MesajinizUlastiEnKisaZamandaIletisimeGecilecektir", this.dataContext.Language));
            }
            else
            {
                response.Message.Add(MyApp.TranslateTo("xLng.GuvenlikKoduGecersiz", this.dataContext.Language));
            }

            return Json(response);
        }

        [HttpPost]
        public IActionResult EtkinligeKayitOl(string _kurumAd, string _adSoyad, string _email, string _telefon, string _brans, string _sCaptchaCode, string _sCaptchaToken)
        {
            var response = new MoResponse();

            var captchaValid = MyApp.ValidateCaptchaToken(_sCaptchaCode, _sCaptchaToken);

            if (captchaValid)
            {                
                try
                {
                    var sonuc = new MyMailHelper(this.dataContext).SendMail_EtkinlikKayit_Bildirim(_kurumAd, _adSoyad, _email, _telefon, _brans);
                }
                catch { }
                response.Message.Add(MyApp.TranslateTo("xLng.MesajinizUlastiEnKisaZamandaIletisimeGecilecektir", this.dataContext.Language));
            }
            else
            {
                response.Message.Add(MyApp.TranslateTo("xLng.GuvenlikKoduGecersiz", this.dataContext.Language));
            }

            return Json(response);
        }



        #region IyziCo ödeme
        //taksit sorgula
        [HttpPost]
        public IActionResult RetrieveInstallmentInfoRequest(string _kartNumarasi)
        {
            var response = new MoResponse();

            try
            {
                var myIyziCoThreeds = new MyIyziCoThreeds();

                if (_kartNumarasi.Length >= 6)
                {
                    var installmentInfo = myIyziCoThreeds.GetIyzipayInstallmentInfo(this.userToken.Culture, "123456789", _kartNumarasi, 0);

                    //if (installmentInfo.Status.ToLower() == Status.SUCCESS.ToString().ToLower())
                    if (installmentInfo.ErrorCode == null)
                    {
                        response.Error = true;
                        response.Data = installmentInfo.InstallmentDetails;
                    }
                    else
                    {
                        response.Message.Add("(" + installmentInfo.ErrorCode + ") " + installmentInfo.ErrorMessage);
                    }
                }
                else
                {
                    response.Message.Add(MyApp.TranslateTo("xLng.EksikKArtNumarasiYolladiniz", this.dataContext.Language));
                }
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Message.Add(ex.MyLastInner().Message);
            }
            finally
            {
                //this.session.SecurityCode = MyCaptcha.NewText();
            }

            return Json(response);
        }

        //CreateThreedsInitializeRequest
        [HttpPost]
        public IActionResult CreateThreedsInitializeRequest(string _kartAdSoyad, string _kartNumarasi, string _kartAy, string _kartYil, string _kartCVC)
        {
            var response = new MoResponse();

            try
            {
                var myIyziCoThreeds = new MyIyziCoThreeds();

                //var business = new Areas.Tem.Codes.TemBusiness(this.context);
                //var musteriAdSoyad = business.GetKullaniciSahipAd(this.session.UserId);
                //var musterAd = musteriAdSoyad.Split(" ")[0];
                //var musterSoyad = musteriAdSoyad.Replace(musterAd, "").Trim();

                //gerekeni yapınca yukarıyı aç bunu sil
                var musteriAdSoyad = _kartAdSoyad;
                var musterAd = _kartAdSoyad.Split(" ")[0];
                var musterSoyad = _kartAdSoyad.Replace(musterAd, "").Trim();

                var sepet = new List<MoIyziSepet>() { };
                sepet.Add(new MoIyziSepet()
                {
                    Id = 1,
                    Ad = "Ürün1",
                    Kategori = "Kategori1",
                    Tip = BasketItemType.VIRTUAL,
                    Tutar = 1
                });


                var istek = new MoIyziOdemeIstek()
                {
                    Culture = "tr",
                    ConversationId = "123456789",
                    Tutar = 1,
                    TutarKomisyonlu = 1,

                    KartAdSoyad = _kartAdSoyad,
                    KartNumarasi = _kartNumarasi,
                    KartAy = _kartAy,
                    KartYil = "20" + _kartYil,
                    KartCVC = _kartCVC,

                    MusteriId = this.userToken.KullaniciSahipId,
                    MusteriAd = musterAd,
                    MusteriSoyad = musterSoyad,
                    MusteriIP = this.dataContext.IPAddress,

                    TeslimatAdSoyad = musteriAdSoyad,
                    TeslimatAdres = "yoq",
                    TeslimatIlce = "yoq",
                    TeslimatSehir = "yoq",

                    FaturaAdSoyad = musteriAdSoyad,
                    FaturaAdres = "yoq",
                    FaturaIlce = "yoq",
                    FaturaSehir = "yoq",

                    SepetId = 1,
                    Sepet = sepet
                };

                var threedsInitialize = myIyziCoThreeds.GetIyzipayThreedsInitializeCreate(istek);

                if (threedsInitialize.Status.ToLower() == Status.SUCCESS.ToString().ToLower())
                {
                    response.Message.Add(threedsInitialize.Status);
                    response.Data = threedsInitialize;

                }
                else
                {
                    response.Error = true;
                    response.Message.Add("(" + threedsInitialize.ErrorCode + ") " + threedsInitialize.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Message.Add(ex.MyLastInner().Message);
            }
            finally
            {
                //this.session.SecurityCode = MyCaptcha.NewText();
            }

            return Json(response);
        }

        //IyzipayThreedsCallback Sonuç //callback
        [HttpPost]
        public IActionResult IyzipayThreedsCallback(string status, string paymentId, string conversationData, long conversationId, string mdStatus)
        {
            Boolean rError = false;
            var rSb = new StringBuilder();

            try
            {
                var business = new Areas.Tem.Codes.TemBusiness(this.dataContext);

                if (status.ToLower() == "success")
                {
                    rSb.Append(MyApp.TranslateTo("xHome.viewHomeSatinAl.OdemeAlinmistir", this.dataContext.Language));
                    //provizyon tamam ödeme alım ve ürün kayıt işlemi yapılacağı yer
                    //...
                }
                else
                {
                    rSb.Append(MyApp.TranslateTo("xLng.viewHomeSatinAl.mdStatus-" + mdStatus, this.dataContext.Language));
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rSb.Append(ex.MyLastInner().Message);

                MyApp.WriteLogForMethodExceptionMessage(System.Reflection.MethodBase.GetCurrentMethod(), ex);
            }
            finally
            {
                ViewBag.error = rError;
                ViewBag.message = rSb.ToString();
                ViewBag.status = status;
                ViewBag.paymentId = paymentId;
                ViewBag.conversationData = conversationData;
                ViewBag.conversationId = conversationId;
                ViewBag.mdStatus = mdStatus;
            }

            return View();
        }

        #endregion
    }
}
