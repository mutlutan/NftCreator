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
            else if(this.userToken.YetkiGrup == EnmYetkiGrup.Musteri)
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

    }
}
