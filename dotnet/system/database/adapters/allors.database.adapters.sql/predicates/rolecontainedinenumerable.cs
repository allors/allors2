// <copyright file="RoleContainedInEnumerable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
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

            if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveDatabaseClasses)
            {
                // TODO: in combination with NOT gives error
                statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IN (");
                statement.Append(inStatement.ToString());
                statement.Append(" ))");
            }
            else if (this.role.IsMany)
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

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.role);
    }
}
