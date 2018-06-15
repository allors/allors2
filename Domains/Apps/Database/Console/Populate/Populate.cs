// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Populate.cs" company="Allors bvba">
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
    using System;
    using System.Data;
    using System.IO;

    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class Populate
    {
        private readonly string dataPath;
        private readonly IDatabase database;
        private readonly ILogger<Populate> logger;

        public Populate(IConfigurationRoot configurationRoot, IDatabaseService databaseService, ILogger<Populate> logger)
        {
            this.dataPath = configurationRoot["datapath"];
            this.database = databaseService.Database;
            this.logger = logger;
        }

        public int Execute()
        {
            Console.WriteLine("Are you sure, all current data will be destroyed? (Y/N)\n");

            var confirmationKey = Console.ReadKey(true).KeyChar.ToString();
            if (confirmationKey.ToLower().Equals("y"))
            {
                this.logger.LogInformation("Begin");

                this.database.Init();

                using (var session = this.database.CreateSession())
                {
                    var directoryInfo = this.dataPath != null ? new DirectoryInfo(this.dataPath) : null;
                    new Setup(session, directoryInfo).Apply();

                    session.Derive();
                    session.Commit();

                    var administrator = new Users(session).GetUser("administrator");
                    session.SetUser(administrator);

                    new Allors.Upgrade(session, directoryInfo).Execute();

                    session.Derive();
                    session.Commit();

                    // new Initial(session, directoryInfo).Execute();
                    new Demo(session, directoryInfo).Execute();

                    session.Derive();
                    session.Commit();
                }

                this.logger.LogInformation("End");
            }

            return 0;
        }
    }
}