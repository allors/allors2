// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetUnitRoleFactory.cs" company="Allors bvba">
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

    using Allors.Adapters.Database.Sql.Commands;
    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class SetUnitRoleFactory : ISetUnitRoleFactory
    {
        public readonly Database Database;
        private readonly Dictionary<IObjectType, Dictionary<IRoleType, string>> sqlByRoleTypeByObjectType;

        public SetUnitRoleFactory(Database database)
        {
            this.Database = database;
            this.sqlByRoleTypeByObjectType = new Dictionary<IObjectType, Dictionary<IRoleType, string>>();
        }

        public ISetUnitRole Create(Sql.DatabaseSession session)
        {
            return new SetUnitRole(this, session);
        }

        public string GetSql(IObjectType objectType, IRoleType roleType)
        {
            Dictionary<IRoleType, string> sqlByRoleType;
            if (!this.sqlByRoleTypeByObjectType.TryGetValue(objectType, out sqlByRoleType))
            {
                sqlByRoleType = new Dictionary<IRoleType, string>();
                this.sqlByRoleTypeByObjectType.Add(objectType, sqlByRoleType);
            }

            if (!sqlByRoleType.ContainsKey(roleType))
            {
                var sql = Sql.Schema.AllorsPrefix + "SR_" + objectType.SingularName + "_" + roleType.SingularPropertyName;
                sqlByRoleType[roleType] = sql;
            }

            return sqlByRoleType[roleType];
        }

        private class SetUnitRole : DatabaseCommand, ISetUnitRole
        {
            private readonly SetUnitRoleFactory factory;
            private readonly Dictionary<IObjectType, Dictionary<IRoleType, NpgsqlCommand>> commandByRoleTypeByObjectType;

            public SetUnitRole(SetUnitRoleFactory factory, Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.factory = factory;
                this.commandByRoleTypeByObjectType = new Dictionary<IObjectType, Dictionary<IRoleType, NpgsqlCommand>>();
            }

            public void Execute(IList<UnitRelation> relation, IObjectType exclusiveLeafClass, IRoleType roleType)
            {
                var schema = this.Database.NpgsqlSchema;

                Dictionary<IRoleType, NpgsqlCommand> commandByRoleType;
                if (!this.commandByRoleTypeByObjectType.TryGetValue(exclusiveLeafClass, out commandByRoleType))
                {
                    commandByRoleType = new Dictionary<IRoleType, NpgsqlCommand>();
                    this.commandByRoleTypeByObjectType.Add(exclusiveLeafClass, commandByRoleType);
                }

                SchemaArrayParameter arrayParam;

                var unitType = (IUnit)roleType.ObjectType;
                var unitTypeTag = unitType.UnitTag;
                switch (unitTypeTag)
                {
                    case UnitTags.AllorsString:
                        arrayParam = schema.StringRelationArrayParam;
                        break;

                    case UnitTags.AllorsInteger:
                        arrayParam = schema.IntegerRelationArrayParam;
                        break;

                    case UnitTags.AllorsFloat:
                        arrayParam = schema.DoubleRelationArrayParam;
                        break;

                    case UnitTags.AllorsBoolean:
                        arrayParam = schema.BooleanRelationArrayParam;
                        break;

                    case UnitTags.AllorsDate:
                        arrayParam = schema.DateTimeRelationArrayParam;
                        break;

                    case UnitTags.AllorsUnique:
                        arrayParam = schema.UniqueRelationArrayParam;
                        break;

                    case UnitTags.AllorsBinary:
                        arrayParam = schema.BinaryRelationArrayParam;
                        break;

                    case UnitTags.AllorsDecimal:
                        arrayParam = schema.DecimalRelationTableParameterByScaleByPrecision[roleType.Precision.Value][roleType.Scale.Value];
                        break;

                    default:
                        throw new ArgumentException("Unknown Unit IObjectType: " + unitTypeTag);
                }

                NpgsqlCommand command;
                if (!commandByRoleType.TryGetValue(roleType, out command))
                {
                    command = this.Session.CreateNpgsqlCommand(this.factory.GetSql(exclusiveLeafClass, roleType));
                    command.CommandType = CommandType.StoredProcedure;

                    this.AddInTable(command, this.Database.NpgsqlSchema.ObjectArrayParam, this.Database.CreateAssociationTable(relation));
                    this.AddInTable(command, arrayParam, this.Database.CreateRoleTable(relation));
                }
                else
                {
                    this.SetInTable(command, this.Database.NpgsqlSchema.ObjectArrayParam, this.Database.CreateAssociationTable(relation));
                    this.SetInTable(command, arrayParam, this.Database.CreateRoleTable(relation));
                }

                command.ExecuteNonQuery();
            }
        }
    }
}