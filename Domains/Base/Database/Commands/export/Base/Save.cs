// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Save.cs" company="Allors bvba">
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
    using System.IO;
    using System.Xml;

    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [Command(Description = "Add file contents to the index")]
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