// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotAssociationContainedInEnumerable.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
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

            if ((this.association.IsMany && this.association.RelationType.RoleType.IsMany) || !this.association.RelationType.ExistExclusiveClasses)
            {
                statement.Append(" (" + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IS NULL OR ");
                statement.Append(" NOT " + this.association.SingularFullName + "_A." + Mapping.ColumnNameForRole + " IN (\n");
                statement.Append(" SELECT " + Mapping.ColumnNameForRole + " FROM " + schema.TableNameForRelationByRelationType[this.association.RelationType] + " WHERE " + Mapping.ColumnNameForAssociation + " IN (");
                statement.Append(inStatement.ToString());
                statement.Append(" ))\n");
            }
            else
            {
                if (this.association.RelationType.RoleType.IsMany)
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
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseAssociation(this.association);
        }
    }
}