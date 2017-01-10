// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleContainedInEnumerable.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

using Allors;
using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    using System.Collections.Generic;
    using System.Text;

    using Adapters;

    using Meta;

    internal sealed class RoleContainedInEnumerable : AllorsPredicateSql
    {
        private readonly IEnumerable<IObject> enumerable;
        private readonly IRoleType role;

        public RoleContainedInEnumerable(AllorsExtentFilteredSql extent, IRoleType role, IEnumerable<IObject> enumerable)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContainedIn(role, this.enumerable);
            this.role = role;
            this.enumerable = enumerable;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            var inStatement = new StringBuilder("0");
            foreach (var inObject in this.enumerable)
            {
                inStatement.Append(",");
                inStatement.Append(inObject.Id);
            }

            statement.Append(" (" + this.role.SingularPropertyName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
            statement.Append(" " + this.role.SingularPropertyName + "_R." + Mapping.ColumnNameForAssociation + " IN (");
            statement.Append(inStatement.ToString());
            statement.Append(" ))");

            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.role);
        }
    }
}