// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleContainedInEnumerable.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Object.Npgsql
{
    using System.Collections.Generic;
    using System.Text;

    using Adapters;

    using Meta;

    internal sealed class RoleContainedInEnumerable : In
    {
        private readonly IEnumerable<IObject> enumerable;
        private readonly IRoleType role;

        internal RoleContainedInEnumerable(ExtentFiltered extent, IRoleType role, IEnumerable<IObject> enumerable)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContainedIn(role, enumerable);
            this.role = role;
            this.enumerable = enumerable;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;

            var inStatement = new StringBuilder("0");
            foreach (var inObject in this.enumerable)
            {
                inStatement.Append(",");
                inStatement.Append(inObject.Id);
            }

            if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveClasses)
            {
                // TODO: in combination with NOT gives error
                statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IN (");
                statement.Append(inStatement.ToString());
                statement.Append(" ))");
            }
            else
            {
                if (this.role.IsMany)
                {
                    statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
                    statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IN (");
                    statement.Append(inStatement.ToString());
                    statement.Append(" ))");
                }
                else
                {
                    statement.Append(" (" + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NOT NULL AND ");
                    statement.Append(" " + schema.ColumnNameByRelationType[this.role.RelationType] + " IN (");
                    statement.Append(inStatement.ToString());
                    statement.Append(" ))");
                }
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
        }
    }
}
