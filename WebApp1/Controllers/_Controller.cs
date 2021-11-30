
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using WebApp1.Codes;
using Shyjus.BrowserDetection;
using Microsoft.Extensions.Caching.Memory;

namespace WebApp1.Controllers
{
#pragma warning disable IDE1006

    public class _Controller : Controller
    {
        public IServiceProvider serviceProvider;
        public IHttpContextAccessor accessor;
        public IMemoryCache memoryCache;

        public IBrowserDetector browserDetector;
        public Models.DataContext dataContext;
        public Models._Rep rep = null;
        public MoUserToken userToken = new();

        public _Controller(IServiceProvider _serviceProvider)
        {
            this.serviceProvider = _serviceProvider;

            this.accessor = (IHttpContextAccessor)this.serviceProvider.GetService(typeof(IHttpContextAccessor));
            this.dataContext = (Models.DataContext)this.serviceProvider.GetService(typeof(Models.DataContext));

            this.accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues tokenHeader);
            this.accessor.HttpContext.Request.Cookies.TryGetValue("Authorization", out var tokenCooki);
            this.userToken = MyApp.ValidateUserToken(this.dataContext, tokenCooki ?? tokenHeader.ToString());
            this.userToken.Host = accessor.HttpContext.Request.Host.Host;

            //Memory Cache 
            //burdan bakıp cachi kullan//https://medium.com/@sefikcankanber/asp-net-core-cache-kullan%C4%B1mlar%C4%B1-in-memory-cache-kullan%C4%B1m%C4%B1-34d54d91d3ce
            this.memoryCache = (IMemoryCache)this.serviceProvider.GetService(typeof(IMemoryCache));

            //browserDetector
            this.browserDetector = (IBrowserDetector)this.serviceProvider.GetService(typeof(IBrowserDetector));

            //var requestCulture = accessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(this.userToken.Culture);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(this.userToken.Culture);

            this.dataContext.SetConnectionString(MyApp.Configs, new CultureInfo(this.userToken.Culture));
            this.dataContext.IPAddress = this.accessor.HttpContext.Connection.RemoteIpAddress == null ? "" : accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            this.dataContext.UserId = this.userToken.UserId;
            this.dataContext.UserName = this.userToken.UserName;

            this.rep = new Models._Rep(this.dataContext);

            #region Cache işlemleri
            try
            {
                MyCache.RefreshAll(this.rep);
            }
            catch { }
            #endregion
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            //ortak viewbag ler
            ViewBag.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();

            ViewBag.Culture = this.userToken.Culture;
            ViewBag.Language = CultureInfo.GetCultureInfo(ViewBag.Culture).Parent.IetfLanguageTag;
            ViewBag.Title = MyApp.AppName;

            ViewBag.LogoImageUrl = "/img/logo/logoYatay.png?v" + WebApp1.Codes.MyApp.Version;

            base.OnActionExecuting(context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Microsoft.Data.SqlClient.SqlConnection.ClearPool((Microsoft.Data.SqlClient.SqlConnection)this.dataContext.GetDbConnection());
                //------------------------
                if (this.dataContext != null)
                {
                    this.dataContext.Dispose();
                    this.dataContext = null;
                }

                //-------------------------
                //GC.Collect();
            }
        }

    }

#pragma warning restore IDE1006
}
