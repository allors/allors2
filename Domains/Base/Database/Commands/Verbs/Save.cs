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

namespace Commands.Verbs
{
    using System;
    using System.IO;
    using System.Xml;

    using Allors;
    using Allors.Services;

    using CommandLine;

    using Microsoft.Extensions.Logging;

    public class Save
    {
        private readonly IDatabase database;
        private readonly ILogger<Save> logger;

        public Save(IDatabaseService databaseService, ILogger<Save> logger)
        {
            this.database = databaseService.Database;
            this.logger = logger;
        }

        public int Execute(Options opts)
        {
            var fileInfo = new FileInfo(opts.File);

            using (var stream = File.Create(fileInfo.FullName))
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    this.logger.LogInformation("Begin", fileInfo.FullName);
                    this.logger.LogInformation("Saving {file}", fileInfo.FullName);
                    this.database.Save(writer);
                    this.logger.LogInformation("End", fileInfo.FullName);
                }
            }

            return 0;
        }

        [Verb("save", HelpText = "Save the database.")]
        public class Options
        {
            [Option('f', "file", Required = false, Default = "population.xml", HelpText = "File to save to.")]
            public string File { get; set; }
        }
    }
}