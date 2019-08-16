// <copyright file="ApplicationBuilderExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
