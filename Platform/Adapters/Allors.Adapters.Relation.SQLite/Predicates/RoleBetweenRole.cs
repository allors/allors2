//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleBetweenRoleSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateRoleBetweenRoleSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors.Meta;

namespace Allors.Adapters.Relation.SQLite
{
    using Adapters;

    internal sealed class AllorsPredicateRoleBetweenRoleSql : AllorsPredicateSql
    {
        private readonly IRoleType first;
        private readonly IRoleType role;
        private readonly IRoleType second;

        internal AllorsPredicateRoleBetweenRoleSql(AllorsExtentFilteredSql extent, IRoleType role, IRoleType first, IRoleType second)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleBetween(role, first, second);
            this.role = role;
            this.first = first;
            this.second = second;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" " + role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " BETWEEN " + first.SingularFullName + "_R." + Mapping.ColumnNameForRole + " AND " + second.SingularFullName + "_R." + Mapping.ColumnNameForRole + " ");
            return Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(role);
            statement.UseRole(first);
            statement.UseRole(second);
        }
    }
}