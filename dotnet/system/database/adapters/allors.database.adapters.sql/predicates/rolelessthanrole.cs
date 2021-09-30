// <copyright file="RoleLessThanRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class RoleLessThanRole : Predicate
    {
        private readonly IRoleType lessThanRole;
        private readonly IRoleType role;

        internal RoleLessThanRole(ExtentFiltered extent, IRoleType role, IRoleType lessThanRole)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleLessThan(role, lessThanRole);
            this.role = role;
            this.lessThanRole = lessThanRole;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " < " + alias + "." + schema.ColumnNameByRelationType[this.lessThanRole.RelationType]);
            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.lessThanRole);
        }
    }
}
