// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateObjectsFactory.cs" company="Allors bvba">
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
    using System.Data.Common;

    using Allors.Adapters.Database.Sql;
    using Allors.Adapters.Database.Sql.Commands;
    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    internal class CreateObjectsFactory : ICreateObjectsFactory
    {
        internal readonly Database Database;

        internal CreateObjectsFactory(Database database)
        {
            this.Database = database;
        }

        public ICreateObjects Create(Sql.DatabaseSession session)
        {
            return new CreateObjects(this, session);
        }

        private class CreateObjects : DatabaseCommand, ICreateObjects
        {
            private readonly CreateObjectsFactory factory;
            private readonly Dictionary<IObjectType, NpgsqlCommand> commandByObjectType;

            public CreateObjects(CreateObjectsFactory factory, Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.factory = factory;
                this.commandByObjectType = new Dictionary<IObjectType, NpgsqlCommand>();
            }

            public IList<Reference> Execute(IClass objectType, int count)
            {
                IObjectType exclusiveLeafClass = objectType.ExclusiveClass;
                Sql.Schema schema = this.Database.Schema;

                NpgsqlCommand command;
                if (!this.commandByObjectType.TryGetValue(exclusiveLeafClass, out command))
                {
                    command = this.Session.CreateNpgsqlCommand(Sql.Schema.AllorsPrefix + "COS_" + exclusiveLeafClass.SingularName);
                    command.CommandType = CommandType.StoredProcedure;
                    this.AddInObject(command, schema.TypeId.Param, objectType.Id);
                    this.AddInObject(command, schema.CountParam, count);

                    this.commandByObjectType[exclusiveLeafClass] = command;
                }
                else
                {
                    this.SetInObject(command, schema.TypeId.Param, objectType.Id);
                    this.SetInObject(command, schema.CountParam, count);
                }

                var objectIds = new List<object>();
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object id = long.Parse(reader[0].ToString());
                        objectIds.Add(id);
                    }
                }

                var strategies = new List<Reference>();

                foreach (object id in objectIds)
                {
                    long objectId = long.Parse(id.ToString());
                    var strategySql = this.Session.CreateAssociationForNewObject(objectType, objectId);
                    strategies.Add(strategySql);
                }

                return strategies;
            }
        }
    }
}