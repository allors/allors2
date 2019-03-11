// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstantiateObjectsFactory.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Allors.Adapters.Database.Sql;
    using Allors.Meta;

    using global::Npgsql;

    using DatabaseSession = DatabaseSession;

    public class InstantiateObjectsFactory
    {
        private readonly Npgsql.Database database;

        internal InstantiateObjectsFactory(Npgsql.Database database)
        {
            this.database = database;
        }

        public InstantiateObjects Create(DatabaseSession session)
        {
            return new InstantiateObjects(this.database, session);
        }

        public class InstantiateObjects
        {
            private readonly Npgsql.Database database;

            private readonly DatabaseSession session;

            private NpgsqlCommand command;

            public InstantiateObjects(Npgsql.Database database, DatabaseSession session)
            {
                this.database = database;
                this.session = session;
            }

            public IList<Reference> Execute(IList<long> objectids)
            {
                if (this.command == null)
                {
                    this.command = this.session.CreateNpgsqlCommand(Schema.AllorsPrefix + "IOS");
                    this.command.CommandType = CommandType.StoredProcedure;
                    Commands.NpgsqlCommandExtensions.AddInTable(this.command, this.database.NpgsqlSchema.ObjectArrayParam, this.session.NpgsqlDatabase.CreateObjectTable(objectids));
                }
                else
                {
                    Commands.NpgsqlCommandExtensions.SetInTable(this.command, this.database.NpgsqlSchema.ObjectArrayParam, this.session.NpgsqlDatabase.CreateObjectTable(objectids));
                }

                var objects = new List<Reference>();
                using (NpgsqlDataReader reader = this.command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var idString = reader[0].ToString();
                        var id = long.Parse(idString);
                        var classId = reader.GetGuid(1);
                        var cacheId = reader.GetInt32(2);

                        var type = (IClass)this.session.Database.ObjectFactory.MetaPopulation.Find(classId);
                        var obj = this.session.GetOrCreateAssociationForExistingObject(type, id, cacheId);

                        objects.Add(obj);
                    }
                }

                return objects;
            }
        }
    }
}