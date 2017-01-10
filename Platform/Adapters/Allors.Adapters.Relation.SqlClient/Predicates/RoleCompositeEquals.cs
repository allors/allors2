//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleCompositeEqualsSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateRoleCompositeEqualsSql type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SqlClient
{
    using System;

    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateRoleCompositeEqualsSql : AllorsPredicateSql
    {
        private readonly object obj;
        private readonly IRoleType role;

        internal AllorsPredicateRoleCompositeEqualsSql(AllorsExtentFilteredSql extent, IRoleType role, Object obj)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleEquals(role, obj);
            this.role = role;
            this.obj = obj;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
            statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + "=" + statement.AddParameter(this.obj) + ")");
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
        }
    }
}