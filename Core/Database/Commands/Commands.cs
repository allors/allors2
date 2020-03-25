// <copyright file="Commands.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System;
    using System.Data;

    using Allors.Database.Adapters.SqlClient;
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
                CommandTimeout = this.CommandTimeout,
            };

            databaseService.Database = new Database(serviceProvider, databaseConfiguration);

            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
        }

        [Option("-i", Description = "Isolation Level (ReadCommitted|Serializable)")]
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

        [Option("-t", Description = "Command Timeout in seconds")]
        public int CommandTimeout { get; set; } = 0;

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
