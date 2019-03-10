// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetCompositeRoleFactory.cs" company="Allors bvba">
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
    using System.Data;

    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class SetCompositeRoleFactory
    {
        public readonly Database Database;
        private readonly Dictionary<IRoleType, string> sqlByRoleType;

        public SetCompositeRoleFactory(Database database)
        {
            this.Database = database;
            this.sqlByRoleType = new Dictionary<IRoleType, string>();
        }

        public SetCompositeRole Create(Sql.DatabaseSession session)
        {
            return new SetCompositeRole(this, session);
        }

        public string GetSql(IRoleType roleType)
        {
            if (!this.sqlByRoleType.ContainsKey(roleType))
            {
                var associationType = roleType.AssociationType;

                string sql;
                if (!roleType.RelationType.ExistExclusiveClasses)
                {
                    sql = Sql.Schema.AllorsPrefix + "S_" + roleType.SingularFullName;
                }
                else
                {
                    sql = Sql.Schema.AllorsPrefix + "S_" + associationType.ObjectType.ExclusiveClass.Name + "_" + roleType.SingularPropertyName;
                }

                this.sqlByRoleType[roleType] = sql;
            }

            return this.sqlByRoleType[roleType];
        }

        public class SetCompositeRole : DatabaseCommand
        {
            private readonly SetCompositeRoleFactory factory;
            private readonly Dictionary<IRoleType, NpgsqlCommand> commandByRoleType;

            public SetCompositeRole(SetCompositeRoleFactory factory, Sql.DatabaseSession session)
                : base((DatabaseSession)session)
            {
                this.factory = factory;
                this.commandByRoleType = new Dictionary<IRoleType, NpgsqlCommand>();
            }

            public void Execute(IList<CompositeRelation> relations, IRoleType roleType)
            {
                NpgsqlCommand command;
                if (!this.commandByRoleType.TryGetValue(roleType, out command))
                {
                    command = this.Session.CreateNpgsqlCommand(this.factory.GetSql(roleType));
                    command.CommandType = CommandType.StoredProcedure;
                    this.commandByRoleType[roleType] = command;

                    this.AddInTable(command, this.Database.NpgsqlSchema.ObjectArrayParam, this.Database.CreateAssociationTable(relations));
                    this.AddInTable(command, this.Database.NpgsqlSchema.CompositeRelationArrayParam, this.Database.CreateRoleTable(relations));
                }
                else
                {
                    this.SetInTable(command, this.Database.NpgsqlSchema.ObjectArrayParam, this.Database.CreateAssociationTable(relations));
                    this.SetInTable(command, this.Database.NpgsqlSchema.CompositeRelationArrayParam, this.Database.CreateRoleTable(relations));
                }

                command.ExecuteNonQuery();
            }
        }
    }
}