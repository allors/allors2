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

    public class Populate : Command
    {
        public override void Execute()
        {
            var database = this.CreateDatabase(IsolationLevel.Serializable);

            Console.WriteLine("Are you sure, all current data will be destroyed? (Y/N)\n");

            var confirmationKey = Console.ReadKey(true).KeyChar.ToString();
            if (confirmationKey.ToLower().Equals("y"))
            {
                database.Init();

                database = this.CreateDatabase(IsolationLevel.Serializable);

                using (var session = database.CreateSession())
                {
                    var dataPath = this.Configuration["dataPath"];
                    var directoryInfo = dataPath != null ? new DirectoryInfo(dataPath) : null;
                    new Setup(session, directoryInfo).Apply();

                    var administrator = new Users(session).GetUser("administrator");
                    session.SetUser(administrator);

                    new Allors.Upgrade(session, directoryInfo).Execute();

                    var validation = session.Derive(false);
                    if (validation.HasErrors)
                    {
                        foreach (var error in validation.Errors)
                        {
                            Console.WriteLine(error);
                        }

                        Console.WriteLine("Not populated");
                    }
                    else
                    {
                        session.Commit();

                        Console.WriteLine("Populated");
                    }
                }
            }
        }
    }
}