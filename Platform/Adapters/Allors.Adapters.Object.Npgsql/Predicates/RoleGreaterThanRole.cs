
// <copyright file="RoleGreaterThanRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
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
