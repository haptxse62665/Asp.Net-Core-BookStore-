using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using ChoMoi.Extensions;
using ChoMoiApi.Extensions;
using DemoAPI.Helper;
using DemoAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChoMoi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _hostingEnvironment = env;
        }

        public static IConfiguration Configuration { get; set; }

        public IHostingEnvironment _hostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomDbContext();

            services.AddCustomizedMvc();

            services.AddSwagger();

            services.AddIdentity();

            services.AddJWT();

            services.AddCustomLocalization();

            services.ConfigureCors();

            services.ConfigureIISIntegration();

            services.ConfigureApiBehaviorOptions();


            services.CustomeCookie();
            /*
             * dependency Injection repositories
             */
            services.RegisterRepositories();

            /*
             * dependency Injection services
             */
            services.RegisterServices();

            /*
			 * Add custom spa static file
			 */
            //services.AddCustomSpaStaticFile(this._hostingEnvironment);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IApplicationLifetime appLifetime, IServiceProvider services, ILoggerFactory loggerFactory)
        {
            if (this._hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(c =>
                c.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()
            );
            var supportedCultures = new[]
            {
                new CultureInfo("en-GB"),
                new CultureInfo("vi-vn")
            };


            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "DefaultApi",
                    template: "api/{controller}/{action}/{id?}"

                );
            });


            // ===== Setup Swagger ======
            app.AddCustomSwagger();


            // ==== Setup Spa Service =====
            //app.AddCustomSpaService(this._hostingEnvironment);

            // ===== Setup log4net =======
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.ConfigureExceptionHandler();

            // ====== SetupIdentity
            app.UseStaticFiles();
            app.UseIdentity();


            // ===== Create tables ======
            //context.Database.Migrate();

            // ===== Create Super Admin ======
            CreateSuperAdminExtendsions.CreateRoles(services).Wait();
        }

    }
}
