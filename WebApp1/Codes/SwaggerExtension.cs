using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;


namespace WebApp1.Codes
{
    //Install-Package Swashbuckle.AspNetCore
    public static class SwaggerExtension
    {
        // bu dosya project properties de, BuildAction = content olacak, copy local true olacak
        readonly static string swaggerFileName = "swagger.xml";
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.01",
                    Title = MyApp.AppName + " api",
                    Description = "Serçe Akademi Web Api",
                    TermsOfService = new System.Uri("https://www.serceakademi.com"),
                    Contact = new OpenApiContact
                    {
                        //Name = "Mutlu MUTLUTAN",
                        Email = "mutlutan@outlook.com",
                        Url = new Uri("http://serceakademi.com")
                    }
                });
                //c.DescribeAllEnumsAsStrings();
                //c.DescribeStringEnumsInCamelCase();
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (System.IO.File.Exists(xmlFilePath))
                {
                    System.IO.File.Copy(xmlFilePath, swaggerFileName, true);
                }

                if (System.IO.File.Exists(swaggerFileName))
                {
                    c.IncludeXmlComments(swaggerFileName);
                }
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            //dosya yok ise Swagger çalışmasın diye
            if (System.IO.File.Exists(swaggerFileName))
            {
                string ct = "\"";
                var htmlHeadContent = $@"
                    <div>
                        <div style='position: absolute; left: 300px;'>
                            <a href='#' onclick={ct}$('#AciklamaDetay').toggle('slow');{ct}>Detaylı açıklama için ...</a>
                        </div>
                        <div id='AciklamaDetay' style='display:none; padding:15px;'>
                            <p>
                                <strong>*</strong> 
                                Detaylı açıklamalar buraya yazılır.
                            </p>
                        </div>
                    <div>
                ";

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.DocumentTitle = MyApp.AppName + " Swagger UI";
                    c.HeadContent = htmlHeadContent;
                    c.InjectJavascript("https://code.jquery.com/jquery-1.12.4.min.js");
                });
            }
        }
    }
}
