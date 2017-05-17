// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleGreaterThanRole.cs" company="Allors bvba">
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

    internal sealed class RoleGreaterThanRole : Predicate
    {
        private readonly IRoleType greaterThanRole;
        private readonly IRoleType role;

        internal RoleGreaterThanRole(ExtentFiltered extent, IRoleType role, IRoleType greaterThanRole)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleGreaterThan(role, greaterThanRole);
            this.role = role;
            this.greaterThanRole = greaterThanRole;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + ">" + alias + "." + schema.ColumnNameByRelationType[this.greaterThanRole.RelationType]);
            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.greaterThanRole);
        }
    }
}