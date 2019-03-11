// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstantiateObjectFactory.cs" company="Allors bvba">
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

namespace Allors.Adapters.Database.Npgsql.Commands.Text
{
    using System;

    using Allors.Adapters.Database.Sql;
    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class InstantiateObjectFactory
    {
        public readonly Database Database;
        public readonly string Sql;

        public InstantiateObjectFactory(Database database)
        {
            this.Database = database;
            this.Sql = "SELECT " + database.Schema.TypeId + ", " + database.Schema.CacheId + "\n";
            this.Sql += "FROM " + database.Schema.Objects + "\n";
            this.Sql += "WHERE " + database.Schema.ObjectId + "=" + database.Schema.ObjectId.Param.InvocationName + "\n";
        }

        public InstantiateObject Create(DatabaseSession session)
        {
            return new InstantiateObject(this, session);
        }

        public class InstantiateObject
        {
            private readonly InstantiateObjectFactory factory;

            private readonly DatabaseSession session;

            private NpgsqlCommand command;

            public InstantiateObject(InstantiateObjectFactory factory, DatabaseSession session)
            {
                this.factory = factory;
                this.session = session;
            }

            public Reference Execute(long objectId)
            {
                if (this.command == null)
                {
                    this.command = this.session.CreateNpgsqlCommand(this.factory.Sql);
                    Commands.NpgsqlCommandExtensions.AddInObject(this.command, this.session.Schema.ObjectId.Param, objectId);
                }
                else
                {
                    Commands.NpgsqlCommandExtensions.SetInObject(this.command, this.session.Schema.ObjectId.Param, objectId);
                }

                using (var reader = this.command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var classId = reader.GetGuid(0);
                        var cacheId = reader.GetInt32(1);

                        var type = (IClass)this.factory.Database.ObjectFactory.MetaPopulation.Find(classId);
                        return this.session.GetOrCreateAssociationForExistingObject(type, objectId, cacheId);
                    }

                    return null;
                }
            }
        }
    }
}