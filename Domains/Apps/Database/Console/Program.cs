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

namespace Allors.Console
{
    using System.IO;
    using System.Linq;

    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using CommandLine;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using NLog.Extensions.Logging;

    using ILogger = Allors.ILogger;
    using ObjectFactory = Allors.ObjectFactory;

    public class Program
    {
        private static readonly ServiceProvider ServiceProvider;

        static Program()
        {
            var services = new ServiceCollection();
            services.AddAllors(new ServiceConfig
                                   {
                                       Directory = ServerDirectory,
                                       ApplicationName = "Server"
                                   });

            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .Build();
            services.AddSingleton(configurationRoot);

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            services.AddTransient<Custom>();
            services.AddTransient<Load>();
            services.AddTransient<Populate>();
            services.AddTransient<Save>();
            services.AddTransient<Upgrade>();

            ServiceProvider = services.BuildServiceProvider();

            var databaseService = ServiceProvider.GetRequiredService<IDatabaseService>();
            var configuration = new Configuration
                                    {
                                        ConnectionString = configurationRoot["allors"],
                                        ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User)),
                                        CommandTimeout = 0
                                    };
            
            databaseService.Database = new Database(ServiceProvider, configuration);

            var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
        }

        private static DirectoryInfo ServerDirectory
        {
            get
            {
                var directoryInfo = new DirectoryInfo(".");
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

            return Parser.Default.ParseArguments<CustomOptions, LoadOptions, PopulateOptions, SaveOptions, UpgradeOptions>(args)
                .MapResult(
                    (CustomOptions opts) => ServiceProvider.GetRequiredService<Custom>().Execute(),
                    (LoadOptions opts) => ServiceProvider.GetRequiredService<Load>().Execute(opts),
                    (PopulateOptions opts) => ServiceProvider.GetRequiredService<Populate>().Execute(),
                    (SaveOptions opts) => ServiceProvider.GetRequiredService<Save>().Execute(opts),
                    (UpgradeOptions opts) => ServiceProvider.GetRequiredService<Upgrade>().Execute(opts),
                    errors =>
                        {
                            logger.LogError("{errors}", errors);
                            return 1;
                        });
        }
    }
}