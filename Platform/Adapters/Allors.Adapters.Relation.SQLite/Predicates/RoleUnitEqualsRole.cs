//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleUnitEqualsRoleSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateRoleUnitEqualsRoleSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors.Meta;

namespace Allors.Adapters.Relation.SQLite
{
    using Adapters;

    internal sealed class AllorsPredicateRoleUnitEqualsRoleSql : AllorsPredicateSql
    {
        private readonly IRoleType equalsRole;
        private readonly IRoleType role;

        internal AllorsPredicateRoleUnitEqualsRoleSql(AllorsExtentFilteredSql extent, IRoleType role, IRoleType equalsRole)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleEquals(role, equalsRole);
            this.role = role;
            this.equalsRole = equalsRole;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + "=" + this.equalsRole.SingularFullName + "_R." + Mapping.ColumnNameForRole);
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.equalsRole);
        }
    }
}