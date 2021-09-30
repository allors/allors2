// <copyright file="NotAssociationContainedInEnumerable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Collections.Generic;
    using System.Text;

    using Adapters;

    using Meta;

    internal sealed class NotAssociationContainedInEnumerable : In
    {
        private readonly IAssociationType association;
        private readonly IEnumerable<IObject> enumerable;

        internal NotAssociationContainedInEnumerable(ExtentFiltered extent, IAssociationType association, IEnumerable<IObject> enumerable)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContainedIn(association, this.enumerable);
            this.association = association;
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
                inStatement.Append(inObject.Id.ToString());
            }

            if ((this.association.IsMany && this.association.RelationType.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IS NULL OR ");
                statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IN (\n");
                statement.Append(" SELECT " + Mapping.ColumnNameForRole + " FROM " + schema.TableNameForRelationByRelationType[this.association.RelationType] + " WHERE " + Mapping.ColumnNameForAssociation + " IN (");
                statement.Append(inStatement.ToString());
                statement.Append(" ))\n");
            }
            else if (this.association.RelationType.RoleType.IsMany)
            {
                statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NULL OR ");
                statement.Append(" NOT " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IN (\n");
                statement.Append(inStatement.ToString());
                statement.Append(" ))\n");
            }
            else
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NULL OR ");
                statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IN (\n");
                statement.Append(inStatement.ToString());
                statement.Append(" ))\n");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
    }
}
