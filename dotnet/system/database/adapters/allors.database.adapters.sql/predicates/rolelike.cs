// <copyright file="RoleLike.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class RoleLike : Predicate
    {
        private readonly IRoleType role;
        private readonly string like;

        internal RoleLike(ExtentFiltered extent, IRoleType role, string like)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleLikeFilter(role, like);
            this.role = role;
            this.like = like;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " LIKE " + statement.AddParameter(this.like));
            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.role);
    }
}
