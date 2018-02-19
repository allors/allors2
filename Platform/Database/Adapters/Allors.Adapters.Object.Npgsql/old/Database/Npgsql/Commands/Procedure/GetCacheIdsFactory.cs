// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCacheIdsFactory.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Npgsql.Commands.Procedure
{
    using System.Collections.Generic;
    using System.Data;

    using Allors.Adapters.Database.Sql;
    using Allors.Adapters.Database.Sql.Commands;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class GetCacheIdsFactory : IGetCacheIdsFactory
    {
        private readonly Database database;

        public GetCacheIdsFactory(Database database)
        {
            this.database = database;
        }

        public Database Database
        {
            get
            {
                return this.database;
            }
        }

        public IGetCacheIds Create(Sql.DatabaseSession session)
        {
            return new GetCacheIds(this, session);
        }

        private class GetCacheIds : DatabaseCommand, IGetCacheIds
        {
            private readonly GetCacheIdsFactory factory;
            private NpgsqlCommand command;

            public GetCacheIds(GetCacheIdsFactory factory, Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.factory = factory;
            }

            public Dictionary<long, int> Execute(ISet<Reference> strategyReferences)
            {
                var schema = this.factory.Database.NpgsqlSchema;

                if (this.command == null)
                {
                    this.command = this.Session.CreateNpgsqlCommand(Sql.Schema.AllorsPrefix + "GC");
                    this.command.CommandType = CommandType.StoredProcedure;
                    this.AddInTable(this.command, schema.ObjectArrayParam, this.Database.CreateObjectTable(strategyReferences));
                }
                else
                {
                    this.SetInTable(this.command, schema.ObjectArrayParam, this.Database.CreateObjectTable(strategyReferences));
                }

                var cacheIdByObjectId = new Dictionary<long, int>();

                using (var reader = this.command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objectId = long.Parse(reader[0].ToString());
                        var cacheId = reader.GetInt32(1);

                        cacheIdByObjectId.Add(objectId, cacheId);
                    }
                }

                return cacheIdByObjectId;
            }
        }
    }
}