// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System;
    using System.IO;
    using Allors.Services;
    using Bogus;
    using McMaster.Extensions.CommandLineUtils;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static int Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();

            const string root = "/config/base";
            configurationBuilder.AddCrossPlatform(".");
            configurationBuilder.AddCrossPlatform(root);
            configurationBuilder.AddCrossPlatform(Path.Combine(root, "commands"));
            configurationBuilder.AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.AddAllors();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));
            services.AddSingleton<Faker>();

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var app = new CommandLineApplication<Commands>();
                app.Conventions.UseDefaultConventions().UseConstructorInjection(serviceProvider);
                return app.Execute(args);
            }
            catch (Exception e)
            {
                var logger = serviceProvider.GetService<ILogger<Program>>();
                logger.LogCritical(e, e.Message);
                return ExitCode.Error;
            }
        }
    }
}
