//<!-- Auto Generated  21.09.2020 13:28:24 -->
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApp1.Codes;
using WebApp1.Controllers;
using System.Collections.Generic;

namespace WebApp1.Areas.Tem.Controllers
{
    [Area("Tem")]
    public class DwaTemController : _Controller
    {
        public DwaTemController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        #region dashboard
        [AuthenticateRequired]
        public IActionResult GetDashList()
        {
            List<MyDashItem> dashList = new() { };

            dashList.Add(new MyDashItem()
            {
                Id = (int)EnmDashItem.KullaniciSayisi,
                TemplateName = "tn1",
                IconClass = "fa-users",
                IconStyle = "color:red;",
                Title = MyApp.TranslateTo("xLng.viewDashBoard.KullaniciSayisi", this.dataContext.Language),
                RefreshTables = "TemKullanici",
                YetkiGrups = "11,21",
                Url = "#/TemKullanici"
            });

            dashList.Add(new MyDashItem()
            {
                Id = (int)EnmDashItem.ProjeSayisi,
                TemplateName = "tn1",
                IconClass = "fa-child",
                IconStyle = "color:deepskyblue;",
                Title = MyApp.TranslateTo("xLng.viewDashBoard.ProjeSayisi", this.dataContext.Language),
                RefreshTables = "NftProje",
                YetkiGrups = "11,21",
                Url = "#/NftProje"
            });

            dashList = dashList.Where(c => c.YetkiGrups.Contains(((int)this.userToken.YetkiGrup).MyToStr())).ToList();

            return Json(dashList);
        }

        [AuthenticateRequired]
        public IActionResult GetDashData(int _dashId)
        {
            List<MyDashData> dashDataList = new() { };

            var temBusiness = new Areas.Tem.Codes.TemBusiness(this.dataContext);

            if (_dashId == (int)EnmDashItem.KullaniciSayisi)
            {
                //query
                var query = this.dataContext.TemKullanici.Where(c => c.Id > 0);

                //sonuç
                var data = query.ToList();
                var aktifCount = data.Where(c => c.Durum == true).Count();
                var pasifCount = data.Where(c => c.Durum == false).Count();

                dashDataList.Add(new MyDashData()
                {
                    Text = MyApp.TranslateTo("xLng.viewDashBoard.Aktif", this.dataContext.Language),
                    Value1 = aktifCount.ToString("N0"),
                    Value2 = MyApp.TranslateTo("xLng.viewDashBoard.Adet", this.dataContext.Language)
                });
                dashDataList.Add(new MyDashData()
                {
                    Text = MyApp.TranslateTo("xLng.viewDashBoard.Pasif", this.dataContext.Language),
                    Value1 = pasifCount.ToString("N0"),
                    Value2 = MyApp.TranslateTo("xLng.viewDashBoard.Adet", this.dataContext.Language)
                });

            }
            else if (_dashId == (int)EnmDashItem.ProjeSayisi)
            {
                //query
                var query = this.dataContext.NftProje.Where(c => c.Id > 0);

                if(this.userToken.YetkiGrup == EnmYetkiGrup.Musteri)
                {
                    query = query.Where(c => c.KullaniciId == this.userToken.UserId);
                }

                //sonuç
                var data = query.ToList();
                var aktifCount = data.Where(c => c.Durum == true).Count();
                var pasifCount = data.Where(c => c.Durum == false).Count();

                dashDataList.Add(new MyDashData()
                {
                    Text = MyApp.TranslateTo("xLng.viewDashBoard.Aktif", this.dataContext.Language),
                    Value1 = aktifCount.ToString("N0"),
                    Value2 = MyApp.TranslateTo("xLng.viewDashBoard.Adet", this.dataContext.Language)
                });
                dashDataList.Add(new MyDashData()
                {
                    Text = MyApp.TranslateTo("xLng.viewDashBoard.Pasif", this.dataContext.Language),
                    Value1 = pasifCount.ToString("N0"),
                    Value2 = MyApp.TranslateTo("xLng.viewDashBoard.Adet", this.dataContext.Language)
                });

            }
            else
            {
                dashDataList.Add(new MyDashData()
                {
                    Text = "",
                    Value1 = "0",
                    Value2 = ""
                });
            }

            return Json(dashDataList);
        }

        #endregion

        #region Genel
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel,Ogretmen")]
        public IActionResult GetConnectedUserList()
        {
            var temBusiness = new Areas.Tem.Codes.TemBusiness(this.dataContext);

            return Json(temBusiness.GetConnectedUserList());
        }

        #endregion
    }
}


