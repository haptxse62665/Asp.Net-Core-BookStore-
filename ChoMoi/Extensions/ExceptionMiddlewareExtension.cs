using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChoMoiApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExceptionMiddlewareExtensions));
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        log.Error(message: GetExceptionString(context, contextFeature));
                    }
                });
            });
        }

        private static string GetExceptionString(HttpContext context, IExceptionHandlerFeature exceptionHandler)
        {
            Exception ex = exceptionHandler.Error;
            StringBuilder stb = new StringBuilder();
            bool isInner = false;
            while (ex != null && !string.IsNullOrEmpty(ex.Message))
            {
                stb.AppendFormat("\n\n{0}: {1},\n\r URL: {2},\n\rStack: {3}\n", isInner ? "Exception" : "--Inner Exception", ex.Message, context.Request.Path, ex.StackTrace);
                ex = ex.InnerException;
                isInner = true;
            }

            return stb.ToString();
        }
    }
}
