// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleExists.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.SqlClient
{
    using Allors.Meta;
    using Adapters;

    internal sealed class RoleExists : Predicate
    {
        private readonly IRoleType role;

        internal RoleExists(ExtentFiltered extent, IRoleType role)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleExists(role);
            this.role = role;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if (this.role.ObjectType.IsUnit)
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NOT NULL");
            }
            else
            {
                if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveClasses)
                {
                    statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL");
                }
                else
                {
                    if (this.role.IsMany)
                    {
                        statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NOT NULL");
                    }
                    else
                    {
                        statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NOT NULL");
                    }
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
