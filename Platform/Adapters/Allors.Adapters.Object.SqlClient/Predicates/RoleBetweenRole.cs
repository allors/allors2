// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleBetweenRole.cs" company="Allors bvba">
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

    internal sealed class RoleBetweenRole : Predicate
    {
        private readonly IRoleType first;
        private readonly IRoleType role;
        private readonly IRoleType second;

        internal RoleBetweenRole(ExtentFiltered extent, IRoleType role, IRoleType first, IRoleType second)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleBetween(role, first, second);
            this.role = role;
            this.first = first;
            this.second = second;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " BETWEEN " + alias + "." + schema.ColumnNameByRelationType[this.first.RelationType] + " AND " + alias + "." + schema.ColumnNameByRelationType[this.second.RelationType] + ")");
            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.first);
            statement.UseRole(this.second);
        }
    }
}
