// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Object.SqlClient.ReadCommitted
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;
    using Adapters;

    using Allors.Adapters.Object.SqlClient.Caching.Debugging;
    using Allors.Adapters.Object.SqlClient.Debug;

    public class Profile : SqlClient.Profile
    {
        private readonly Prefetchers prefetchers = new Prefetchers();

        private readonly DebugConnectionFactory connectionFactory;
        private readonly DebugCacheFactory cacheFactory;

        public Profile()
        {
        }

        public Profile(DebugConnectionFactory connectionFactory, DebugCacheFactory cacheFactory)
        {
            this.connectionFactory = connectionFactory;
            this.cacheFactory = cacheFactory;
        }

        public override Action[] Markers
        {
            get
            {
                var markers = new List<Action> 
                { 
                    () => { }, 
                    () => this.Session.Commit(), 
                };

                if (Settings.ExtraMarkers)
                {
                    markers.Add(
                        () =>
                        {
                            foreach (var @class in this.Session.Database.MetaPopulation.Classes)
                            {
                                var prefetchPolicy = this.prefetchers[@class];
                                this.Session.Prefetch(prefetchPolicy, this.Session.Extent(@class));
                            }
                        });
                }

                return markers.ToArray();
            }
        }

        protected override string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["sqlclientobject"].ConnectionString;

        public IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init)
        {
            var configuration = new SqlClient.Configuration
                                    {
                                        ObjectFactory = this.CreateObjectFactory(metaPopulation),
                                        ConnectionString = this.ConnectionString,
                                        ConnectionFactory = this.connectionFactory,
                                        CacheFactory = this.cacheFactory
                                    };
            var database = new Database(configuration);

            if (init)
            {
                database.Init();
            }

            return database;
        }

        public override IDatabase CreatePopulation()
        {
            return new Memory.Database(new Memory.Configuration { ObjectFactory = this.ObjectFactory });
        }

        public override IDatabase CreateDatabase()
        {
            var configuration = new SqlClient.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.ConnectionString,
                ConnectionFactory = this.connectionFactory,
                CacheFactory = this.cacheFactory
            };

            var database = new Database(configuration);

            return database;
        }

        protected override bool Match(ColumnTypes columnType, string dataType)
        {
            dataType = dataType.Trim().ToLowerInvariant();

            switch (columnType)
            {
                case ColumnTypes.ObjectId:
                    return dataType.Equals("int");
                case ColumnTypes.TypeId:
                    return dataType.Equals("uniqueidentifier");
                case ColumnTypes.CacheId:
                    return dataType.Equals("int");
                case ColumnTypes.Binary:
                    return dataType.Equals("varbinary");
                case ColumnTypes.Boolean:
                    return dataType.Equals("bit");
                case ColumnTypes.Decimal:
                    return dataType.Equals("decimal");
                case ColumnTypes.Float:
                    return dataType.Equals("float");
                case ColumnTypes.Integer:
                    return dataType.Equals("int");
                case ColumnTypes.String:
                    return dataType.Equals("nvarchar");
                case ColumnTypes.Unique:
                    return dataType.Equals("uniqueidentifier");
                default:
                    throw new Exception("Unsupported columntype " + columnType);
            }
        }
    }
}