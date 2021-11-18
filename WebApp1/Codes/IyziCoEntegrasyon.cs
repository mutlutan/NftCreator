using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Codes
{
    // https://dev.iyzipay.com/tr/test-kartlari

    #region Iyzico Threeds Models (Normail işlemlerdeki models)
    public class MoIyziSepet
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Kategori { get; set; }
        public Iyzipay.Model.BasketItemType Tip { get; set; }
        public decimal Tutar { get; set; }
    }

    public class MoIyziOdemeIstek
    {
        public string Culture { get; set; }
        public string ConversationId { get; set; }
        public decimal Tutar { get; set; }
        public decimal TutarKomisyonlu { get; set; }

        public string KartAdSoyad { get; set; }
        public string KartNumarasi { get; set; }
        public string KartAy { get; set; }
        public string KartYil { get; set; }
        public string KartCVC { get; set; }

        public int MusteriId { get; set; }
        public string MusteriAd { get; set; }
        public string MusteriSoyad { get; set; }
        public string MusteriIP { get; set; }

        public string TeslimatAdSoyad { get; set; }
        public string TeslimatAdres { get; set; }
        public string TeslimatIlce { get; set; }
        public string TeslimatSehir { get; set; }

        public string FaturaAdSoyad { get; set; }
        public string FaturaAdres { get; set; }
        public string FaturaIlce { get; set; }
        public string FaturaSehir { get; set; }

        public int SepetId { get; set; }
        public List<MoIyziSepet> Sepet { get; set; } = new List<MoIyziSepet>();
    }

    public class MyIyziCoThreeds
    {
        private Iyzipay.Options GetIyzipayOptions()
        {
            //test veya canlı olmasında değişmesi gereken yer
            Iyzipay.Options options = new()
            {
                BaseUrl = "https://sandbox-api.iyzipay.com",
                ApiKey = "sandbox-afXhZPW0MQlE4dCUUlHcEopnMBgXnAZI",
                SecretKey = "sandbox-wbwpzKIiplZxI3hh5ALI4FJyAcZKL6kq"
            };

            return options;
        }

        private Iyzipay.Request.CreatePaymentRequest GetPaymentRequest(MoIyziOdemeIstek model)
        {
            var request = new Iyzipay.Request.CreatePaymentRequest()
            {
                Locale = model.Culture,
                ConversationId = model.ConversationId,
                Price = model.Tutar.ToString(),
                PaidPrice = model.TutarKomisyonlu.ToString(),
                Currency = Iyzipay.Model.Currency.TRY.ToString(),
                Installment = 1,
                BasketId = model.SepetId.ToString(),
                PaymentChannel = Iyzipay.Model.PaymentChannel.WEB.ToString(),
                PaymentGroup = Iyzipay.Model.PaymentGroup.SUBSCRIPTION.ToString()
            };

            request.PaymentCard = new Iyzipay.Model.PaymentCard()
            {
                CardHolderName = model.KartAdSoyad,
                CardNumber = model.KartNumarasi,
                ExpireMonth = model.KartAy, //12
                ExpireYear = model.KartYil, //"2030";
                Cvc = model.KartCVC,
                RegisterCard = 0
            };

            request.Buyer = new Iyzipay.Model.Buyer()
            {
                Id = model.MusteriId.ToString(),
                Name = model.MusteriAd,
                Surname = model.MusteriSoyad,
                Email = "email@email.com",
                IdentityNumber = "00000",
                RegistrationAddress = "yoq",
                Ip = model.MusteriIP,
                City = "yoq",
                Country = "yoq"
            };

            request.ShippingAddress = new Iyzipay.Model.Address()
            {
                ContactName = model.TeslimatAdSoyad,
                Description = model.TeslimatAdres,
                City = model.TeslimatIlce,
                Country = model.TeslimatSehir
            };

            request.BillingAddress = new Iyzipay.Model.Address()
            {
                ContactName = model.FaturaAdSoyad,
                Description = model.FaturaAdres,
                City = model.FaturaIlce,
                Country = model.FaturaSehir
            };

            request.BasketItems = new List<Iyzipay.Model.BasketItem>();
            foreach (var basketItem in model.Sepet)
            {
                request.BasketItems.Add(new Iyzipay.Model.BasketItem()
                {
                    Id = basketItem.Id.ToString(),
                    Name = basketItem.Ad,
                    Category1 = basketItem.Kategori,
                    Price = basketItem.Tutar.ToString(),
                    ItemType = basketItem.Tip.ToString()
                });
            }

            return request;
        }

        public Iyzipay.Model.InstallmentInfo GetIyzipayInstallmentInfo(string culture, string conversationId, string binNumber, decimal price)
        {
            Iyzipay.Request.RetrieveInstallmentInfoRequest request = new()
            {
                Locale = culture,
                ConversationId = conversationId,
                BinNumber = binNumber,
                Price = price.ToString()
            };

            var installmentInfo = Iyzipay.Model.InstallmentInfo.Retrieve(request, this.GetIyzipayOptions());

            return installmentInfo;
        }

        public Iyzipay.Model.ThreedsInitialize GetIyzipayThreedsInitializeCreate(MoIyziOdemeIstek model)
        {
            var request = this.GetPaymentRequest(model);
            //request.CallbackUrl = "https://www.serceakademi.com/Home/IyzipayThreedsCallback"; // bu bilgiyi veritabanına koy
            request.CallbackUrl = "http://localhost:5002/Home/IyzipayThreedsCallback"; // bu bilgiyi veritabanına koy

            Iyzipay.Model.ThreedsInitialize threedsInitialize = Iyzipay.Model.ThreedsInitialize.Create(request, this.GetIyzipayOptions());

            return threedsInitialize;
        }
    }
    #endregion

    #region Iyzico Subscription 
    public enum EnmIyzicoAbonelikDonem
    {
        Gunluk = 11,
        Haftalik = 12,
        Aylik = 13,
        Yillik = 14
    }

    public enum EnmIyzicoAbonelikDurum
    {
        ACTIVE = 1,
        PENDING = 2,
        UNPAID = 3,
        UPGRADED = 4,
        CANCELED = 5,
        EXPIRED = 6
    }

    public class MoIyzicoCheckoutForm
    {
        public int ConversationId { get; set; }
        public string MailAdres { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        //public string Sehir { get; set; }
        //public string Ilce { get; set; }
        //public string Adres { get; set; }
        public string FiyatPlanReferansKod { get; set; }
    }

    public class MyIyziCoSubscription
    {
        private readonly Models.DataContext Context;

        private Iyzipay.Options Options { get; set; }
        private string CallbackUrl { get; set; }

        public MyIyziCoSubscription(Models.DataContext dataContext)
        {
            this.Context = dataContext;
            var parametre = this.Context.TemParametre.FirstOrDefault();
            this.CallbackUrl = parametre.HostAddress + "/Ogrenci/IyzicoCheckoutFormCallback";
            this.Options = new Iyzipay.Options()
            {
                BaseUrl = parametre.IyzicoBaseUrl,      //"https://sandbox-api.iyzipay.com"   //canlı"https://api.iyzipay.com"
                ApiKey = parametre.IyzicoApiKey,        //"sandbox-afXhZPW0MQlE4dCUUlHcEopnMBgXnAZI",
                SecretKey = parametre.IyzicoSecretKey   //"sandbox-wbwpzKIiplZxI3hh5ALI4FJyAcZKL6kq"
            };
        }

        public CheckoutFormResource InitializeCheckoutForm(MoIyzicoCheckoutForm model)
        {
            var request = new InitializeCheckoutFormRequest
            {
                Locale = Iyzipay.Model.Locale.TR.ToString(),
                Customer = new CheckoutFormCustomer
                {
                    Email = model.MailAdres,
                    Name = model.Ad,
                    Surname = model.Soyad,
                    BillingAddress = new Iyzipay.Model.Address
                    {
                        City = "ANKARA",
                        Country = "Türkiye",
                        Description = "billing-address-description",
                        ContactName = "billing-contact-name",
                        ZipCode = "010101"
                    },
                    ShippingAddress = new Iyzipay.Model.Address
                    {
                        City = "ANKARA",
                        Country = "Türkiye",
                        Description = "shipping-address-description",
                        ContactName = "shipping-contact-name",
                        ZipCode = "010102"
                    },
                    GsmNumber = "+905350000000",
                    IdentityNumber = "55555555555",
                },
                CallbackUrl = this.CallbackUrl,
                ConversationId = model.ConversationId.ToString(),
                PricingPlanReferenceCode = model.FiyatPlanReferansKod,
                SubscriptionInitialStatus = SubscriptionStatus.ACTIVE.ToString()
            };

            CheckoutFormResource response = Subscription.InitializeCheckoutForm(request, this.Options);

            return response;
        }

        public Iyzipay.Model.CheckoutForm CheckoutFormRetrieve(string conversationId, string token)
        {
            var request = new Iyzipay.Request.RetrieveCheckoutFormRequest()
            {
                ConversationId = conversationId,
                Token = token
            };

            var checkoutForm = Iyzipay.Model.CheckoutForm.Retrieve(request, this.Options);

            return checkoutForm;
        }

        public Iyzipay.Model.V2.ResponseData<SubscriptionResource> SubscriptionRetrieve(string conversationId, string subscriptionReferenceCode)
        {
            var request = new RetrieveSubscriptionRequest
            {
                Locale = Iyzipay.Model.Locale.TR.ToString(),
                ConversationId = conversationId,

                SubscriptionReferenceCode = subscriptionReferenceCode

            };

            var sonuc = Subscription.Retrieve(request, this.Options);

            return sonuc;
        }

        public Iyzipay.Model.V2.ResponseData<CustomerResource> CustomerRetrieve(string conversationId, string customerReferenceCode)
        {
            var request = new RetrieveCustomerRequest
            {
                Locale = Iyzipay.Model.Locale.TR.ToString(),
                ConversationId = conversationId,
                CustomerReferenceCode = customerReferenceCode
            };

            var sonuc = Customer.Retrieve(request, this.Options);

            return sonuc;
        }

        public Iyzipay.Model.V2.ResponsePagingData<CustomerResource> CustomerRetrieveAll(string conversationId)
        {
            var request = new Iyzipay.PagingRequest()
            {
                Locale = Iyzipay.Model.Locale.TR.ToString(),
                ConversationId = conversationId,
                Count = 10,
                Page = 1
            };

            var sonuc = Customer.RetrieveAll(request, this.Options);

            return sonuc;
        }

        public Iyzipay.Model.V2.ResponsePagingData<SubscriptionResource> SubscriptionSearch(string conversationId, string subscriptionReferenceCode)
        {
            var request = new SearchSubscriptionRequest
            {
                Locale = Iyzipay.Model.Locale.TR.ToString(),
                ConversationId = conversationId,
                Page = 1,
                Count = 10,
                SubscriptionStatus = SubscriptionStatus.ACTIVE.ToString(),
                SubscriptionReferenceCode = subscriptionReferenceCode
            };

            var sonuc = Subscription.Search(request, this.Options);

            return sonuc;
        }


    }
    #endregion

}
