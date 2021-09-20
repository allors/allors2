// <copyright file="AssociationContains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class AssociationContains : Predicate
    {
        private readonly IObject allorsObject;
        private readonly IAssociationType association;

        internal AssociationContains(ExtentFiltered extent, IAssociationType association, IObject allorsObject)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationContains(association, allorsObject);
            this.association = association;
            this.allorsObject = allorsObject;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if ((this.association.IsMany && this.association.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append("\n");
                statement.Append("EXISTS(\n");
                statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject + "\n");
                statement.Append("FROM " + schema.TableNameForRelationByRelationType[this.association.RelationType] + "\n");
                statement.Append("WHERE " + Mapping.ColumnNameForAssociation + "=" + this.allorsObject.Strategy.ObjectId + "\n");
                statement.Append("AND " + Mapping.ColumnNameForRole + "=" + alias + "." + Mapping.ColumnNameForObject + "\n");
                statement.Append(")");
            }
            else
            {
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " = " + this.allorsObject.Strategy.ObjectId);
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
    }
}
