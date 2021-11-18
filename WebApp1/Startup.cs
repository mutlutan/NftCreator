using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApp1.Codes;
using Microsoft.AspNetCore.Rewrite;
using System.Reflection;

namespace WebApp1
{
    public class Startup
    {
        private IWebHostEnvironment Env { get; set; }
        public IConfigurationRoot Configuration { get; }
        public Startup(IWebHostEnvironment _env)
        {
            this.Env = _env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(this.Env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{this.Env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();

            MyApp.Env = _env;
            MyApp.EnvContentRootPath = this.Env.ContentRootPath;
            MyApp.EnvWebRootPath = this.Env.WebRootPath;
            MyApp.ConfigurationRoot = this.Configuration;
            MyApp.CheckAreas();
            MyApp.LoadDictionary();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Env.IsDevelopment())
            {
                services.AddMvc().AddRazorRuntimeCompilation();
            }
            //Add DbContext, EntityFramework services.
            services.AddDbContext<Models.DataContext>(options => options.UseSqlServer(MyApp.Configs.DefaultConnectionString));

            services.AddBrowserDetection(); //Install-Package Shyjus.BrowserDetector -Version 1.0.8

            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.ValueCountLimit = 2048;
                options.ValueLengthLimit = 52428800 * 2; //100MB
                //options.MultipartBodyLengthLimit = long.MaxValue; // if don't set default value is: 128 MB
                //options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddControllers();

            services.AddRazorPages(options =>
            {
                //...
            })
            .AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            })
            .AddMvcOptions(options =>
            {
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            #region Swagger
            services.AddSwagger();
            #endregion

            #region Health Checks
            //services.AddHealthChecks();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            //loggerFactory
            //var loggerFactory = (Microsoft.Extensions.Logging.ILoggerFactory)serviceProvider.GetService(typeof(Microsoft.Extensions.Logging.ILoggerFactory));
            //loggerFactory.AddConsole();
            //loggerFactory.AddProvider(new MyLoggerProvider());
            //dataContext
            //var dataContext = (Models.DataContext)serviceProvider.GetService(typeof(Models.DataContext));

            if (this.Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();//geçici

                #region https için
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttps().AddRedirectToWww()); //sub domainler çalışmayabilir
                //veya ...
                app.UseHsts();
                app.UseHttpsRedirection();
                #endregion
            }

            #region Use DefaultFiles/StaticFiles/FileServer Files
            // wwwroot dizini içindeki index.html default olarak açması için
            app.UseDefaultFiles();
            #endregion

            #region mime-type eklemek için
            // Add new mappings // mime-type eklemek için
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            provider.Mappings[".ino"] = "application/x-msdownload";
            provider.Mappings[".mblock"] = "application/x-msdownload";
            provider.Mappings[".stl"] = "application/x-msdownload";
            provider.Mappings[".py"] = "application/x-msdownload";
            provider.Mappings[".ev3"] = "application/x-msdownload";
            provider.Mappings[".sldprt"] = "application/x-msdownload";
            provider.Mappings[".sldasm"] = "application/x-msdownload";
            provider.Mappings[".slddrw"] = "application/x-msdownload";
            provider.Mappings[".slddrt"] = "application/x-msdownload";
            provider.Mappings[".asm"] = "application/x-msdownload";
            provider.Mappings[".drw"] = "application/x-msdownload";
            provider.Mappings[".sb3"] = "application/x-msdownload";
            provider.Mappings[".fzz"] = "application/x-msdownload";
            provider.Mappings[".hex"] = "application/x-msdownload";
            provider.Mappings[".apk"] = "application/x-msdownload";
            provider.Mappings[".sb"] = "application/x-msdownload";

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider,
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(30)
                    };
                }
            });

            #endregion

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #region Swagger
            app.UseCustomSwagger();
            #endregion

            #region Application...
            var applicationLifetime = (IHostApplicationLifetime)serviceProvider.GetService(typeof(IHostApplicationLifetime));
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                MyApp.ApplicationStartedDateTime = DateTime.Now;
                MyApp.WriteLog(EnmLogTur.Genel, Environment.NewLine + "...................");
                MyApp.WriteLogForMethod(MethodBase.GetCurrentMethod(), EnmLogTur.Genel, "Application started => " + DateTime.Now.ToString());

                MyJobs.MyTimer = new System.Threading.Timer(
                    new System.Threading.TimerCallback(MyJobs.Gorevler),
                    MyApp.Configs, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(1)
                );
            });
            #endregion

            #region HealthChecks
            //app.UseHealthChecks("/hc");
            #endregion

        }
    }
}
