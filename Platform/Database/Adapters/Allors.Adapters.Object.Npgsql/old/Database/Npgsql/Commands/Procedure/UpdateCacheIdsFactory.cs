// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCacheIdsFactory.cs" company="Allors bvba">
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

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class UpdateCacheIdsFactory
    {
        private readonly Database database;

        public UpdateCacheIdsFactory(Database database)
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

        public UpdateCacheIds Create(Sql.DatabaseSession session)
        {
            return new UpdateCacheIds(this, session);
        }

        public class UpdateCacheIds : DatabaseCommand
        {
            private readonly UpdateCacheIdsFactory factory;
            private NpgsqlCommand command;

            public UpdateCacheIds(UpdateCacheIdsFactory factory, Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.factory = factory;
            }

            public void Execute(Dictionary<Reference, Roles> modifiedRolesByReference)
            {
                var schema = this.factory.Database.NpgsqlSchema;

                if (this.command == null)
                {
                    this.command = this.Session.CreateNpgsqlCommand(Schema.AllorsPrefix + "UC");
                    this.command.CommandType = CommandType.StoredProcedure;
                    this.AddInTable(this.command, schema.ObjectArrayParam, this.factory.Database.CreateObjectTable(modifiedRolesByReference.Keys));
                }
                else
                {
                    this.SetInTable(this.command, schema.ObjectArrayParam, this.factory.Database.CreateObjectTable(modifiedRolesByReference.Keys));
                }

                this.command.ExecuteNonQuery();
            }
        }
    }
}