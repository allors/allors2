//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleBetweenValueSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateRoleBetweenValueSql type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SqlClient
{
    using System;

    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateRoleBetweenValueSql : AllorsPredicateSql
    {
        private readonly object first;
        private readonly IRoleType roleType;
        private readonly object second;

        internal AllorsPredicateRoleBetweenValueSql(AllorsExtentFilteredSql extent, IRoleType roleType, Object first, Object second)
        {
            extent.CheckRole(roleType);
            PredicateAssertions.ValidateRoleBetween(roleType, first, second);
            this.roleType = roleType;
            this.first = roleType.ObjectType is IUnit ? roleType.Normalize(first) : first;
            this.second = roleType.ObjectType is IUnit ? roleType.Normalize(second) : second;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" " + this.roleType.SingularFullName + "_R." + Mapping.ColumnNameForRole + " BETWEEN " + statement.AddParameter(this.first) + " AND " + statement.AddParameter(this.second) + " ");
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.roleType);
        }
    }
}