using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

namespace WebApp1.Codes
{
    //gmail göndeririken hata alınıyor olabilir, Google hesabınızın dışarıdan harici bir uygulama tarafından kullanılmasına izin verilmiyor olmasıdır.
    //aşağıdaki linke girip devam et demek yeterli
    // https://accounts.google.com/DisplayUnlockCaptcha

    public class MyMailAccount
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public Boolean EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class MyMailHelper : IDisposable
    {
        private readonly Models.DataContext dataContext;
        private readonly Models.TemParametre parametre;

        public MyMailHelper(Models.DataContext _dataContext)
        {
            this.dataContext = _dataContext;
            this.parametre = this.dataContext.TemParametre.FirstOrDefault();
        }

        public MyMailAccount GetDefaultAccount()
        {
            return new MyMailAccount()
            {
                Host = this.parametre.MailHost,
                Port = this.parametre.MailPort,
                EnableSsl = this.parametre.MailEnableSsl,
                UserName = this.parametre.MailUserName,
                Password = parametre.MailPassword.MyFromBase64Str()
            };
        }

        public MimeKit.MimeMessage MesajOlustur(string _fromAdress, string _fromDisplayName, List<string> _to, List<string> _cc, List<string> _bcc, string _subject, string _body)
        {
            var message = new MimeKit.MimeMessage() { };

            message.From.Add(new MimeKit.MailboxAddress(_fromDisplayName, _fromAdress));

            if (_to != null)
            {
                foreach (var item in _to)
                {
                    foreach (var adres in item.MyToTrim().Split(new char[] { ',', ';' }))
                    {
                        if (adres.MyToTrim().Length > 0)
                        {
                            message.To.Add(new MimeKit.MailboxAddress(adres, adres));
                        }
                    }
                }
            }

            if (_cc != null)
            {
                foreach (var item in _cc)
                {
                    foreach (var adres in item.MyToTrim().Split(new char[] { ',', ';' }))
                    {
                        if (adres.MyToTrim().Length > 0)
                        {
                            message.Cc.Add(new MimeKit.MailboxAddress(adres, adres));
                        }
                    }
                }
            }

            if (_bcc != null)
            {
                foreach (var item in _bcc)
                {
                    foreach (var adres in item.MyToTrim().Split(new char[] { ',', ';' }))
                    {
                        if (adres.MyToTrim().Length > 0)
                        {
                            message.Bcc.Add(new MimeKit.MailboxAddress(adres, adres));
                        }
                    }
                }
            }

            message.Subject = _subject;
            message.Body = new MimeKit.BodyBuilder()
            {
                HtmlBody = _body
            }.ToMessageBody();

            return message;
        }

        public Boolean SendMailWithMailKit(MimeKit.MimeMessage _message, MyMailAccount _mailAccount)
        {
            var smtpClient = new MailKit.Net.Smtp.SmtpClient();

            //Host; smtp.yandex.com smtp-mail.outlook.com yandex.com
            //Port;  outlook.com:587 ssl false // gmail port : 465 ssl true
            smtpClient.Connect(_mailAccount.Host, _mailAccount.Port, _mailAccount.EnableSsl);
            smtpClient.Authenticate(_mailAccount.UserName, _mailAccount.Password);
            smtpClient.Send(_message);
            smtpClient.Disconnect(true);

            return true;
        }

        #region Sablon gönderici
        public Boolean SendMailForSablon(int _sablonId, List<string> _toMailList, Dictionary<string, string> _data)
        {
            Boolean rTF = false;

            _data.Add("[#DateTime.Today#]", DateTime.Today.Date.ToString("d"));
            _data.Add("[#DateTime.Now#]", DateTime.Now.ToString("g"));

            var postaSablon = this.dataContext.TemMailSablon
                .Where(c => c.Id == _sablonId)
                .Include(i => i.Antet)
                .FirstOrDefault();

            string antetIcerik = postaSablon.Antet.Icerik.MyToTrim();
            string mailsubject = postaSablon.Konu.MyToStr() + " " + DateTime.Now.ToString("g");
            string mailBody = postaSablon.Icerik.MyToStr();

            //body replace
            foreach (var item in _data)
            {
                antetIcerik = antetIcerik.Replace(item.Key, item.Value.MyToStr());
                mailsubject = mailsubject.Replace(item.Key, item.Value.MyToStr());
                mailBody = mailBody.Replace(item.Key, item.Value.MyToStr());
            }

            // antet geçişi
            if (antetIcerik.Length > 0 && antetIcerik.Contains("[#AntetContent#]"))
            {
                mailBody = antetIcerik.Replace("[#AntetContent#]", mailBody);
            }

            //log ekle
            Models.TemMailHareket modelMailHareket = new() { };
            modelMailHareket.Id = (int)this.dataContext.GetNextSequenceValue("sqTemMailHareket");
            modelMailHareket.KayitZaman = DateTime.Now;
            modelMailHareket.SablonId = _sablonId;
            modelMailHareket.DurumId = 1; //0:Gonderilecek, 1:göderiliyor, 2:gönderildi, 3:Hata
            modelMailHareket.DenemeSayisi = 1;
            modelMailHareket.SonDenemeZaman = DateTime.Now;
            modelMailHareket.Adres = string.Join(",", _toMailList);
            modelMailHareket.Kopya = postaSablon.Kopya;
            modelMailHareket.Gizli = postaSablon.Gizli;
            modelMailHareket.Konu = mailsubject;
            modelMailHareket.Icerik = mailBody;
            this.dataContext.Add(modelMailHareket);
            this.dataContext.SaveChanges();

            //gönder
            try
            {
                //mesaj oluştur
                var mesaj = MesajOlustur(
                    parametre.MailUserName,
                    parametre.KurumAd,
                    _toMailList,
                    postaSablon.Kopya?.Split(",").ToList(),
                    postaSablon.Gizli?.Split(",").ToList(),
                    mailsubject,
                    mailBody
                );

                rTF = this.SendMailWithMailKit(mesaj, GetDefaultAccount());

                //log update
                modelMailHareket.SonDenemeZaman = DateTime.Now;
                modelMailHareket.DurumId = 2;  //0:Gonderilecek, 1:göderiliyor, 2:gönderildi, 3:Hata
                this.dataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //log update
                modelMailHareket.SonDenemeZaman = DateTime.Now;
                modelMailHareket.Aciklama = ex.Message;
                modelMailHareket.DurumId = 3; //0:Gonderilecek, 1:göderiliyor, 2:gönderildi, 3:Hata
                this.dataContext.SaveChanges();

                // Exception devam etsin yola...
                throw new Exception(ex.MyLastInner().Message, ex);
            }

            return rTF;
        }
        #endregion

        #region areas gönderimler

        public Boolean SendMail_Sifre_Bildirim(string kullaniciMailAdres, string yeniSifre)
        {
            Dictionary<string, string> data = new() { };
            data.Add("[#Kullanici_Ad#]", kullaniciMailAdres.MyToStr());
            data.Add("[#Kullanici_Sifre#]", yeniSifre);

            var to = new List<string>() { kullaniciMailAdres.MyToStr() };
            return this.SendMailForSablon((int)EnmMailSablon.SifreBildirim, to, data);
        }

        public Boolean SendMail_Iletisim_Bildirim(string _kurumAd, string _adSoyad, string _email, string _telefon, string _mesaj)
        {
            Dictionary<string, string> data = new() { };
            data.Add("[#Ziyaretci_KurumAd#]", _kurumAd);
            data.Add("[#Ziyaretci_AdSoyad#]", _adSoyad);
            data.Add("[#Ziyaretci_Mail#]", _email);
            data.Add("[#Ziyaretci_Telefon#]", _telefon);
            data.Add("[#Ziyaretci_Mesaj#]", _mesaj);

            var to = new List<string>() { this.parametre.KurumMail.MyToStr() };
            return this.SendMailForSablon((int)EnmMailSablon.IletisimBildirim, to, data);
        }

        public Boolean SendMail_UyeMailOnay(string mail, string adSoyad, string onayLinkValue)
        {
            Dictionary<string, string> data = new() { };
            data.Add("[#AdSoyad#]", adSoyad);
            data.Add("[#OnayLinkHostAddress#]", this.parametre.HostAddress);
            data.Add("[#OnayLinkValue#]", onayLinkValue);
            data.Add("[#OnayLinkText#]", MyApp.TranslateTo("xLng.UyeliginiziOnaylayin", this.dataContext.Language));

            var to = new List<string>() { mail };
            return this.SendMailForSablon((int)EnmMailSablon.UyeOnayBildirim, to, data);
        }
        #endregion



        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
