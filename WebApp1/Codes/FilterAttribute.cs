using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Codes
{
    public class AuthenticateRequiredAttribute : ActionFilterAttribute
    {
        public string AuthorityKeys { get; set; } = "";
        public string AuthorityGrups { get; set; } = "";
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //var svc = actionContext.HttpContext.RequestServices;
            var dataContext = actionContext.HttpContext.GetService<Models.DataContext>();
            var temBusiness = new Areas.Tem.Codes.TemBusiness(dataContext);

            actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues tokenHeader);
            actionContext.HttpContext.Request.Cookies.TryGetValue("Authorization", out var tokenCooki);
            var userToken = MyApp.ValidateUserToken(dataContext, tokenCooki ?? tokenHeader.ToString());

            if (userToken.UserIsLogon)
            {
                //AuthorityKeys boş ise yetki key kontrol edilmeyecek anlamındadır
                if (AuthorityKeys.MyToTrim().Length > 0)
                {
                    //Key kontrolü yapılıyor
                    Boolean keyGecerli = false;
                    foreach (string key in AuthorityKeys.MyToTrim().Split(","))
                    {
                        if (key.MyToTrim().Length > 0 && temBusiness.UserIsAuthorized(userToken.UserId, key.MyToTrim()))
                        {
                            keyGecerli = true;
                        }
                    }

                    if (!keyGecerli)
                    {
                        actionContext.Result = new BadRequestObjectResult(MyApp.TranslateTo("xLng.IslemIcinYetkiGerekli", dataContext.Language));
                    }
                }

                //AuthorityGrups boş ise yetki grup kontrol edilmeyecek anlamındadır
                if (AuthorityGrups.MyToTrim().Length > 0)
                {
                    //Grup kontrolü yapılıyor
                    Boolean grupGecerli = false;
                    foreach (string grup in AuthorityGrups.MyToTrim().Split(","))
                    {
                        if (grup.MyToTrim().Length > 0)
                        {
                            if (grup.MyToTrim() == ((int)userToken.YetkiGrup).MyToStr())
                            {
                                grupGecerli = true;
                            }
                        }
                    }

                    if (!grupGecerli)
                    {
                        actionContext.Result = new BadRequestObjectResult(MyApp.TranslateTo("xLng.IslemIcinYetkiGerekli", dataContext.Language));
                    }
                }


            }
            else
            {
                var result = new ObjectResult("Token geçersiz!") { StatusCode = StatusCodes.Status401Unauthorized };
                actionContext.Result = result;
            }
        }

    }

}
