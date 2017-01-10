//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleGreaterThanSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateRoleGreaterThanSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    using Adapters;

    internal sealed class AllorsPredicateRoleGreaterThanSql : AllorsPredicateSql
    {
        private readonly IRoleType greaterThanRole;
        private readonly IRoleType role;

        internal AllorsPredicateRoleGreaterThanSql(AllorsExtentFilteredSql extent, IRoleType role, IRoleType greaterThanRole)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleGreaterThan(role, greaterThanRole);
            this.role = role;
            this.greaterThanRole = greaterThanRole;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " > " + this.greaterThanRole.SingularFullName + "_R." + Mapping.ColumnNameForRole);
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.greaterThanRole);
        }
    }
}