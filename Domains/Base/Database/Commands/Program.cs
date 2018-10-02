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

namespace Commands
{
    using System;
    using System.IO;
    using System.Linq;

    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using CommandLine;

    using Commands.Verbs;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using NLog.Extensions.Logging;
    
    public class Program
    {
        private static readonly ServiceProvider ServiceProvider;

        static Program()
        {
            var services = new ServiceCollection();
            services.AddAllors();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var assembly = typeof(Program).Assembly;
            var verbTypes = assembly.ExportedTypes
                .Where(v => v.IsClass && v.IsPublic && !v.IsNested && v.Namespace.StartsWith("Commands.Verbs"));

            foreach (var verbType in verbTypes)
            {
                services.AddTransient(verbType);
            }

            ServiceProvider = services.BuildServiceProvider();

            var databaseService = ServiceProvider.GetRequiredService<IDatabaseService>();
            var databaseConfiguration = new Configuration
                                    {
                                        ConnectionString = configuration["allors"],
                                        ObjectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(User)),
                                        CommandTimeout = 0
                                    };
            
            databaseService.Database = new Database(ServiceProvider, databaseConfiguration);

            var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
        }

        private static DirectoryInfo ServerDirectory
        {
            get
            {
                var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                while (directoryInfo != null && directoryInfo.GetDirectories("Server").Length == 0)
                {
                    directoryInfo = directoryInfo.Parent;
                }

                directoryInfo = directoryInfo?.GetDirectories("Server").FirstOrDefault();
                return directoryInfo;
            }
        }

        public static int Main(string[] args)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                return Parser.Default.ParseArguments<Custom.Options, Load.Options, Populate.Options, Save.Options, Upgrade.Options>(args)
                    .MapResult(
                        (Custom.Options opts) => ServiceProvider.GetRequiredService<Custom>().Execute(),
                        (Load.Options opts) => ServiceProvider.GetRequiredService<Load>().Execute(opts),
                        (Populate.Options opts) => ServiceProvider.GetRequiredService<Populate>().Execute(),
                        (Save.Options opts) => ServiceProvider.GetRequiredService<Save>().Execute(opts),
                        (Upgrade.Options opts) => ServiceProvider.GetRequiredService<Upgrade>().Execute(opts),
                        errors =>
                            {
                                logger.LogError("{errors}", errors);
                                return ExitCode.Error;
                            });
            } 
            catch (Exception e)
            {
                logger.LogError("Uncaught Exception: {exception}", e);
                return ExitCode.Error;
            }
        }
    }
}