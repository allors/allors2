// <copyright file="Load.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

            var fileName = this.configuration["populationFile"] ?? this.FileName;
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
