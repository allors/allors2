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

namespace Allors.Console
{
    using System.IO;
    using System.Xml;

    using Allors.Services;

    using Microsoft.Extensions.Logging;

    public class Load
    {
        private readonly IDatabase database;
        private readonly ILogger<Load> logger;

        public Load(IDatabaseService databaseService, ILogger<Load> logger)
        {
            this.database = databaseService.Database;
            this.logger = logger;
        }

        public int Execute(LoadOptions opts)
        {
            var fileInfo = new FileInfo(opts.File);

            using (var reader = XmlReader.Create(fileInfo.FullName))
            {
                this.logger.LogInformation("Begin", fileInfo.FullName);
                this.logger.LogInformation("Loading {file}", fileInfo.FullName);
                this.database.Load(reader);
                this.logger.LogInformation("End");
            }

            return 0;
        }
    }
}