// <copyright file="RoleExists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

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
            else if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL");
            }
            else if (this.role.IsMany)
            {
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NOT NULL");
            }
            else
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NOT NULL");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.role);
    }
}
