using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoMoi;

namespace ChoMoiApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        //public static IApplicationBuilder AddCustomSpaService(this IApplicationBuilder app, IHostingEnvironment hostingEnvironment)
        //{
        //    if (!Startup.Configuration.GetValue<bool>("NotAllowSpaService"))
        //    {
        //        app.UseSpaStaticFiles();

        //        app.UseSpa(spa =>
        //        {
        //            // To learn more about options for serving an Angular SPA from ASP.NET Core,
        //            // see https://go.microsoft.com/fwlink/?linkid=864501

        //            spa.Options.SourcePath = "ClientApp";

        //            if (hostingEnvironment.IsDevelopment())
        //            {
        //                spa.UseAngularCliServer(npmScript: "start");
        //            }
        //        });
        //    }

        //    return app;
        //}

        public static IApplicationBuilder AddCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            return app;
        }
    }
}
