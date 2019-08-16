// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Load.cs" company="PlaceholderCompany">
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

    [Command(Description = "Load the population from file")]
    public class Load
    {
        private readonly IConfiguration configuration;

        private readonly IDatabaseService databaseService;

        private readonly ILogger<Load> logger;

        public Load(IConfiguration configuration, IDatabaseService databaseService, ILogger<Load> logger)
        {
            this.configuration = configuration;
            this.databaseService = databaseService;
            this.logger = logger;
        }

        [Option("-f", Description = "File to load (default is population.xml)")]
        public string FileName { get; set; } = "population.xml";

        public int OnExecute(CommandLineApplication app)
        {
            this.logger.LogInformation("Begin");

            var fileName = this.FileName ?? this.configuration["populationFile"];
            var fileInfo = new FileInfo(fileName);

            using (var reader = XmlReader.Create(fileInfo.FullName))
            {
                this.logger.LogInformation("Loading {file}", fileInfo.FullName);
                this.databaseService.Database.Load(reader);
            }

            this.logger.LogInformation("End");
            return ExitCode.Success;
        }
    }
}
