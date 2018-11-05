// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Load.cs" company="Allors bvba">
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

    using Microsoft.Extensions.Logging;

    [Command(Description = "Add file contents to the index")]
    public class Load
    {
        private readonly IDatabaseService databaseService;

        private readonly ILogger<Load> logger;

        public Load(IDatabaseService databaseService, ILogger<Load> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        [Option("-f", Description = "File to load (default is population.xml)")]
        public string FileName { get; set; } = "population.xml";

        public int OnExecute(CommandLineApplication app)
        {
            var fileInfo = new FileInfo(this.FileName);

            using (var reader = XmlReader.Create(fileInfo.FullName))
            {
                this.logger.LogInformation("Begin", fileInfo.FullName);
                this.logger.LogInformation("Loading {file}", fileInfo.FullName);
                this.databaseService.Database.Load(reader);
                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }
    }
}