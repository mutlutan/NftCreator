using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApp1.Codes;
using Microsoft.Data.SqlClient;

using DatabaseSchemaReader;
using DatabaseSchemaReader.Compare;

namespace WebApp1.Controllers
{
    public class CompareController : _Controller
    {
        public CompareController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        public IActionResult Index()
        {
            ViewBag.kaynak = MyApp.Configs.DefaultConnectionString;
            ViewBag.hedef = MyApp.Configs.DefaultConnectionString;

            return View();
        }

        public IActionResult Run(string _kaynak, string _hedef)
        {
            Boolean rSonuc = false;
            System.Text.StringBuilder logs = new System.Text.StringBuilder();

            try
            {
                logs = MyApp.CompareDatabase(_kaynak, _hedef);
            }
            catch (Exception ex)
            {
                logs.Append(ex.MyLastInner().Message);
            }

            logs.Append(Environment.NewLine + "İşlem sonu...");

            return Json(new { Sonuc = rSonuc, Mesaj = logs.ToString() });
        }


    }
}