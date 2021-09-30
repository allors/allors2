// <copyright file="AssociationExists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class AssociationExists : Predicate
    {
        private readonly IAssociationType association;

        internal AssociationExists(ExtentFiltered extent, IAssociationType association)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.ValidateAssociationExists(association);
            this.association = association;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if ((this.association.IsMany && this.association.RelationType.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveDatabaseClasses)
            {
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL");
            }
            else if (this.association.RelationType.RoleType.IsMany)
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.association.RelationType] + " IS NOT NULL");
            }
            else
            {
                statement.Append(" " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForObject + " IS NOT NULL");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseAssociation(this.association);
    }
}
