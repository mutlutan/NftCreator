
//<!-- Auto Generated  18.09.2018 14:35:34 -->

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebApp1.Codes;
using WebApp1.Controllers;
using System.Collections.Generic;

namespace WebApp1.Areas.Tem.Controllers
{
    [Area("Tem")]
    public class DwaTemKullaniciController : _Controller
    {
        public DwaTemKullaniciController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequiredAttribute(AuthorityKeys = "TemKullanici.D_R.")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult dsr = new();
            try
            {
                var query = this.rep.Areas_Tem_RepTemKullanici.Get().Where(c => c.Id > 0);

                //kullanýcý admin deðil ise adminleri göremez
                if (this.userToken.YetkiGrup != EnmYetkiGrup.Admin)
                {
                    query = query.Where(c => c.Rols.Contains("1001") == false);
                }

                dsr = query.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemKullanici.D_C.")]
        public ActionResult GetByNew()
        {
            return Json(this.rep.Areas_Tem_RepTemKullanici.GetByNew());
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemKullanici.D_C.")]
        public ActionResult Create([FromBody] Areas.Tem.Dto.DtoTemKullanici dto)
        {
            DataSourceResult dsr = new();
            try
            {
                //admin deðil ise, rol ekleyemez
                if (this.userToken.YetkiGrup != EnmYetkiGrup.Admin)
                {
                    dto.Rols = "";
                }

                int id = this.rep.Areas_Tem_RepTemKullanici.CreateOrUpdate(dto);
                this.rep.SaveChanges();
                dsr.Data = this.rep.Areas_Tem_RepTemKullanici.GetById(id);

                #region cache için
                MyCache.TemKullaniciListLoad(this.rep);
                #endregion
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemKullanici.D_U.")]
        public ActionResult Update([FromBody] Areas.Tem.Dto.DtoTemKullanici dto)
        {
            DataSourceResult dsr = new();
            try
            {
                int id = this.rep.Areas_Tem_RepTemKullanici.CreateOrUpdate(dto);
                this.rep.SaveChanges();
                dsr.Data = this.rep.Areas_Tem_RepTemKullanici.GetById(id);

                #region cache için
                MyCache.TemKullaniciListLoad(this.rep);
                #endregion
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemKullanici.D_D.")]
        public ActionResult Delete(int _id)
        {
            DataSourceResult dsr = new();
            try
            {
                this.rep.Areas_Tem_RepTemKullanici.Delete(_id);
                this.rep.SaveChanges();

                #region cache için
                MyCache.TemKullaniciListLoad(this.rep);
                #endregion
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemKullanici.A_ResetPassword.")]
        public ActionResult ResetPasswordSifre(int _id)
        {
            string rMessage = string.Empty;
            Boolean rOk = false;
            try
            {
                var dtoKullanici = this.rep.Areas_Tem_RepTemKullanici.GetById(_id).FirstOrDefault();

                if (dtoKullanici != null)
                {
                    string mailAdres = dtoKullanici.Ad;
                    if (mailAdres.IsValidEmail())
                    {
                        dtoKullanici.Sifre = Guid.NewGuid().ToString().MyToUpper().Replace("-", "").Substring(0, 8);
                        using (var mailHelper = new MyMailHelper(this.dataContext))
                        {
                            mailHelper.SendMail_Sifre_Bildirim(mailAdres, dtoKullanici.Sifre);
                        }

                        int id = this.rep.Areas_Tem_RepTemKullanici.CreateOrUpdate(dtoKullanici);
                        this.rep.SaveChanges();
                        rMessage += MyApp.TranslateTo("xLng.YeniSifreKayitliMailAdresineGonderildi", this.dataContext.Language);
                        rOk = true;
                    }
                    else
                    {
                        rMessage += MyApp.TranslateTo("xLng.LutfenGecerliBirMailAdresGiriniz", this.dataContext.Language);
                    }
                }
                else
                {
                    rMessage += MyApp.TranslateTo("xLng.IslemYapilacakKayitBulunamadi", this.dataContext.Language);
                }
            }
            catch (Exception ex)
            {
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { Message = rMessage, Ok = rOk });
        }

    }
}


