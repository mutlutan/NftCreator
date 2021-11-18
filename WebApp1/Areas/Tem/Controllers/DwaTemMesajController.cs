
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
    public class DwaTemMesajController : _Controller
    {
        public DwaTemMesajController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemMesaj.D_R.")]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int? id)
        {
            TreeDataSourceResult dsr = new();
            try
            {
                var query = this.rep.Areas_Tem_RepTemMesaj.Get().Where(c => c.Id > 0);
                if (id == null || id == 0)
                {
                   query = query.Where(c => c.UstId == 0);
                }
                else
                {
                   query = query.Where(c => c.UstId == id);
                }
                dsr = query.ToTreeDataSourceResult(request);
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemMesaj.D_C.")]
        public ActionResult GetByNew()
        {
             return Json(this.rep.Areas_Tem_RepTemMesaj.GetByNew());
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemMesaj.D_C.")]
        public ActionResult Create([FromBody]Areas.Tem.Dto.DtoTemMesaj dto)
        {
            TreeDataSourceResult dsr = new();
            try
            {
                int id = this.rep.Areas_Tem_RepTemMesaj.CreateOrUpdate(dto);
                this.rep.SaveChanges();
                dsr.Data = this.rep.Areas_Tem_RepTemMesaj.GetById(id);

            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemMesaj.D_U.")]
        public ActionResult Update([FromBody]Areas.Tem.Dto.DtoTemMesaj dto)
        {
            TreeDataSourceResult dsr = new();
            try
            {
                int id = this.rep.Areas_Tem_RepTemMesaj.CreateOrUpdate(dto);
                this.rep.SaveChanges();
                dsr.Data = this.rep.Areas_Tem_RepTemMesaj.GetById(id);
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            
            return Json(dsr);
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        [AuthenticateRequired(AuthorityKeys = "TemMesaj.D_D.")]
        public ActionResult Delete(int _id)
        {
            TreeDataSourceResult dsr = new();
            try
            {
                this.rep.Areas_Tem_RepTemMesaj.Delete(_id);
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


