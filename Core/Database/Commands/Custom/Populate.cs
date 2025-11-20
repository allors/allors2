// <copyright file="Populate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.IO;

    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [Command(Description = "Create a population")]
    public class Populate
    {
        private readonly IDatabaseService databaseService;

        private readonly ILogger<Populate> logger;

        private readonly DirectoryInfo dataPath;

        public Populate(IConfiguration configuration, IDatabaseService databaseService, ILogger<Populate> logger)
        {
            this.dataPath = new DirectoryInfo(".").GetAncestorSibling(configuration["datapath"]);
            this.databaseService = databaseService;
            this.logger = logger;
        }

        public int OnExecute(CommandLineApplication app)
        {
            this.logger.LogInformation("Begin");

            this.databaseService.Database.Init();

            using (var session = this.databaseService.Database.CreateSession())
            {
                var config = new Config { DataPath = this.dataPath };
                new Setup(session, config).Apply();

                session.Derive();
                session.Commit();

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

                new Allors.Upgrade(session, this.dataPath).Execute();

                session.Derive();
                session.Commit();
            }

            this.logger.LogInformation("End");

            return ExitCode.Success;
        }
    }
}
