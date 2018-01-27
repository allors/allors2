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
    using System.Collections.Generic;
    using System.Data;

    using Allors.Adapters.Database.Sql;
    using Allors.Adapters.Database.Sql.Commands;
    using Allors.Meta;

    using global::Npgsql;

    using DatabaseSession = DatabaseSession;

    internal class InstantiateObjectsFactory : IInstantiateObjectsFactory
    {
        private readonly Npgsql.Database database;

        internal InstantiateObjectsFactory(Npgsql.Database database)
        {
            this.database = database;
        }

        public IInstantiateObjects Create(Sql.DatabaseSession session)
        {
            return new InstantiateObjects(this.database, session);
        }

        private class InstantiateObjects : DatabaseCommand, IInstantiateObjects
        {
            private readonly Npgsql.Database database;
            private NpgsqlCommand command;

            public InstantiateObjects(Npgsql.Database database, Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.database = database;
            }

            public IList<Reference> Execute(IList<long> objectids)
            {
                if (this.command == null)
                {
                    this.command = this.Session.CreateNpgsqlCommand(Schema.AllorsPrefix + "IOS");
                    this.command.CommandType = CommandType.StoredProcedure;
                    this.AddInTable(this.command, this.database.NpgsqlSchema.ObjectArrayParam, this.Database.CreateObjectTable(objectids));
                }
                else
                {
                    this.SetInTable(this.command, this.database.NpgsqlSchema.ObjectArrayParam, this.Database.CreateObjectTable(objectids));
                }

                var objects = new List<Reference>();
                using (NpgsqlDataReader reader = this.command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var idString = reader[0].ToString();
                        var id = this.Database.AllorsObjectIds.Parse(idString);
                        var classId = this.GetClassId(reader, 1);
                        var cacheId = this.GetCachId(reader, 2);

                        var type = (IClass)this.Session.Database.ObjectFactory.MetaPopulation.Find(classId);
                        var obj = this.Session.GetOrCreateAssociationForExistingObject(type, id, cacheId);

                        objects.Add(obj);
                    }
                }

                return objects;
            }
        }
    }
}