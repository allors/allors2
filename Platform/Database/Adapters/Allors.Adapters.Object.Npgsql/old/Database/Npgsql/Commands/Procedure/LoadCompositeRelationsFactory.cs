// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadCompositeRelationsFactory.cs" company="Allors bvba">
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

    public class LoadCompositeRelationsFactory
    {
        internal readonly Npgsql.ManagementSession ManagementSession;
        
        internal LoadCompositeRelationsFactory(Npgsql.ManagementSession session)
        {
            this.ManagementSession = session;
        }

        public LoadCompositeRelations Create(IRoleType roleType)
        {
            var associationType = roleType.AssociationType;

            string sql;
            if (roleType.IsMany)
            {
                if (associationType.IsMany || !roleType.RelationType.ExistExclusiveClasses)
                {
                    sql = Schema.AllorsPrefix + "A_" + roleType.SingularFullName;
                }
                else
                {
                    var compositeType = (IComposite)roleType.ObjectType;
                    sql = Schema.AllorsPrefix + "A_" + compositeType.ExclusiveClass.Name + "_" + associationType.SingularFullName;
                }
            }
            else
            {
                if (!roleType.RelationType.ExistExclusiveClasses)
                {
                    sql = Schema.AllorsPrefix + "S_" + roleType.SingularFullName;
                }
                else
                {
                    sql = Schema.AllorsPrefix + "S_" + associationType.ObjectType.ExclusiveClass.Name + "_" + roleType.SingularPropertyName;
                }
            }

            return new LoadCompositeRelations(this, sql);
        }

        public class LoadCompositeRelations
        {
            private readonly LoadCompositeRelationsFactory factory;
            private readonly string sql;

            public LoadCompositeRelations(LoadCompositeRelationsFactory factory, string sql)
            {
                this.factory = factory;
                this.sql = sql;
            }

            public void Execute(IList<CompositeRelation> relations)
            {
                var database = this.factory.ManagementSession.NpgsqlDatabase;

                var command = this.factory.ManagementSession.CreateNpgsqlCommand(this.sql);
                command.CommandType = CommandType.StoredProcedure;
                Commands.NpgsqlCommandExtensions.AddInTable(command, database.NpgsqlSchema.ObjectArrayParam, database.CreateAssociationTable(relations));
                Commands.NpgsqlCommandExtensions.AddInTable(command, database.NpgsqlSchema.CompositeRelationArrayParam, database.CreateRoleTable(relations));

                command.ExecuteNonQuery();
            }
        }
    }
}