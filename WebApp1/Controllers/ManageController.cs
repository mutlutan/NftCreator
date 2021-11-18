using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp1.Codes;
using WebApp1.Models;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.IO;
using WebApp1.Areas.Tem.Codes;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;
using Iyzipay.Model.V2.Subscription;

namespace WebApp1.Controllers
{
    public class ManageController : _Controller
    {

        public ManageController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }


        // GET: /<controller>/
        public IActionResult Index()
        {
            if (!this.userToken.UserIsLogon)
            {
                return RedirectToAction("Index", "Account");
            }
            else if (this.userToken.YetkiGrup == EnmYetkiGrup.Musteri)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public decimal GetMemory()
        {
            return Convert.ToInt32(GC.GetTotalMemory(false) / 1000000);
        }
        public IActionResult Collect()
        {
            string beforeMemory = this.GetMemory() + " MB";
            GC.Collect();
            string nowMemory = this.GetMemory() + " MB";

            var obj = new
            {
                BeforeMemory = beforeMemory,
                NowMemory = nowMemory,
            };

            return Json(obj);
        }

        public IActionResult SqlConClearAllPools()
        {
            Microsoft.Data.SqlClient.SqlConnection.ClearAllPools();
            
            var obj = new
            {
                Message=""
            };

            return Json(obj);
        }


    }
}
