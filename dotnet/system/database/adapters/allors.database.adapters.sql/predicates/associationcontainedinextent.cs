// <copyright file="AssociationContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;

    using Meta;

    internal sealed class AssociationContainedInExtent : In
    {
        private readonly IAssociationType association;
        private readonly SqlExtent inExtent;

        internal AssociationContainedInExtent(ExtentFiltered extent, IAssociationType association, Allors.Database.Extent inExtent)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContainedIn(association, inExtent);
            this.association = association;
            this.inExtent = ((Extent)inExtent).ContainedInExtent;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            var inStatement = statement.CreateChild(this.inExtent, this.association);

            inStatement.UseRole(this.association.RoleType);

            if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL AND ");
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IN (\n");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))\n");
            }
            else if (this.association.RoleType.IsMany)
            {
                statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NOT NULL AND ");
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IN (\n");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))\n");
            }
            else
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IN (\n");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))\n");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
    }
}
