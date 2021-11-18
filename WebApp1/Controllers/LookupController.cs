using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp1.Codes;
using Dapper;
using Kendo.Mvc.UI;
using WebApp1.Areas.Tem.Codes;
using System.Reflection;

namespace WebApp1.Controllers
{
    public class LookupController : _Controller
    {
        public LookupController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }


        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public ActionResult Read(LookupRequest request)
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

        #region Ulke Sehir Ilce lookup

        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public ActionResult ReadUlkeSehirIlce()
        {
            DataSourceResult dsr = new();
            try
            {
                var queryResult = this.dataContext.TemIlce
                    .OrderBy(o => o.Ad)
                    .Select(s => new { value = s.Id, text = s.Ad.MyToTrim() + " / " + s.Sehir.Ad.MyToTrim() + " / " + s.Sehir.Ulke.Ad.MyToTrim() });

                dsr.Data = queryResult;
                dsr.Total = queryResult.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        #endregion

        #region Kullanıcı Kişi
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public ActionResult ReadKullaniciSahip()
        {
            DataSourceResult dsr = new();
            try
            {
                Areas.Tem.Codes.TemBusiness temBusiness = new(this.dataContext);
                var queryResult = this.dataContext.TemKullanici
                    .OrderBy(o => o.Ad)
                    .Select(s => new { value = s.Id, text = s.Ad + $" <{temBusiness.GetKullaniciSahipAdSahipTur(s.Id)}>" });

                dsr.Data = queryResult;
                dsr.Total = queryResult.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        #endregion


        #region enum listeleri 
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired]
        public ActionResult ReadEnmSahipTur()
        {
            DataSourceResult dsr = new();
            try
            {
                var enumArray = (EnmYetkiGrup[])Enum.GetValues(typeof(EnmYetkiGrup));
                var data = enumArray
                    .Select(s => new { value = (int)s, text = s.ToString() });

                dsr.Data = data;
                dsr.Total = data.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [AuthenticateRequired]
        [ResponseCache(Duration = 0)]
        public ActionResult ReadEnmSahipTurForDescription()
        {
            DataSourceResult dsr = new();
            try
            {
                var enumArray = (EnmYetkiGrup[])Enum.GetValues(typeof(EnmYetkiGrup));
                var data = enumArray
                    .Select(s => new { value = (int)s, text = s.MyGetDescription() });

                dsr.Data = data;
                dsr.Total = data.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }
        #endregion

        #region xxx 

        #endregion
    }
}