// <copyright file="NotRoleContainedInEnumerable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System.Collections.Generic;
    using System.Text;

    using Adapters;

    using Allors.Meta;

    internal sealed class NotRoleContainedInEnumerable : In
    {
        private readonly IEnumerable<IObject> enumerable;
        private readonly IRoleType role;

        internal NotRoleContainedInEnumerable(ExtentFiltered extent, IRoleType role, IEnumerable<IObject> enumerable)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContainedIn(role, enumerable);
            this.role = role;
            this.enumerable = enumerable;
        }

        internal override bool IsNotFilter => true;

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
                statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NULL OR ");
                statement.Append(" NOT " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForAssociation + " IN (");
                statement.Append(" SELECT " + Mapping.ColumnNameForAssociation + " FROM " + schema.TableNameForRelationByRelationType[this.role.RelationType] + " WHERE " + Mapping.ColumnNameForRole + " IN (");
                statement.Append(inStatement.ToString());
                statement.Append(" )))");
            }
            else
            {
                if (this.role.IsMany)
                {
                    statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NULL OR ");
                    statement.Append(" NOT " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IN (");
                    statement.Append(inStatement.ToString());
                    statement.Append(" ))");
                }
                else
                {
                    statement.Append(" (" + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NULL OR ");
                    statement.Append(" NOT " + schema.ColumnNameByRelationType[this.role.RelationType] + " IN (");
                    statement.Append(inStatement.ToString());
                    statement.Append(" ))");
                }
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.role);
    }
}
