// <copyright file="Populate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System.IO;

    using Allors;
    using Allors.Domain;
    using Allors.Services;
    using Bogus;
    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [Command(Description = "Start with a new population")]
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

        [Option("-d", Description = "Create demo population")]
        public bool UseDemo { get; set; } = false;

        public int OnExecute(CommandLineApplication app)
        {
            this.logger.LogInformation("Begin");

            this.databaseService.Database.Init();

            using (var session = this.databaseService.Database.CreateSession())
            {
                var config = new Config { DataPath = this.dataPath };

                if (this.UseDemo)
                {
                    config.SetupSecurity = true;
                }

                new Setup(session, config).Apply();

                session.Derive();
                session.Commit();

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

                new Allors.Upgrade(session, this.dataPath).Execute();

                session.Derive();
                session.Commit();

                if (this.UseDemo)
                {
                    var faker = new Faker("en");
                    session.GetSingleton().Full(config.DataPath, faker);

                    session.Derive();
                    session.Commit();
                }
            }

            this.logger.LogInformation("End");

            return ExitCode.Success;
        }
    }
}
