// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertObjectFactory.cs" company="Allors bvba">
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
    using Allors.Meta;

    using global::Npgsql;

    using DatabaseSession = DatabaseSession;

    public class InsertObjectFactory
    {
        public InsertObject Create(DatabaseSession session)
        {
            return new InsertObject(session);
        }

        public class InsertObject
        {
            private readonly DatabaseSession session;

            private readonly Dictionary<IObjectType, NpgsqlCommand> commandByObjectType;

            public InsertObject(DatabaseSession session)
            {
                this.session = session;
                this.commandByObjectType = new Dictionary<IObjectType, NpgsqlCommand>();
            }

            public Reference Execute(IClass objectType, long objectId)
            {
                var exclusiveLeafClass = objectType.ExclusiveClass;
                var schema = this.session.Schema;

                NpgsqlCommand command;
                if (!this.commandByObjectType.TryGetValue(exclusiveLeafClass, out command))
                {
                    command = this.session.CreateNpgsqlCommand(Sql.Schema.AllorsPrefix + "INS_" + exclusiveLeafClass.Name);
                    command.CommandType = CommandType.StoredProcedure;
                    Commands.NpgsqlCommandExtensions.AddInObject(command, schema.ObjectId.Param, objectId);
                    Commands.NpgsqlCommandExtensions.AddInObject(command, schema.TypeId.Param, objectType.Id);

                    this.commandByObjectType[exclusiveLeafClass] = command;
                }
                else
                {
                    Commands.NpgsqlCommandExtensions.SetInObject(command, schema.ObjectId.Param, objectId);
                    Commands.NpgsqlCommandExtensions.SetInObject(command, schema.TypeId.Param, objectType.Id);
                }

                var result = (bool)command.ExecuteScalar();
                return result ? this.session.CreateAssociationForNewObject(objectType, objectId) : null;
            }
        }
    }
}