// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateObjectFactory.cs" company="Allors bvba">
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
// <summary>
//   
// </summary>
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

    public class CreateObjectFactory 
    {
        public CreateObject Create(Sql.DatabaseSession session)
        {
            return new CreateObject(session);
        }

        public class CreateObject : DatabaseCommand
        {
            private readonly Dictionary<IObjectType, NpgsqlCommand> commandByObjectType;

            public CreateObject(Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.commandByObjectType = new Dictionary<IObjectType, NpgsqlCommand>();
            }

            public Reference Execute(IClass objectType)
            {
                var exclusiveLeafClass = objectType.ExclusiveClass;
                var schema = this.Database.Schema;

                NpgsqlCommand command;
                if (!this.commandByObjectType.TryGetValue(exclusiveLeafClass, out command))
                {
                    command = this.Session.CreateNpgsqlCommand(Schema.AllorsPrefix + "CO_" + exclusiveLeafClass.Name);
                    command.CommandType = CommandType.StoredProcedure;
                    this.AddInObject(command, schema.TypeId.Param, objectType.Id);

                    this.commandByObjectType[exclusiveLeafClass] = command;
                }
                else
                {
                    this.SetInObject(command, schema.TypeId.Param, objectType.Id);
                }

                var result = command.ExecuteScalar();
                var objectId = long.Parse(result.ToString());
                return this.Session.CreateAssociationForNewObject(objectType, objectId);
            }
        }
    }
}