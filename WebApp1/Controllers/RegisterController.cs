using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp1.Codes;

namespace WebApp1.Controllers
{
    public class RegisterController : _Controller
    {
        public RegisterController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0)]
        public ActionResult Save(string lisansKey)
        {
            ViewBag.Message = "";

            try
            {
                //http://localhost/Register/Save?lisansKey=benikaydet
                //bu bölümde sKey ile gelen guid ile lisans sunucudan lisansını alır ve db yok ise açar
                if (lisansKey == "benikaydet")
                {
                    System.Text.StringBuilder LogList = new();
                    LogList.Append(this.dataContext.CreateDatabase());
                    LogList.Append(this.dataContext.CreateTables());

                    ViewBag.Message = LogList.ToString();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex.MyLastInner().Message;
            }

            return View();
        }



    }
}