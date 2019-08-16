// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Custom.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Commands
{
    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;

    [Command(Description = "Execute custom code")]
    public class Custom
    {
        private readonly IDatabaseService databaseService;

        private readonly ILogger<Custom> logger;

        public Custom(IDatabaseService databaseService, ILogger<Custom> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        private Commands Parent { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                // Custom code

                session.Derive();
                session.Commit();

                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }
    }
}
