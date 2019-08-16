// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Populate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Commands
{
    using System;
    using System.IO;

    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [Command(Description = "Add file contents to the index")]
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

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                new Allors.Upgrade(session, this.dataPath).Execute();

                session.Derive();
                session.Commit();
            }

            this.logger.LogInformation("End");

            return ExitCode.Success;
        }
    }
}
