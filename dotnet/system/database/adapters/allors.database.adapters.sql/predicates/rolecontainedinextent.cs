// <copyright file="RoleContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;

    using Meta;

    internal sealed class RoleContainedInExtent : In
    {
        private readonly SqlExtent inExtent;
        private readonly IRoleType role;

        internal RoleContainedInExtent(ExtentFiltered extent, IRoleType role, Allors.Database.Extent inExtent)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleContainedIn(role, inExtent);
            this.role = role;
            this.inExtent = ((Extent)inExtent).ContainedInExtent;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            var inStatement = statement.CreateChild(this.inExtent, this.role);

            inStatement.UseAssociation(this.role.AssociationType);

            if ((this.role.IsMany && this.role.RelationType.AssociationType.IsMany) || !this.role.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForAssociation + " IN (");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))");
            }
            else if (this.role.IsMany)
            {
                statement.Append(" (" + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
                statement.Append(" " + this.role.SingularFullName + "_R." + Mapping.ColumnNameForObject + " IN (");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))");
            }
            else
            {
                statement.Append(" (" + schema.ColumnNameByRelationType[this.role.RelationType] + " IS NOT NULL AND ");
                statement.Append(" " + schema.ColumnNameByRelationType[this.role.RelationType] + " IN (");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.role);
    }
}
