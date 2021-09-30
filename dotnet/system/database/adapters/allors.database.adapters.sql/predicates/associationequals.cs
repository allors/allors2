// <copyright file="AssociationEquals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class AssociationEquals : Predicate
    {
        private readonly IObject allorsObject;
        private readonly IAssociationType association;

        internal AssociationEquals(ExtentFiltered extent, IAssociationType association, IObject allorsObject)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.AssertAssociationEquals(association, allorsObject);
            this.association = association;
            this.allorsObject = allorsObject;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if ((this.association.IsMany && this.association.RelationType.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL AND ");
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + "=" + this.allorsObject.Strategy.ObjectId + ")");
            }
            else if (this.association.RelationType.RoleType.IsMany)
            {
                statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NOT NULL AND ");
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + "=" + this.allorsObject.Strategy.ObjectId + ")");
            }
            else
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NOT NULL AND ");
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " =" + this.allorsObject.Strategy.ObjectId + ")");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
    }
}
