using Api.Models;
using Api.Repositories;
using ChoMoi;
using ChoMoi.Api.Services.Implement;
using ChoMoi.Extensions;
using DemoAPI.Helper;
using DemoAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChoMoiApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddCustomSpaStaticFile(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
        //{
        //    if (!Startup.Configuration.GetValue<bool>("NotAllowSpaService"))
        //    {
        //        if (hostingEnvironment.IsDevelopment())
        //        {
        //            services.AddSpaStaticFiles(c =>
        //            {
        //                c.RootPath = "ClientApp/dist";
        //            });
        //        }
        //        else
        //        {
        //            services.AddSpaStaticFiles(c =>
        //            {
        //                c.RootPath = "ClientApp";
        //            });
        //        }
        //    }

        //    return services;
        //}

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services)
        {
            services.AddDbContext<BookStoreContext>(options =>
                    options.UseLazyLoadingProxies()
                    .UseSqlServer(Startup.Configuration.GetConnectionString("BookStoreDB")));

            return services;
        }



        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com", Url = "www.talkingdotnet.com" }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                    });
            });


            return services;
        }
        public static IServiceCollection AddJWT(this IServiceCollection services)
        {
            //JWT
            var appSettingsSection = Startup.Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            //and this: add identity and create the db
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreContext>()
                .AddDefaultTokenProviders();

            //cut down alternative
            services.AddIdentityCore<User>(options => { });
            new IdentityBuilder(typeof(User), typeof(IdentityRole), services)
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<BookStoreContext>();

            //Condition
            services.Configure<IdentityOptions>(o => {
                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = false;
            });

            return services;
        }

        public static IServiceCollection CustomeCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(0);
                options.SlidingExpiration = true;
            });

            return services;
        }

        private static string GetXmlCommentsPath()
        {
            var app = System.AppContext.BaseDirectory;
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
        }

        public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
        {
            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    })
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return services;
        }

        public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                                new CultureInfo("en-US"),
                                new CultureInfo("vi-VN")
                    };

                opts.DefaultRequestCulture = new RequestCulture("vi-VN");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(options => options.ResourcesPath = "Localization");

            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UserService, UserService>();


            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services;
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }
        public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });
        }

    }
}
