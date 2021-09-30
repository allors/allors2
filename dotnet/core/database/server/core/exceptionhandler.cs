// <copyright file="ExceptionHandler.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    public static class ExceptionHandler
    {
        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder appBuilder, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            async Task Middleware(HttpContext context, Func<Task> next)
            {
                var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandler != null)
                {
                    var error = exceptionHandler.Error;

                    var logger = loggerFactory.CreateLogger(error.GetType());
                    logger.LogError(error, "Unhandled Exception");

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = error is SecurityTokenExpiredException
                        ? (int)HttpStatusCode.Unauthorized
                        : (int)HttpStatusCode.InternalServerError;

                    var message = env.IsDevelopment() ?
                        $"{error.Message}\n{error.StackTrace}" :
                        $"{error.Message}";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                }
                else
                {
                    await next();
                }
            }

            return appBuilder.UseExceptionHandler(v => v.Use(Middleware));
        }
    }
}
