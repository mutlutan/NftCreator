
//<!-- Auto Generated user1 -->

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebApp1.Codes;
using WebApp1.Controllers;

namespace WebApp1.Areas.Tem.Controllers
{
    [Area("Tem")]
    public class DwaTemParametreController : _Controller
    {
        public DwaTemParametreController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemParametre.D_R.")]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult dsr = new();
            try
            {
                var query = this.rep.Areas_Tem_RepTemParametre.Get().Where(c => c.Id > 0);
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
        [AuthenticateRequired(AuthorityKeys = "TemParametre.D_C.")]
        public ActionResult GetByNew()
        {
             return Json(this.rep.Areas_Tem_RepTemParametre.GetByNew());
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemParametre.D_C.")]
        public ActionResult Create([FromBody]Areas.Tem.Dto.DtoTemParametre dto)
        {
            DataSourceResult dsr = new();
            try
            {
                int id = this.rep.Areas_Tem_RepTemParametre.CreateOrUpdate(dto);
                this.rep.SaveChanges();
                dsr.Data = this.rep.Areas_Tem_RepTemParametre.GetById(id);

            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemParametre.D_U.")]
        public ActionResult Update([FromBody]Areas.Tem.Dto.DtoTemParametre dto)
        {
            DataSourceResult dsr = new();
            try
            {
                int id = this.rep.Areas_Tem_RepTemParametre.CreateOrUpdate(dto);
                this.rep.SaveChanges();
                dsr.Data = this.rep.Areas_Tem_RepTemParametre.GetById(id);
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemParametre.D_D.")]
        public ActionResult Delete(int _id)
        {
            DataSourceResult dsr = new();
            try
            {
                this.rep.Areas_Tem_RepTemParametre.Delete(_id);
                this.rep.SaveChanges();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }


    }
}


