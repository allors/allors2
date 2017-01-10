// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
//   Copyright 2002-2010 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Special.Npgsql.IntegerId.ReadCommitted
{
    using System;
    using System.Collections.Generic;

    using Allors.Adapters.Database.Npgsql.IntegerId;
    using Allors.Meta;

    public class Profile : Npgsql.Profile
    {
        private static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["npgsql"].ConnectionString;

        public override Action[] Markers
        {
            get
            {
                var markers = new List<Action> 
                { 
                    () => { }, 
                    () => this.Session.Commit() 
                };

                if (Settings.ExtraMarkers)
                {
                    markers.Add(
                        () =>
                        {
                            this.Session.Commit();
                            ((Database)this.Session.Population).Cache.Invalidate();
                        });
                }

                return markers.ToArray();
            }
        }

        public IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init)
        {
            var configuration = new Adapters.Database.Npgsql.IntegerId.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                CacheFactory = this.CacheFactory,
                Id = Guid.NewGuid(),
                ConnectionString = ConnectionString
            };
            var database = new Database(configuration);

            if (init)
            {
                database.Init();
            }

            return database;
        }

        public override IDatabase CreateDatabase()
        {
            var configuration = new Adapters.Database.Npgsql.IntegerId.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                CacheFactory = this.CacheFactory,
                Id = Guid.NewGuid(),
                ConnectionString = ConnectionString
            };
            var database = new Database(configuration);

            return database;
        }

        public override IWorkspace CreateWorkspace(IDatabase database)
        {
            var configuration = new Workspace.Memory.IntegerId.Configuration { Database = database };
            return new Workspace.Memory.IntegerId.Workspace(configuration);
        }
    }
}