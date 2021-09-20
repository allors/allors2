// <copyright file="RoleGreaterThanValue.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class RoleGreaterThanValue : Predicate
    {
        private readonly object obj;
        private readonly IRoleType roleType;

        internal RoleGreaterThanValue(ExtentFiltered extent, IRoleType roleType, object obj)
        {
            extent.CheckRole(roleType);
            PredicateAssertions.ValidateRoleGreaterThan(roleType, obj);
            this.roleType = roleType;
            this.obj = roleType.Normalize(obj);
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.roleType.RelationType] + ">" + statement.AddParameter(this.obj));
            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.roleType);
    }
}
