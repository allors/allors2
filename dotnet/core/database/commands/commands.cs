// <copyright file="Commands.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System;
    using System.Data;
    using Allors;
    using Allors.Database.Adapters;
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
            var databaseBuilder = new DatabaseBuilder(serviceProvider, configuration, new ObjectFactory(MetaPopulation.Instance, typeof(User)), this.IsolationLevel, this.CommandTimeout);
            databaseService.Database = databaseBuilder.Build();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
        }

        [Option("-i", Description = "Isolation Level (Snapshot|RepeatableRead|Serializable)")]
        public IsolationLevel? IsolationLevel { get; set; }

        [Option("-t", Description = "Command Timeout in seconds")]
        public int? CommandTimeout { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
