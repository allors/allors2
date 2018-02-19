// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetUnitRolesFactory.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Text;

    using Allors.Adapters.Database.Sql;
    using Allors.Adapters.Database.Sql.Commands;
    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class SetUnitRolesFactory : ISetUnitRolesFactory
    {
        public readonly Database Database;

        public SetUnitRolesFactory(Database database)
        {
            this.Database = database;
        }

        public ISetUnitRoles Create(Sql.DatabaseSession session)
        {
            return new SetUnitRoles(session);
        }

        private class SetUnitRoles : DatabaseCommand, ISetUnitRoles
        {
            private readonly DatabaseSession session;

            private readonly Dictionary<IObjectType, Dictionary<IList<IRoleType>, NpgsqlCommand>> commandByKeyByObjectType; 

            public SetUnitRoles(Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.session = (DatabaseSession)session;
                this.commandByKeyByObjectType = new Dictionary<IObjectType, Dictionary<IList<IRoleType>, NpgsqlCommand>>();
            }

            public void Execute(Roles roles, IList<IRoleType> sortedRoleTypes)
            {
                var schema = this.Database.Schema;

                var exclusiveLeafClass = roles.Reference.ObjectType.ExclusiveClass;

                Dictionary<IList<IRoleType>, NpgsqlCommand> commandByKey;
                if (!this.commandByKeyByObjectType.TryGetValue(exclusiveLeafClass, out commandByKey))
                {
                    commandByKey = new Dictionary<IList<IRoleType>, NpgsqlCommand>(new SortedRoleTypesComparer());
                    this.commandByKeyByObjectType.Add(exclusiveLeafClass, commandByKey);
                }

                NpgsqlCommand command;
                if (!commandByKey.TryGetValue(sortedRoleTypes, out command))
                {
                    command = this.session.CreateNpgsqlCommand();
                    this.AddInObject(command, schema.ObjectId.Param, roles.Reference.ObjectId);

                    var sql = new StringBuilder();
                    sql.Append("UPDATE " + schema.Table(exclusiveLeafClass) + " SET\n");

                    var count = 0;
                    foreach (var roleType in sortedRoleTypes)
                    {
                        if (count > 0)
                        {
                            sql.Append(" , ");
                        }

                        ++count;

                        var column = schema.Column(roleType);
                        sql.Append(column + "= " + column.Param.InvocationName);

                        var unit = roles.ModifiedRoleByRoleType[roleType];
                        this.AddInObject(command, column.Param, unit);
                    }

                    sql.Append("\nWHERE " + schema.ObjectId + "= :" + schema.ObjectId.Param + "\n");

                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();

                    commandByKey.Add(sortedRoleTypes, command);
                }
                else
                {
                    this.SetInObject(command, schema.ObjectId.Param, roles.Reference.ObjectId);
                    
                    foreach (var roleType in sortedRoleTypes)
                    {
                        var column = schema.Column(roleType);

                        var unit = roles.ModifiedRoleByRoleType[roleType];
                        this.SetInObject(command, column.Param, unit);
                    }

                    command.ExecuteNonQuery();
                }
            }

            private class SortedRoleTypesComparer : IEqualityComparer<IList<IRoleType>>
            {
                public bool Equals(IList<IRoleType> x, IList<IRoleType> y)
                {
                    if (x.Count == y.Count)
                    {
                        for (var i = 0; i < x.Count; i++)
                        {
                            if (!x[i].Equals(y[i]))
                            {
                                return false;
                            }
                        }

                        return true;
                    }

                    return false;
                }

                public int GetHashCode(IList<IRoleType> roleTypes)
                {
                    var hashCode = 0;
                    foreach (var roleType in roleTypes)
                    {
                        hashCode = hashCode ^ roleType.GetHashCode();
                    }

                    return hashCode;
                }
            }
        }
    }
}