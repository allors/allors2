//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleInstanceofSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsPredicateRoleInstanceofSql type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SQLite
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateRoleInstanceofSql : AllorsPredicateSql
    {
        private readonly IClass[] instanceClasses;
        private readonly IRoleType role;

        internal AllorsPredicateRoleInstanceofSql(AllorsExtentFilteredSql extent, IRoleType role, IObjectType instanceType, IClass[] instanceClasses)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleInstanceOf(role, instanceType);
            this.role = role;
            this.instanceClasses = instanceClasses;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            if (this.instanceClasses.Length == 1)
            {
                statement.Append(" (" + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForType + " IS NOT NULL AND ");
                statement.Append(" " + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForType + "=" + statement.AddParameter(this.instanceClasses[0].Id) + ")");
            }
            else if (this.instanceClasses.Length > 1)
            {
                statement.Append(" ( ");
                for (int i = 0; i < this.instanceClasses.Length; i++)
                {
                    statement.Append(" (" + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForType + " IS NOT NULL AND ");
                    statement.Append(" " + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForType + "=" + statement.AddParameter(this.instanceClasses[i].Id) + ")");
                    if (i < this.instanceClasses.Length - 1)
                    {
                        statement.Append(" OR ");
                    }
                }

                statement.Append(" ) ");
            }

            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
            statement.UseRoleInstance(this.role);
        }
    }
}