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

using MysticMind.PostgresEmbed;

namespace Allors.Adapters.Object.Npgsql.ReadCommitted
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Allors.Meta;
    using global::Npgsql;
    using Microsoft.Extensions.DependencyInjection;

    public class Profile : Npgsql.Profile
    {
        private readonly PgServer pgServer;

        public Profile(PgServer pgServer)
        {
            this.pgServer = pgServer;
            var services = new ServiceCollection();
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; set; }

        public override Action[] Markers
        {
            get
            {
                var markers = new List<Action>
                {
                    () => { },
                    () => this.Session.Commit(),
                };

                return markers.ToArray();
            }
        }

        protected string ConnectionString => $"Server=localhost; Port={this.pgServer.PgPort}; User Id=postgres; Password=test; Database=postgres; Pooling=false; Timeout=300; CommandTimeout=300";

        public IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init)
        {
            var configuration = new Npgsql.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.ConnectionString,
                IsolationLevel = IsolationLevel.Serializable
            };

            var database = new Database(this.ServiceProvider, configuration);

            if (init)
            {
                database.Init();
            }

            return database;
        }

        public override IDatabase CreateDatabase()
        {
            var configuration = new Npgsql.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.ConnectionString
            };

            var database = new Database(this.ServiceProvider, configuration);

            return database;
        }

        public override IDatabase CreatePopulation() => new Memory.Database(this.ServiceProvider, new Memory.Configuration { ObjectFactory = this.ObjectFactory });

        protected override NpgsqlConnection CreateDbConnection() => new NpgsqlConnection(this.ConnectionString);
    }
}
