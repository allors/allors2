// <copyright file="ApplicationBuilderExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Allors.Server
{
    using Allors.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseAllors(this IApplicationBuilder app, IDatabase database)
        {
            var databaseService = app.ApplicationServices.GetRequiredService<IDatabaseService>();
            databaseService.Database = database;
            return app;
        }
    }
}
