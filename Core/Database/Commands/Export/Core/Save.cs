// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Save.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Commands
{
    using System.IO;
    using System.Xml;

    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [Command(Description = "Save the population to file")]
    public class Save
    {
        private readonly IConfiguration configuration;

        private readonly IDatabaseService databaseService;

        private readonly ILogger<Save> logger;

        public Save(IConfiguration configuration, IDatabaseService databaseService, ILogger<Save> logger)
        {
            this.configuration = configuration;
            this.databaseService = databaseService;
            this.logger = logger;
        }

        [Option("-f", Description = "File to save")]
        public string FileName { get; set; } = "population.xml";

        public int OnExecute(CommandLineApplication app)
        {
            this.logger.LogInformation("Begin");

            var fileName = this.FileName ?? this.configuration["populationFile"];
            var fileInfo = new FileInfo(fileName);

            using (var stream = File.Create(fileInfo.FullName))
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    this.logger.LogInformation("Saving {file}", fileInfo.FullName);
                    this.databaseService.Database.Save(writer);
                }
            }

            this.logger.LogInformation("End");
            return ExitCode.Success;
        }
    }
}
