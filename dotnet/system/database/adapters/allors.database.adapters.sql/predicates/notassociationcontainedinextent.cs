// <copyright file="NotAssociationContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;

    using Meta;

    internal sealed class NotAssociationContainedInExtent : In
    {
        private readonly IAssociationType association;
        private readonly SqlExtent inExtent;

        internal NotAssociationContainedInExtent(ExtentFiltered extent, IAssociationType association, Allors.Database.Extent inExtent)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContainedIn(association, inExtent);
            this.association = association;
            this.inExtent = ((Extent)inExtent).ContainedInExtent;
        }

        internal override bool IsNotFilter => true;

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            var inStatement = statement.CreateChild(this.inExtent, this.association);

            inStatement.UseRole(this.association.RoleType);

            if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IS NULL OR");
                statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IN (\n");
                statement.Append(" SELECT " + Mapping.ColumnNameForRole + " FROM " + schema.TableNameForRelationByRelationType[this.association.RelationType] + " WHERE " + Mapping.ColumnNameForAssociation + " IN (");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" )))\n");
            }
            else if (this.association.RoleType.IsMany)
            {
                statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NULL OR ");
                statement.Append(" NOT " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IN (\n");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))\n");
            }
            else
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NULL OR ");
                statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IN (\n");
                this.inExtent.BuildSql(inStatement);
                statement.Append(" ))\n");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
    }
}
