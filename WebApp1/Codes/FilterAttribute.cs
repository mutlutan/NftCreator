using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

//using Kendo.Mvc.Extensions;


namespace WebApp1.Codes
{
    public class AuthenticateRequiredAttribute : ActionFilterAttribute
    {
        public string AuthorityKeys { get; set; } = "";
        public string AuthorityGrups { get; set; } = "";
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var svc = actionContext.HttpContext.RequestServices;
            var dataContext = (Models.DataContext)svc.GetService(typeof(Models.DataContext));
            var temBusiness = new Areas.Tem.Codes.TemBusiness(dataContext);

            actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues tokenHeader);
            actionContext.HttpContext.Request.Cookies.TryGetValue("Authorization", out var tokenCooki);
            var userToken = MyApp.ValidateUserToken(dataContext, tokenCooki ?? tokenHeader.ToString());

            if (userToken.UserIsLogon)
            {
                //AuthorityKeys veya AuthorityGrups dan biri doluysa
                if (AuthorityKeys.MyToTrim().Length > 0 || AuthorityGrups.MyToTrim().Length > 0)
                {
                    //AuthorityKeys boş ise yetki key kontrol edilmeyecek anlamındadır
                    Boolean keyGecerli = false;
                    if (AuthorityKeys.MyToTrim().Length > 0)
                    {
                        //Key kontrolü yapılıyor

                        foreach (string key in AuthorityKeys.MyToTrim().Split(","))
                        {
                            if (key.MyToTrim().Length > 0 && temBusiness.UserIsAuthorized(userToken.UserId, key.MyToTrim()))
                            {
                                keyGecerli = true;
                            }
                        }
                    }

                    //AuthorityGrups boş ise yetki grup kontrol edilmeyecek anlamındadır
                    Boolean grupGecerli = false;
                    if (AuthorityGrups.MyToTrim().Length > 0)
                    {
                        //Grup kontrolü yapılıyor
                        foreach (string grup in AuthorityGrups.MyToTrim().Split(","))
                        {
                            if (grup.MyToTrim().Length > 0)
                            {
                                if (grup.MyToTrim() == userToken.YetkiGrup.MyToStr())
                                {
                                    grupGecerli = true;
                                }
                            }
                        }
                    }

                    // grup veya key den biri geçerli değilse mesaj
                    if (!(grupGecerli || keyGecerli))
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
