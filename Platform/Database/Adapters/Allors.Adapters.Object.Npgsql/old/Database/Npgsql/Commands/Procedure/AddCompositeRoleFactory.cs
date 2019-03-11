// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddCompositeRoleFactory.cs" company="Allors bvba">
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

    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class AddCompositeRoleFactory
    {
        internal readonly Database Database;
        private readonly Dictionary<IRoleType, string> sqlByRoleType;

        internal AddCompositeRoleFactory(Database database)
        {
            this.Database = database;
            this.sqlByRoleType = new Dictionary<IRoleType, string>();
        }

        public AddCompositeRole Create(DatabaseSession session)
        {
            return new AddCompositeRole(this, session);
        }

        internal string GetSql(IRoleType roleType)
        {
            if (!this.sqlByRoleType.ContainsKey(roleType))
            {
                var associationType = roleType.AssociationType;

                string sql;
                if (associationType.IsMany || !roleType.RelationType.ExistExclusiveClasses)
                {
                    sql = Sql.Schema.AllorsPrefix + "A_" + roleType.SingularFullName;
                }
                else
                {
                    var compositeType = (IComposite)roleType.ObjectType;
                    sql = Sql.Schema.AllorsPrefix + "A_" + compositeType.ExclusiveClass.Name + "_" + associationType.SingularFullName;
                }
 
                this.sqlByRoleType[roleType] = sql;
            }

            return this.sqlByRoleType[roleType];
        }

        public class AddCompositeRole
        {
            private readonly AddCompositeRoleFactory factory;

            private readonly DatabaseSession session;

            private readonly Dictionary<IRoleType, NpgsqlCommand> commandByRoleType;

            public AddCompositeRole(AddCompositeRoleFactory factory, DatabaseSession session)
            {
                this.factory = factory;
                this.session = session;
                this.commandByRoleType = new Dictionary<IRoleType, NpgsqlCommand>();
            }

            public void Execute(IList<CompositeRelation> relations, IRoleType roleType)
            {
                NpgsqlCommand command;
                if (!this.commandByRoleType.TryGetValue(roleType, out command))
                {
                    command = this.session.CreateNpgsqlCommand(this.factory.GetSql(roleType));
                    command.CommandType = CommandType.StoredProcedure;
                    NpgsqlCommandExtensions.AddInTable(command, this.session.Schema.ObjectArrayParam, this.session.Database.CreateAssociationTable(relations));
                    NpgsqlCommandExtensions.AddInTable(command, this.session.Schema.CompositeRelationArrayParam, this.session.Database.CreateRoleTable(relations));

                    this.commandByRoleType[roleType] = command;
                }
                else
                {
                    NpgsqlCommandExtensions.SetInTable(command, this.session.Schema.ObjectArrayParam, this.session.Database.CreateAssociationTable(relations));
                    NpgsqlCommandExtensions.SetInTable(command, this.session.Schema.CompositeRelationArrayParam, this.session.Database.CreateRoleTable(relations));
                }

                command.ExecuteNonQuery();
            }
        }
    }
}