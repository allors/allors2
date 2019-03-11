// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetUnitRolesFactory.cs" company="Allors bvba">
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
    using System.Data.Common;

    using Allors.Adapters.Database.Sql;
    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class GetUnitRolesFactory
    {
        public readonly Database Database;
        private readonly Dictionary<IObjectType, string> sqlByObjectType;

        public GetUnitRolesFactory(Database database)
        {
            this.Database = database;
            this.sqlByObjectType = new Dictionary<IObjectType, string>();
        }

        public GetUnitRoles Create(DatabaseSession session)
        {
            return new GetUnitRoles(this, session);
        }

        public string GetSql(IObjectType objectType)
        {
            if (!this.sqlByObjectType.ContainsKey(objectType))
            {
                var sql = Sql.Schema.AllorsPrefix + "GU_" + objectType.SingularName;
                this.sqlByObjectType[objectType] = sql;
            }

            return this.sqlByObjectType[objectType];
        }

        public class GetUnitRoles
        {
            private readonly GetUnitRolesFactory factory;

            private readonly DatabaseSession session;

            private readonly Dictionary<IObjectType, NpgsqlCommand> commandByObjectType;

            public GetUnitRoles(GetUnitRolesFactory factory, DatabaseSession session)
            {
                this.factory = factory;
                this.session = session;
                this.commandByObjectType = new Dictionary<IObjectType, NpgsqlCommand>();
            }

            public void Execute(Roles roles)
            {
                var reference = roles.Reference;
                var objectType = reference.ObjectType;

                NpgsqlCommand command;
                if (!this.commandByObjectType.TryGetValue(objectType, out command))
                {
                    command = this.session.CreateNpgsqlCommand(this.factory.GetSql(objectType));
                    command.CommandType = CommandType.StoredProcedure;
                    Commands.NpgsqlCommandExtensions.AddInObject(command, this.session.Schema.ObjectId.Param, reference.ObjectId);

                    this.commandByObjectType[objectType] = command;
                }
                else
                {
                    Commands.NpgsqlCommandExtensions.SetInObject(command, this.session.Schema.ObjectId.Param, reference.ObjectId);
                }

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var sortedUnitRoles = this.session.Database.GetSortedUnitRolesByObjectType(reference.ObjectType);

                        for (var i = 0; i < sortedUnitRoles.Length; i++)
                        {
                            var roleType = sortedUnitRoles[i];

                            object unit = null;
                            if (!reader.IsDBNull(i))
                            {
                                var unitType = (IUnit)roleType.ObjectType;
                                var unitTypeTag = unitType.UnitTag;
                                switch (unitTypeTag)
                                {
                                    case UnitTags.String:
                                        unit = reader.GetString(i);
                                        break;
                                    case UnitTags.Integer:
                                        unit = reader.GetInt32(i);
                                        break;
                                    case UnitTags.Decimal:
                                        unit = reader.GetDecimal(i);
                                        break;
                                    case UnitTags.Float:
                                        unit = reader.GetDouble(i);
                                        break;
                                    case UnitTags.Boolean:
                                        unit = reader.GetBoolean(i);
                                        break;
                                    case UnitTags.DateTime:
                                        var dateTime = reader.GetDateTime(i);
                                        unit = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
                                        break;
                                    case UnitTags.Unique:
                                        unit = reader.GetGuid(i);
                                        break;
                                    case UnitTags.Binary:
                                        var byteArray = (byte[])reader.GetValue(i);
                                        unit = byteArray;
                                        break;
                                    default:
                                        throw new ArgumentException("Unknown Unit IObjectType: " + roleType.ObjectType.SingularName);
                                }
                            }

                            roles.CachedObject.SetValue(roleType, unit);
                        }
                    }
                }
            }
        }
    }
}