using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp1.Codes;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace WebApp1.Controllers
{
    public class UpdateController : _Controller
    {
        public UpdateController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0)]
        public ActionResult Apply(string updateToken)
        {
            string resultMessage = "";

            try
            {
                //http://localhost:5002/Update/Apply?updateToken=UPDATE-TOKEN-T01
                //bu bölümde sKey ile gelen doğru ise  veritabanını günceller
                if (updateToken == MyApp.Configs.UpdateToken)
                {
                    resultMessage = this.dataContext.RunDatabaseUpdate();
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.MyLastInner().Message;
            }

            ViewBag.Message = resultMessage;

            return View();
        }

    }
}