// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Commands
{
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddAllors();

            const string FileName = @"base.appSettings.json";
            var userSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/allors/{FileName}";
            var systemSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}/allors/{FileName}";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .AddJsonFile(systemSettings, true)
                .AddJsonFile(userSettings, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var app = new CommandLineApplication<Commands>();
                app.Conventions.UseDefaultConventions().UseConstructorInjection(serviceProvider);
                app.Execute(args);
            }
            catch (Exception e)
            {
                var logger = serviceProvider.GetService<ILogger<Program>>();
                logger.LogCritical(e, "Uncaught exception");
            }
        }
    }
}