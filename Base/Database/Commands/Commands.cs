// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Commands.cs" company="Allors bvba">
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
    using System.Data;

    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using NLog.Extensions.Logging;

    [Command(Description = "Allors Base Commands")]
    [Subcommand(
        typeof(Save),
        typeof(Load),
        typeof(Upgrade),
        typeof(Populate),
        typeof(Print),
        typeof(Custom))]
    public class Commands
    {
        public Commands(IServiceProvider serviceProvider, IConfiguration configuration, IDatabaseService databaseService, ILoggerFactory loggerFactory)
        {
            var databaseConfiguration = new Configuration
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ObjectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(User)),
                IsolationLevel = this.IsolationLevel,
                CommandTimeout = this.CommandTimeout
            };

            databaseService.Database = new Database(serviceProvider, databaseConfiguration);

            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
        }


        [Option("-i", Description = "Isolation Level (Snapshot|Serializable)")]
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.Snapshot;

        [Option("-t", Description = "Command Timeout in seconds")]
        public int CommandTimeout { get; set; } = 0;

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}